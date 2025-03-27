using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BloodBankManagementSystem.Core.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using Microsoft.JSInterop;
namespace BloodBankManagementSystem.UI.Pages
{
     public partial class ManageDonor{
     private List<Donor> AllDonorsList = new();
     private List<Donor> FilteredDonorsList = new();
     private string Message{get; set;} = string.Empty;
     private bool IsCreateUpdatePopup{get; set;}
     public bool IsLoading {get; set;} = false;
     private Donor SelectedDonor = new Donor();
     private int donorIdToDelete{get;set;} 
     private DotNetObjectReference<ManageDonor> objRef;

     protected override async Task OnInitializedAsync()
     {
          await FetchDonors();
          await ApplyFilterDonorsList();
     }
    private async Task FetchDonors()
    {
       var response = await Http.GetAsync("api/donors/allDonors");
       if (!response.IsSuccessStatusCode)
       {
          var errorText = await response.Content.ReadAsStringAsync();
          Console.WriteLine($"API Error: {response.StatusCode}, Response: {errorText}");
          
          if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
          {
          Message = "No donors found.";
          }
          else
          {
          Message = $"Error fetching patients: {errorText}";
          }
          return;
       }
       AllDonorsList = await response.Content.ReadFromJsonAsync<List<Donor>>();
    }

    private void OpenCreatePopup()
    {
        SelectedDonor = new Donor();
        IsCreateUpdatePopup = true;
    }

    private void OpenEditModal(Donor donor)
    {
        SelectedDonor = donor;
        IsCreateUpdatePopup = true;
    }

    protected override void OnInitialized()
    {
        objRef = DotNetObjectReference.Create(this);
    }

    private async Task DeleteDonor(int donorId)
    {
        donorIdToDelete = donorId;
        await JSRuntime.InvokeVoidAsync("ShowDeleteConfirmation", objRef, "Donor");
    }

    [JSInvokable]
    public async Task PerformDelete()
    {
        try
        {
            await Http.DeleteAsync($"api/donors/delete/{donorIdToDelete}");
            AllDonorsList.RemoveAll(d => d.Id == donorIdToDelete);
            StateHasChanged();
        }
        catch (Exception ex)
        {
          throw ex;
        }
    }

    public void Dispose()
    {
        objRef?.Dispose();
    }

    private async Task RefreshDonorList()
    {
        IsCreateUpdatePopup = false;
        await FetchDonors();
        ApplyFilterDonorsList();
        StateHasChanged(); 
    }

    private async Task ApplyFilterDonorsList(){
          //FilteredDonorsList = AllDonorsList.Where(x=>x.IsEligible==true).ToList();
          FilteredDonorsList = AllDonorsList;
    }

    private void HandleCancelOrClose()
    {
        IsCreateUpdatePopup = false;
    }


    public bool CheckEligibility(Donor donor)
    {
          if (!donor.IsAlive || donor.Age < 18 || donor.Age > 65 || donor.Weight < 50 || donor.MedicalHistory.ToString()!="None")
               return false;
          if (donor.LastDonationDate.HasValue && (DateTime.Now - donor.LastDonationDate.Value).TotalDays / 30 < 3)
               return false;
          return true;
     }
    
   }
}