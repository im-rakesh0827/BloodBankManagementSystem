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
namespace BloodBankManagementSystem.UI.Pages.Donors
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
     private CustomPopup donorPopupRef;
    private Dictionary<string, string> FilterOptions = new Dictionary<string, string>();
    private string FilterBasedOn {get; set;} = "Active";



     protected override async Task OnInitializedAsync()
     {
          FilterOptions = FilterOptionsHelper.AllFilterOption;
          await FetchDonors();
          await ApplyFilterDonorsList();
     }
    private async Task FetchDonors()
    {
        var url = $"{ServerConstants.APICallNames.GetAllDonors.GetStringValue()}";
       var response = await Http.GetAsync(url);
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

    private void OpenCreatePopUp()
    {
        SelectedDonor = new Donor();
        IsCreateUpdatePopup = true;
        donorPopupRef?.Show(); 
    }

    private void OpenEditOrUpdatePopUp(Donor donor)
    {
        SelectedDonor = donor;
        IsCreateUpdatePopup = true;
        donorPopupRef?.Show();
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
            var url = $"{ServerConstants.APICallNames.DeleteDonor.GetStringValue()}{donorIdToDelete}";
            await Http.DeleteAsync(url);
            AllDonorsList.RemoveAll(d => d.Id == donorIdToDelete);
            await RefreshDonorList();
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

    private async Task ApplyFilterDonorsList1(){
          //FilteredDonorsList = AllDonorsList.Where(x=>x.IsEligible==true).ToList();
        //   FilteredDonorsList = AllDonorsList;

        switch (FilterBasedOn)
            {
              case "Active":
                  FilteredDonorsList = AllDonorsList.Where(p => p.IsActive && p.IsAlive).ToList();
              break;
              case "Last7Days":
                  var sevenDaysAgo = DateTime.Now.AddDays(-7);
                  FilteredDonorsList = AllDonorsList.Where(p => p.CreatedAt >= sevenDaysAgo && p.IsActive && p.IsAlive).ToList();
              break;
              case "Last30Days":
                    var thirtyDaysAgo = DateTime.Now.AddDays(-30);
                    FilteredDonorsList = AllDonorsList.Where(p => p.CreatedAt >= thirtyDaysAgo && p.IsActive && p.IsAlive).ToList();
                    break;
              case "Last1Year":
                    var oneYearAgo = DateTime.Now.AddYears(-1);
                    FilteredDonorsList = AllDonorsList.Where(p => p.CreatedAt >= oneYearAgo && p.IsActive && p.IsAlive).ToList();
              break;
              case "All":
                  FilteredDonorsList = AllDonorsList.Where(p=>p.IsActive).ToList();
              break;
              default:
                  FilteredDonorsList = AllDonorsList.Where(p => p.IsActive && p.IsAlive).ToList();
              break;
            }
    }


    //Filter logic in different way
    private async Task ApplyFilterDonorsList()
    {

        var now = DateTime.Now;
        var sevenDaysAgo = now.AddDays(-7);
        var thirtyDaysAgo = now.AddDays(-30);
        var oneYearAgo = now.AddYears(-1);
        FilteredDonorsList = FilterBasedOn switch
        {
            "Active"      => AllDonorsList.Where(p => p.IsActive && p.IsAlive).ToList(),
            "Last7Days"   => AllDonorsList.Where(p => p.CreatedAt >= sevenDaysAgo && p.IsActive && p.IsAlive).ToList(),
            "Last30Days"  => AllDonorsList.Where(p => p.CreatedAt >= thirtyDaysAgo && p.IsActive && p.IsAlive).ToList(),
            "Last1Year"   => AllDonorsList.Where(p => p.CreatedAt >= oneYearAgo && p.IsActive && p.IsAlive).ToList(),
            "All"         => AllDonorsList.Where(p => p.IsActive).ToList(),
            _             => AllDonorsList.Where(p => p.IsActive && p.IsAlive).ToList(),
        };
        await Task.CompletedTask;
    }



    private void HandleCancelOrClose()
    {
        IsCreateUpdatePopup = false;
        donorPopupRef?.Hide();
    }


    public bool CheckEligibility(Donor donor)
    {
          if (!donor.IsAlive || donor.Age < 18 || donor.Age > 65 || donor.Weight < 50 || donor.MedicalHistory.ToString()!="None")
               return false;
          if (donor.LastDonationDate.HasValue && (DateTime.Now - donor.LastDonationDate.Value).TotalDays / 30 < 3)
               return false;
          return true;
    }


    private bool ShowHistoryPopup = false;
    private async Task ShowDonorHistory(Donor donor)
    {
        SelectedDonor = donor;
        ShowHistoryPopup = true;
    }

    private void CloseHistoryPopup()
    {
        ShowHistoryPopup = false;
    }

       private async Task ExportToExcel()
       {
            var exportData = FilteredDonorsList.Select(p => new
            {
                p.FirstName,
                p.LastName
            }).ToList();
            await JSRuntime.InvokeVoidAsync("exportToExcel", exportData, "DonorsList.xlsx");
        }







        private async Task ExportCSV(){

               await ExportGridData.ExportGridToCsv(FilteredDonorsList, JSRuntime, "DonorsList.csv");
          }

          private async Task ExportExcel()
          {
               var fileBytes = ExportGridData.ExportToExcelBytes(FilteredDonorsList);
               var base64 = Convert.ToBase64String(fileBytes);
               await JSRuntime.InvokeVoidAsync("BlazorDownloadFile", "DonorsList.xlsx", 
               "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", base64);
          }



          private async Task HandleExportData(string exportType){
               switch(exportType.ToUpper()){
                    case "EXCEL":
                         ExportExcel();
                    break;
                    case "CSV":
                         ExportCSV();
                    break;
                    default:
                         ExportExcel();
                    break;
               }

          }

          public void HandleApplyFilter(string filterOption){
               FilterBasedOn = filterOption;
               ApplyFilterDonorsList();
          }
   }
}