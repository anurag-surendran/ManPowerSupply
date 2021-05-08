using ManPowerSupplyAPI.DatabaseManagement;
using ManPowerSupplyAPI.DatabaseManagement.Entities;
using ManPowerSupplyAPI.Helpers;
using ManPowerSupplyAPI.Models;
using ManPowerSupplyAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ManPowerSupplyAPI.Services
{
    public class UserService : IUserRepository
    {
        private readonly AppSettings _appSettings;
        private readonly ManPowerDbContext _context;

        public UserService(IOptions<AppSettings> appSettings, ManPowerDbContext context)
        {
            _appSettings = appSettings.Value;
            _context = context;
        }


        UserResponseModel IUserRepository.Authenticate(AuthenticateRequest model, string ipAddress)
        {
            var user = _context.Users.SingleOrDefault(x => x.UserName == model.Username); 

            // return null if user not found
            if (user == null) return null;

            //validate password
            PasswordHasher passwordHasher = new PasswordHasher();
            var (Verified, NeedsUpgrade) = passwordHasher.Check(user.Password, model.Password);
            if (!Verified)
                return null;

            user.UserRoleMappings = (from role in _context.UserRoleMappings.Include(x => x.Role)
                                     where role.UserId == user.Id
                                     select role).ToList();
            user.UserOrganizationMappings = (from org in _context.UserOrganizationMappings.Include(x => x.Organization)
                                             where org.UserId == user.Id
                                             select org).ToList();

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);
            var refreshToken = generateRefreshToken(ipAddress);


            if (user.RefreshTokens == null)
                user.RefreshTokens = new List<RefreshTokenEntity>();

            var userResponse = new UserResponseModel(user, token, refreshToken.Token);

            LoggedUser.Instance.CurrentUser = userResponse;

            user.RefreshTokens.Add(refreshToken);
            _context.Update(user);
            _context.SaveChanges();

            return userResponse;
        }

        UserResponseModel IUserRepository.RefreshToken(string token, string ipAddress)
        {
            var user = _context.Users.Include(x=>x.RefreshTokens).SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            // return null if no user found with token
            if (user == null) return null;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            var IsExpiredRefreshToken = DateTime.UtcNow >= refreshToken.Expires;
            var IsActiveRefreshToken = refreshToken.Revoked == null && !IsExpiredRefreshToken;
            // return null if token is no longer active
            if (!IsActiveRefreshToken) return null;

            // replace old refresh token with a new one and save
            var newRefreshToken = generateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            user.RefreshTokens.Add(newRefreshToken);
            _context.Update(user);
            _context.SaveChanges();

            user.UserRoleMappings = (from role in _context.UserRoleMappings.Include(x => x.Role)
                                     where role.UserId == user.Id
                                     select role).ToList();
            user.UserOrganizationMappings = (from org in _context.UserOrganizationMappings.Include(x => x.Organization)
                                             where org.UserId == user.Id
                                             select org).ToList();

            // generate new jwt
            var jwtToken = generateJwtToken(user);

            return new UserResponseModel(user, jwtToken, newRefreshToken.Token);
        }

        bool IUserRepository.RevokeToken(string token, string ipAddress)
        {
            var user = _context.Users.Include(x=>x.RefreshTokens).SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

            // return false if no user found with token
            if (user == null) return false;

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

            var IsExpiredRefreshToken = DateTime.UtcNow >= refreshToken.Expires;
            var IsActiveRefreshToken = refreshToken.Revoked == null && !IsExpiredRefreshToken;

            // return false if token is not active
            if (!IsActiveRefreshToken) return false;

            // revoke token and save
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            _context.Update(user);
            _context.SaveChanges();

            return true;
        }

        UserResponseModel IUserRepository.GetById(int id)
        {
            var userEntity =  _context.Users.FirstOrDefault(x => x.Id == id);

            userEntity.UserRoleMappings = (from role in _context.UserRoleMappings.Include(x => x.Role)
                                     where role.UserId == userEntity.Id
                                     select role).ToList();
            userEntity.UserOrganizationMappings = (from org in _context.UserOrganizationMappings.Include(x => x.Organization)
                                             where org.UserId == userEntity.Id
                                             select org).ToList();

            return new UserResponseModel(userEntity);
        }

        private string generateJwtToken(UserEntity user)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName+" "+user.LastName),
                new Claim(ClaimTypes.NameIdentifier, user.UserName)
                }),

                Expires = DateTime.UtcNow.AddDays(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            foreach (var role in user.UserRoleMappings)
            {
                tokenDescriptor.Subject.AddClaim(new Claim(
                    ClaimTypes.Role, role.Role.Name
                ));
            }
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private RefreshTokenEntity generateRefreshToken(string ipAddress)
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return new RefreshTokenEntity
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow,
                CreatedByIp = ipAddress
            };
        }

        
    }
}
