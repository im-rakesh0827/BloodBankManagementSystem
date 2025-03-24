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
        [Parameter] public Patient? PatientData { get; set; }
        [Parameter] public EventCallback OnClose { get; set; }
        [Parameter] public EventCallback OnPatientUpdated { get; set; }

        private void ClosePopup()
    {
        OnClose.InvokeAsync();
    }


    protected override void OnParametersSet()
    {
        if (PatientData != null)
        {
          ButtonName = "Update";
          ClearResetButton = "Reset";
          PageTitle = "Update Patient";
          AssignPatientDataToBeUpdated();
        }
    }

    private void AssignPatientDataToBeUpdated()
    {
          NewPatient = new Patient
            {
                PatientID = PatientData.PatientID,
                FirstName = PatientData.FirstName,
                LastName = PatientData.LastName,
                Age = PatientData.Age,
                BloodTypeNeeded = PatientData.BloodTypeNeeded,
                PhoneNumber = PatientData.PhoneNumber,
                Address = PatientData.Address,
                Country = PatientData.Country,
                State = PatientData.State,
                District = PatientData.District,
                PinCode = PatientData.PinCode
            };
    }

    private async Task HandleCancelButton()
    {
        ClosePopup();
    }

    private async Task HandleClearRestore()
    {
          if(PatientData!=null){
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