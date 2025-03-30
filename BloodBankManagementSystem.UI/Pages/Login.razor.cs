using BloodBankManagementSystem.UI.Services;
using BloodBankManagementSystem.Core.Models;
using System.Net.Http.Json; 
namespace BloodBankManagementSystem.UI.Pages{

    public partial class Login{

     private string email = "";
    private string password = "";
    private string errorMessage = "";
    LoginRequest loginRequest = new LoginRequest();

    private async Task HandleLogin()
    {
        var success = await AuthService.Login(loginRequest);
        if (!success)
        {
            errorMessage = "Invalid credentials!";
        }
        else
        {
            Navigation.NavigateTo("/");
        }
    }

    }


}
