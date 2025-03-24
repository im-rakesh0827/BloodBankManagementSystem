using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BloodBankManagementSystem.Core.Models;
using Microsoft.AspNetCore.Components;
namespace BloodBankManagementSystem.UI.Pages{
     public partial class RegisterUser
     {
          private User User { get; set; } = new();
          private string ConfirmPassword { get; set; } = string.Empty;
          public string ErrorMessage { get; set; } = string.Empty;
          private async Task SaveUser()
          {
               ErrorMessage = string.Empty;
               if (User.PasswordHash != ConfirmPassword)
               {
                    ErrorMessage = "Passwords do not match.";
                    return;
               }

               var response = await Http.PostAsJsonAsync("api/Users/register", User);
               if (response.IsSuccessStatusCode)
               {
                    ErrorMessage = "User registered successfully!";
                    User = new User();
               }
               else
               {
                    ErrorMessage = "Registration failed.";
               }
          }
     }
}
