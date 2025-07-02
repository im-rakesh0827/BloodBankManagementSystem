using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BloodBankManagementSystem.Core.Models;

namespace BloodBankManagementSystem.API.Services
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user);
    }
}