using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BloodBankManagementSystem.Core.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using BloodBankManagementSystem.UI.Helpers;
using System.Linq;
using Microsoft.JSInterop;

namespace BloodBankManagementSystem.UI.Pages.Patients
{
  public partial class ManagePatients
    {
        private List<Patient> AllPatientsList { get; set; } = new();
        private List<Patient> FilteredPatientsList { get; set; } = new List<Patient>();
        private string Message { get; set; } = string.Empty;
        private Patient SelectedPatient { get; set; } = new();
        private bool IsCreateUpdatePopup{get;set;} = false;
        private string FilterBasedOn {get; set;} = "Active";
        private bool IsLoading{get; set;} = false;
        private void OpenEditModal(Patient patient)
        {
          SelectedPatient = patient;
          IsCreateUpdatePopup = true;
          StateHasChanged(); // Ensure UI updates
        }

        private void OpenCreatePoup(){
          IsCreateUpdatePopup = true;
          SelectedPatient = new Patient();
          StateHasChanged(); // Ensure UI updates
        }

        private void HandleCancelOrClose()
        {
          IsCreateUpdatePopup = false;
          StateHasChanged(); // Ensure UI updates
        }
        protected override async Task OnInitializedAsync()
        {
          await LoadAllPatients();
        }

        private async Task LoadAllPatients()
        {
               try
               {
                    var url = $"{ServerConstants.APICallNames.GetAllPatients.GetStringValue()}";
                    var response = await Http.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                    {
                         var errorText = await response.Content.ReadAsStringAsync();
                         Console.WriteLine($"API Error: {response.StatusCode}, Response: {errorText}");
                         
                         if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                         {
                         Message = "No patients found.";
                         }
                         else
                         {
                         Message = $"Error fetching patients: {errorText}";
                         }
                         return;
                    }

                    // var responseText = await response.Content.ReadAsStringAsync();
                    // Console.WriteLine($"Raw API Response: {responseText}"); 
                    AllPatientsList = await response.Content.ReadFromJsonAsync<List<Patient>>();
                    ApplyFilteredPatientsList();
               }
               catch (Exception ex)
               {
                    Console.WriteLine($"Error fetching patients: {ex.Message}");
                    Message = "An error occurred while fetching patients.";
               }
          }

     private int patientIdToDelete {get;set;} 
     private DotNetObjectReference<ManagePatients> objRef;
      protected override void OnInitialized()
    {
        objRef = DotNetObjectReference.Create(this);
    }
    private async Task DeletePatient(int patientId)
    {
        patientIdToDelete = patientId;
        await JSRuntime.InvokeVoidAsync("ShowDeleteConfirmation", objRef, "Patient");
    }
    [JSInvokable]
    public async Task PerformDelete()
    {
        try
        {
            var url = $"{ServerConstants.APICallNames.DeletePatient.GetStringValue()}{patientIdToDelete}";
            var response = await Http.DeleteAsync(url);
               if (response.IsSuccessStatusCode)
               {
                  FilteredPatientsList.Remove(FilteredPatientsList.FirstOrDefault(p => p.PatientID == patientIdToDelete));
                  await RefreshPatientList();
               }
               else
               {
                    var errorText = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {errorText}");
               }
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
     private async Task RefreshPatientList()
       {
          await LoadAllPatients();
          ApplyFilteredPatientsList();
          StateHasChanged(); 
       }

       public void ApplyFilteredPatientsList(){
            switch (FilterBasedOn)
            {
              case "Active":
                  FilteredPatientsList = AllPatientsList.Where(p => p.IsActive && p.IsAlive).ToList();
              break;
              case "Last7Days":
                  var sevenDaysAgo = DateTime.Now.AddDays(-7);
                  FilteredPatientsList = AllPatientsList.Where(p => p.CreatedAt >= sevenDaysAgo && p.IsActive && p.IsAlive).ToList();
              break;
              case "Last30Days":
                    var thirtyDaysAgo = DateTime.Now.AddDays(-30);
                    FilteredPatientsList = AllPatientsList.Where(p => p.CreatedAt >= thirtyDaysAgo && p.IsActive && p.IsAlive).ToList();
                    break;
              case "Last1Year":
                    var oneYearAgo = DateTime.Now.AddYears(-1);
                    FilteredPatientsList = AllPatientsList.Where(p => p.CreatedAt >= oneYearAgo && p.IsActive && p.IsAlive).ToList();
              break;
              case "All":
                  FilteredPatientsList = AllPatientsList.Where(p=>p.IsActive).ToList();
              break;
              default:
                  FilteredPatientsList = AllPatientsList.Where(p => p.IsActive && p.IsAlive).ToList();
              break;
            }
           StateHasChanged();
     }













     private bool ShowHistoryPopup = false;
    private List<PatientHistory> PatientHistoryList = new();

    private async Task ShowPatientHistory(Patient patient)
    {
        IsLoading = true;
        SelectedPatient = patient;
        PatientHistoryList = await GetPatientHistory(patient.PatientID); 
        ShowHistoryPopup = true;
        await Task.Delay(100);
        IsLoading = false;
    }

    private void CloseHistoryPopup()
    {
        ShowHistoryPopup = false;
    }

    private async Task<List<PatientHistory>> GetPatientHistory(int patientId)
    {
          try
          {
              // var url = $"{ServerConstants.GetPatientHistoryById}{patientId}";
              // string endpoint = ServerConstants.APICallNames.GetPatientHistoryById.GetStringValue();
              string url = $"{ServerConstants.APICallNames.GetPatientHistoryById.GetStringValue()}{patientId}";

              var response = await Http.GetFromJsonAsync<List<PatientHistory>>(url);
              return response ?? new List<PatientHistory>();
          }
          catch (Exception ex)
          {
              Console.WriteLine($"Error fetching patient history: {ex.Message}");
              return new List<PatientHistory>();
          }
    }



    private async Task ExportPatientsToExcel()
{
    var exportData = FilteredPatientsList.Select(p => new
    {
        p.FirstName,
        p.LastName,
        p.Age,
        p.BloodTypeNeeded,
        p.PhoneNumber,
        p.Country,
        p.State,
        p.District,
        p.PinCode,
        FullAddress = p.Address
    }).ToList();

    await JSRuntime.InvokeVoidAsync("exportToExcel", exportData, "RegisteredPatients.xlsx");
}



    }
}
