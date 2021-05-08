using ManPowerSupplyAPI.DatabaseManagement.Entities;
using ManPowerSupplyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManPowerSupplyAPI.Repositories
{
    public interface IUserRepository
    {
        UserResponseModel Authenticate(AuthenticateRequest model, string ipAddress);
        UserResponseModel RefreshToken(string token, string ipAddress);
        bool RevokeToken(string token, string ipAddress);
        UserResponseModel GetById(int id);

    }
}
