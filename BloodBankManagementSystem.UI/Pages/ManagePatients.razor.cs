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
    public partial class ManagePatients
    {
        private List<Patient> AllPatientsList { get; set; } = new();
        private List<Patient> FilteredPatientsList { get; set; } = new List<Patient>();
        private string Message { get; set; } = string.Empty;
        private Patient SelectedPatient { get; set; } = new();
        private bool IsCreateUpdatePopup{get;set;} = false;
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
                    var response = await Http.GetAsync("api/bloodbank/allPatients");
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
            var response = await Http.DeleteAsync($"api/bloodbank/{patientIdToDelete}");
               if (response.IsSuccessStatusCode)
               {
                    FilteredPatientsList.Remove(FilteredPatientsList.FirstOrDefault(p => p.PatientID == patientIdToDelete));
                    StateHasChanged();
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
          StateHasChanged(); // Force UI update
       }

       public void ApplyFilteredPatientsList(){
          FilteredPatientsList = AllPatientsList.Where(p => p.IsActive).ToList();
     }
    }
}
