using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BloodBankManagementSystem.Core.Models;
using BloodBankManagementSystem.UI.Helpers;
using BloodBankManagementSystem.Core.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using BloodBankManagementSystem.UI.Shared;
using System.Linq;
using Microsoft.JSInterop;
namespace BloodBankManagementSystem.UI.Pages.Donors{

     public partial class DonorHistoryComponent{

    [Parameter] public EventCallback OnClose { get; set; }
    [Parameter] public Donor? DonorDetails { get; set; }
    private List<DonorHistory> DonorHistoryList = new();
    private string FullName{get; set;} = string.Empty;

    protected override async Task OnInitializedAsync(){
          FullName = DonorDetails.FirstName+" "+DonorDetails.LastName;
        DonorHistoryList = await GetDonorHistory(DonorDetails.Id); 
    }

    private void Close()
    {
        OnClose.InvokeAsync();
    }


private async Task<List<DonorHistory>> GetDonorHistory(int donorId)
{
    try
    {
        var url = $"{ServerConstants.APICallNames.GetDonorHistoryById.GetStringValue()}{donorId}";
        var response = await Http.GetFromJsonAsync<List<DonorHistory>>(url);
        return response ?? new List<DonorHistory>();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error fetching donor history: {ex.Message}");
        return new List<DonorHistory>();
    }
}

     }

}
