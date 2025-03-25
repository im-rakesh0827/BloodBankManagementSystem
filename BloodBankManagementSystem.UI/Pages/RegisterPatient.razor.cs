using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BloodBankManagementSystem.Core.Models;
using Microsoft.AspNetCore.Components;

namespace BloodBankManagementSystem.UI.Pages
{
    public partial class RegisterPatient
    {
        private List<string> BloodTypes = new()
        {
          "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-"
        };
        public string ButtonName {get;set;} = "Register";
        public string ClearResetButton {get;set;} = "Clear";
        public string PageTitle {get;set;} = "Register Patient";
        private Patient NewPatient { get; set; } = new();
        private string Message { get; set; } = string.Empty;
        [Parameter] public Patient? SelectedPatientData { get; set; }
        [Parameter] public EventCallback OnClose { get; set; }
        [Parameter] public EventCallback OnPatientUpdated { get; set; }
        [Parameter] public bool IsCreateUpdatePatientPopup {get;set;}

    protected override void OnParametersSet()
    {
        if (SelectedPatientData != null && SelectedPatientData.PatientID>0)
        {
          ButtonName = "Update";
          ClearResetButton = "Reset";
          PageTitle = "Update Patient";
          AssignPatientDataToBeUpdated();
        }
        else{
          NewPatient = new Patient();
        }
    }

    private void AssignPatientDataToBeUpdated()
    {
          NewPatient = new Patient
            {
                PatientID = SelectedPatientData.PatientID,
                FirstName = SelectedPatientData.FirstName,
                LastName = SelectedPatientData.LastName,
                Age = SelectedPatientData.Age,
                BloodTypeNeeded = SelectedPatientData.BloodTypeNeeded,
                PhoneNumber = SelectedPatientData.PhoneNumber,
                Address = SelectedPatientData.Address,
                Country = SelectedPatientData.Country,
                State = SelectedPatientData.State,
                District = SelectedPatientData.District,
                PinCode = SelectedPatientData.PinCode
            };
    }

    private async Task HandleCancelOrClose()
    {
        OnClose.InvokeAsync();
    }

    private async Task HandleClearRestore()
    {
          if(SelectedPatientData!=null){
               AssignPatientDataToBeUpdated();
          }
          else{
               NewPatient = new Patient();
          }
    }
     
     private async Task HandleSubmit()
     {
          if(NewPatient.PatientID==0){
               await CreatePatient();
          }
          else{
               await UpdatePatient();
          }
          await OnClose.InvokeAsync();
          await OnPatientUpdated.InvokeAsync(); // Notify parent
     }

     private async  Task CreatePatient(){
     try
          {
               var response = await Http.PostAsJsonAsync("api/bloodbank/register", NewPatient);

               if (response.IsSuccessStatusCode)
               {
               Message = "Patient registered successfully!";
               NewPatient = new(); // Reset form
               }
               else
               {
               var errorText = await response.Content.ReadAsStringAsync();
               Message = $"Error: {errorText}";
               }
          }
          catch (Exception ex)
          {
               Message = $"An error occurred: {ex.Message}";
          }
     }

     private async Task UpdatePatient()
     {
          try{
               var response = await Http.PutAsJsonAsync($"api/bloodbank/{NewPatient.PatientID}", NewPatient);
          if (response.IsSuccessStatusCode)
          {
               await OnClose.InvokeAsync(); // Close modal after saving
          }
          }
          catch(Exception){
               throw;
          }
     }

    }
}