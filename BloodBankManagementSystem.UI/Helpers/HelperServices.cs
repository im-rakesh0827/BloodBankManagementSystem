using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace BloodBankManagementSystem.UI.Helpers
{
    public class HelperServices
    {
        private readonly AuthenticationStateProvider _authStateProvider;

    public HelperServices(AuthenticationStateProvider authStateProvider)
    {
        _authStateProvider = authStateProvider;
    }

    public async Task<string> GetLoggedInUserIdAsync()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;
        return user.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
    }
        
    }    
    
}