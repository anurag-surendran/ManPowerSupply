using ManPowerSupplyAPI.DatabaseManagement;
using ManPowerSupplyAPI.DatabaseManagement.Entities;
using ManPowerSupplyAPI.Helpers;
using ManPowerSupplyAPI.Models;
using ManPowerSupplyAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManPowerSupplyAPI.Services
{
    public class UserManagerService : IUserManagerRepository
    {
        private readonly ManPowerDbContext _context;

        public UserManagerService(ManPowerDbContext context)
        {
            _context = context;
        }

        async Task<UserResponseModel> IUserManagerRepository.AddUser(UserRequestModel requestModel)
        {
            if (_context.Users.Any(x => x.UserName == requestModel.UserName))
                throw new Exception("User name is already exists");

            PasswordHasher passwordHasher = new PasswordHasher();
            var hasedPassword = passwordHasher.Hash(requestModel.Password);

            var loggedUser = LoggedUser.Instance.CurrentUser;

            var userEntity = new UserEntity
            {
                FirstName = requestModel.FirstName,
                LastName = requestModel.LastName,
                UserName = requestModel.UserName,
                MobileNumber = requestModel.MobileNumber,
                EMail = requestModel.EMail,
                Password = hasedPassword,
                PasswordSalt = hasedPassword.Split('.')[1],
                AccountStatus = UserAccountStatus.Active,
                CreatedBy = loggedUser.Id,
                CreatedOn = DateTime.Now,

            };

            userEntity.UserOrganizationMappings = (from x in requestModel.Organizations
                                                   select new UserOrganizationMappingEntity
                                                   {
                                                       UserId = userEntity.Id,
                                                       OrganizationId = x.Id,
                                                       Organization = _context.Organizations.FirstOrDefault(org => org.Id == x.Id)
                                                   }).ToList();
            userEntity.UserRoleMappings = (from x in requestModel.Roles
                                           select new UserRoleMappingEntity
                                           {
                                               UserId = userEntity.Id,
                                               RoleId = x.Id,
                                               Role = _context.Roles.FirstOrDefault(role=>role.Id == x.Id)
                                           }).ToList();

            _context.Users.Add(userEntity);
            await _context.SaveChangesAsync();

            return new UserResponseModel(userEntity);
        }

        async Task<IEnumerable<UserOrganization>> IUserManagerRepository.GetAllOrganization()
        {
            var organizations = await (from x in _context.Organizations
                                 select new UserOrganization
                                 {
                                     Id = x.Id,
                                     Name = x.Name,
                                     Address = x.Address
                                 }).ToListAsync();
            return organizations;
        }

        async Task<IEnumerable<UserRole>> IUserManagerRepository.GetAllRoles()
        {
            var getAllRoles = await (from x in _context.Roles
                                     where !x.IsDeleted
                                     select new UserRole
                                     {
                                         Id = x.Id,
                                         Name = x.Name
                                     }).ToListAsync();
            return getAllRoles;
        }

        async Task<bool> IUserManagerRepository.UpdatePassword(UpdatePasswordRequestModel requestModel)
        {
            //validate User
            var userEntityQuery = from user in _context.Users where user.Id == requestModel.UserId select user;
            if (!userEntityQuery.Any())
                throw new Exception("user not found");
            var userEntity = userEntityQuery.First();

            //Validate Old Password 
            var existingHashedPassword = userEntity.Password;
            PasswordHasher passwordHasher = new PasswordHasher();
            var (Verified, NeedsUpgrade) = passwordHasher.Check(existingHashedPassword, requestModel.OldPassword);
            if (!Verified)
                throw new Exception("Existing password is not match with our database");

            //Create new hashed Password
            var newHashedPassword = passwordHasher.Hash(requestModel.NewPassword);

            //Update Password
            userEntity.Password = newHashedPassword;
            _context.Users.Update(userEntity);
            await _context.SaveChangesAsync();

            return true;
        }

        async Task<UserResponseModel> IUserManagerRepository.UpdateUser(UserRequestModel requestModel, long userId)
        {
            var userEntityQuery = from user in _context.Users where user.Id == userId select user;
            if (!userEntityQuery.Any())
                throw new Exception("user not found");
            var userEntity = userEntityQuery.First();

            userEntity.FirstName = requestModel.FirstName;
            userEntity.LastName = requestModel.LastName;
            userEntity.MobileNumber = requestModel.MobileNumber;
            userEntity.EMail = requestModel.EMail;
            userEntity.UpdatedBy = LoggedUser.Instance.CurrentUser.Id;
            userEntity.UpdatedOn = DateTime.Now;


            _context.Users.Update(userEntity);
            await _context.SaveChangesAsync();

            userEntity.UserRoleMappings = (from role in _context.UserRoleMappings.Include(x => x.Role)
                                     where role.UserId == userEntity.Id
                                     select role).ToList();
            userEntity.UserOrganizationMappings = (from org in _context.UserOrganizationMappings.Include(x => x.Organization)
                                             where org.UserId == userEntity.Id
                                             select org).ToList();
            return new UserResponseModel(userEntity);
        }

        async Task<bool> IUserManagerRepository.ValidatePassword(ValidatePasswordRequestModel requestModel)
        {
            var userEntityQuery = from user in _context.Users where user.Id == requestModel.UserId select user;
            if (!userEntityQuery.Any())
                throw new Exception("user not found");
            var userEntity = await userEntityQuery.FirstAsync();

            //Validate Old Password 
            var existingHashedPassword = userEntity.Password;
            PasswordHasher passwordHasher = new PasswordHasher();
            var (Verified, NeedsUpgrade) = passwordHasher.Check(existingHashedPassword, requestModel.Password);
            if (!Verified)
                throw new Exception("Existing password is not match with our database");

            return true;
        }
    }
}
