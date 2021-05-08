using ManPowerSupplyAPI.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ManPowerSupplyAPI.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public Task Invoke(HttpContext httpContext, IUserRepository userService)
        {
            LoggedUser.Instance.CurrentUser = null;
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (httpContext.Request.Method.Contains("RefreshToken"))
                token = null;
            try
            {
                if (!string.IsNullOrEmpty(token))
                    attachUserToContext(httpContext, userService, token);
            }
            catch (SecurityTokenExpiredException)
            {
                var statusCode = HttpStatusCode.Unauthorized;
                var result = JsonSerializer.Serialize(new
                {
                    statusCode = $"{(int)statusCode} - {statusCode}",
                    errorMessage = "Token has been expired"
                });

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)statusCode;
                return httpContext.Response.WriteAsync(result);
            }
            catch (SecurityTokenInvalidAudienceException)
            {
                var statusCode = HttpStatusCode.BadRequest;
                var result = JsonSerializer.Serialize(new
                {
                    statusCode = $"{(int)statusCode} - {statusCode}",
                    errorMessage = "Invalid Request!, Organization Id is missing"
                });

                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = (int)statusCode;
                return httpContext.Response.WriteAsync(result);
            }

            return _next(httpContext);
        }

        private void attachUserToContext(HttpContext context, IUserRepository userService, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                var user = userService.GetById(userId);

                if (!context.Request.Path.Value.EndsWith("api/User/SetOrganization"))
                {
                    var orrganizationId = context.Request.Headers["OrganizationId"].FirstOrDefault();

                    if (string.IsNullOrEmpty(orrganizationId))
                        orrganizationId = context.Request.Cookies["organizationId"];

                    if (string.IsNullOrEmpty(orrganizationId))
                        throw new SecurityTokenInvalidAudienceException();
                    else
                        LoggedUser.Instance.CurrentOrganization = Convert.ToInt64(orrganizationId);
                }

                // attach user to context on successful jwt validation
                context.Items["User"] = user;
                LoggedUser.Instance.CurrentUser = user;
            }
            catch (SecurityTokenExpiredException ex)
            {
                throw new SecurityTokenExpiredException();
            }
            catch (SecurityTokenInvalidAudienceException ex)
            {
                throw new SecurityTokenInvalidAudienceException();
            }
            catch (Exception ex)
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class JwtMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtMiddleware>();
        }
    }
}
