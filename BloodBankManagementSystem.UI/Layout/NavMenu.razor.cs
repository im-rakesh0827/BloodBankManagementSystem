using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Blazored.LocalStorage;
using Radzen; // If you're using RadzenMenu events

namespace BloodBankManagementSystem.UI.Layout
{
    public partial class NavMenu : ComponentBase
    {
        [Inject] private ILocalStorageService LocalStorage { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;

        private bool IsAdmin = false;
        private bool IsManager = false;

        protected override async Task OnInitializedAsync()
        {
            var token = await LocalStorage.GetItemAsync<string>("authToken");
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role || c.Type == "role");

                if (roleClaim != null)
                {
                    var role = roleClaim.Value;
                    Console.WriteLine($"Role : {role}");

                    IsAdmin = role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
                    IsManager = role.Equals("Manager", StringComparison.OrdinalIgnoreCase);
                }
            }
        }

        void OnParentClicked(MenuItemEventArgs args)
        {
            Console.WriteLine($"{args.Text} clicked from parent");
        }

        void OnChildClicked(MenuItemEventArgs args)
        {
            Console.WriteLine($"{args.Text} clicked from child");
        }



    //     protected override async Task OnInitializedAsync()
    // {
    //     var role = await LocalStorage.GetItemAsync<string>("userRole");
    //     Console.WriteLine("Role : "+role);

    //     if (!string.IsNullOrEmpty(role))
    //     {
    //         IsAdmin = role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
    //         IsManager = role.Equals("Manager", StringComparison.OrdinalIgnoreCase);
    //     }
    // } 
    }
}
