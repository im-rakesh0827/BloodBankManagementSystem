using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BloodBankManagementSystem.Core.Models;
using Microsoft.AspNetCore.Components;
using BloodBankManagementSystem.Core.Helpers;
using BloodBankManagementSystem.Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace BloodBankManagementSystem.UI.Pages
{
    public partial class RegisterPatient
    {
        public string ButtonName {get;set;} = "Save";
        public string ClearResetButton {get;set;} = "Clear";
        public string PageTitle {get;set;} = "Register Patient";
        private Patient NewPatient { get; set; } = new();
        private string Message { get; set; } = string.Empty;
        [Parameter] public Patient? SelectedPatientData { get; set; }
        [Parameter] public EventCallback OnClose { get; set; }
        [Parameter] public EventCallback OnPatientUpdated { get; set; }
        [Parameter] public bool IsCreateUpdatePatientPopup {get;set;}
        private bool IsLoading{get; set;} = false;

        private List<string> BloodGroupsList = new List<string>();
          private List<string> GenderList = new List<string>();
         protected override void OnInitialized()
        {
          BloodGroupsList = BloodBankHelper.GetAllBloodGroups();
          GenderList = BloodBankHelper.GetAllGenders();
        }


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
          IsLoading = true;
          if(NewPatient!=null && NewPatient.PatientID>0){
               await Task.Delay(1500);
               await UpdatePatient();
          }
          else{
               await CreatePatient();
          }
          await OnPatientUpdated.InvokeAsync();
          IsLoading = false;
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