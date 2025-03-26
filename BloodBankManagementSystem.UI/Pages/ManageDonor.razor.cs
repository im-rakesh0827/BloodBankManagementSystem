using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BloodBankManagementSystem.Core.Models;
using Microsoft.AspNetCore.Components;

namespace BloodBankManagementSystem.UI.Pages
{
     public partial class ManageDonor{
     private List<Donor> AllDonorsList = new();
     private List<Donor> FilteredDonorsList = new();

     private string Message{get; set;} = string.Empty;
     private bool IsCreateUpdatePopup{get; set;}
     public bool IsLoading {get; set;} = false;
     private Donor SelectedDonor = new Donor();
     protected override async Task OnInitializedAsync()
     {
          await FetchDonors();
     }
    private async Task FetchDonors()
    {
     //   IsLoading = true;
     //   await Task.Delay(1500);

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
     //   IsLoading = false;
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

     private async Task DeleteDonor(int donorId)
    {
        await Http.DeleteAsync($"api/donors/{donorId}");
        AllDonorsList.RemoveAll(d => d.Id == donorId);
    }

    private async Task RefreshDonorList()
    {
        IsCreateUpdatePopup = false;
        await FetchDonors();
        ApplyFilterDonorsList();
        StateHasChanged(); 
    }

    private async Task ApplyFilterDonorsList(){
          FilteredDonorsList = AllDonorsList.Where(x=>x.IsEligible).ToList();
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