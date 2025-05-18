using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BloodBankManagementSystem.Core.Models;
using BloodBankManagementSystem.UI.Pages;
using BloodBankManagementSystem.UI.Helpers;
using BloodBankManagementSystem.Core.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using Microsoft.JSInterop;
namespace BloodBankManagementSystem.UI.Pages.Users
{
     public partial class UserHistoryComponent{

     [Parameter] public EventCallback OnClose { get; set; }
    [Parameter] public User? UserDetails { get; set; }
    public List<UserHistory> UserHistoryList { get; set; } = new();
    private string FullName {get; set;} = string.Empty;


     private void Close()
    {
        OnClose.InvokeAsync();
    }


    protected override async Task OnInitializedAsync(){
        FullName = UserDetails.FirstName+" "+UserDetails.LastName;
        UserHistoryList = await GetUserHistory(UserDetails.Id);
    }


    private async Task<List<UserHistory>> GetUserHistory(int userId)
        {
            try
            {
                var url = $"{ServerConstants.APICallNames.GetUserHistoryById.GetStringValue()}{userId}";
                var response = await Http.GetFromJsonAsync<List<UserHistory>>(url);
                return response ?? new List<UserHistory>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching patient history: {ex.Message}");
                return new List<UserHistory>();
            }
        }

          

     }
}