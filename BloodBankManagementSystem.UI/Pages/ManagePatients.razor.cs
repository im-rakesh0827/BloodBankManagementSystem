using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using BloodBankManagementSystem.Core.Models;
using Microsoft.AspNetCore.Components;

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
                    // FilteredPatientsList = (await response.Content.ReadFromJsonAsync<List<Patient>>())?
                    // .Where(p => p.IsActive)
                    // .ToList();
               }
               catch (Exception ex)
               {
                    Console.WriteLine($"Error fetching patients: {ex.Message}");
                    Message = "An error occurred while fetching patients.";
               }
          }

         private async Task DeletePatient(int patientId)
        {
          //    bool confirmDelete = await JS.InvokeAsync<bool>("confirm", "Are you sure you want to delete this patient?");
          bool confirmDelete = true;
          if (confirmDelete)
          {
               var response = await Http.DeleteAsync($"api/bloodbank/{patientId}");
               if (response.IsSuccessStatusCode)
               {
                    FilteredPatientsList.Remove(FilteredPatientsList.FirstOrDefault(p => p.PatientID == patientId));
                    StateHasChanged();
               }
               else
               {
                    var errorText = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {errorText}");
               }
          }
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
