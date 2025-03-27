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
using Microsoft.JSInterop;

namespace BloodBankManagementSystem.UI.Pages
{
    public partial class RegisterPatient
    {
        public string ButtonName {get;set;} = "Save";
        public string ClearResetButton {get;set;} = "Clear";
        public string PageTitle {get;set;} = "Register Patient";
        private Patient PatientModel { get; set; } = new();
        private string Message { get; set; } = string.Empty;
        [Parameter] public Patient? SelectedPatientData { get; set; }
        [Parameter] public EventCallback OnClose { get; set; }
        [Parameter] public EventCallback OnPatientUpdated { get; set; }
        [Parameter] public bool IsCreateUpdatePatientPopup {get;set;}
        private bool IsLoading{get; set;} = false;
        private List<string> BloodGroupsList = new List<string>();
        private List<string> GenderList = new List<string>();
        private Notification NotificationModel = new Notification();
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
          PatientModel = new Patient();
        }
    }

    private void AssignPatientDataToBeUpdated()
    {
          PatientModel = new Patient
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
                PinCode = SelectedPatientData.PinCode,
                Email = SelectedPatientData.Email,
                Weight = SelectedPatientData.Weight,
                IsActive = SelectedPatientData.IsActive,
                IsAlive = SelectedPatientData.IsAlive
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
               PatientModel = new Patient();
          }
    }
     
     private async Task HandleSubmit()
     {
          IsLoading = true;
          if(PatientModel!=null && PatientModel.PatientID>0){
               await Task.Delay(1500);
               await UpdatePatient();
          }
          else{
               await CreatePatient();
          }
          await JSRuntime.InvokeVoidAsync("ShowToastAlert", NotificationModel.Message, NotificationModel.Header, NotificationModel.Icon);
          IsLoading = false;
     }

     private async  Task CreatePatient(){
     try
          {
               var response = await Http.PostAsJsonAsync("api/patients/register", PatientModel);
               if (response.IsSuccessStatusCode)
               {
                    NotificationModel.Message = "Patient registered successfully!";
                    NotificationModel.Header = "Success";
                    NotificationModel.Icon = "success";
               }
               else
               {
                    var errorText = await response.Content.ReadAsStringAsync();
                    NotificationModel.Message = $"Error: {errorText}";
                    NotificationModel.Header = "Information";
                    NotificationModel.Icon = "info";
               }
               PatientModel = new(); // Reset form
               await OnPatientUpdated.InvokeAsync();
               await OnClose.InvokeAsync();
          }
          catch (Exception ex)
          {
               NotificationModel.Message = $"An error occurred: {ex.Message}";
               NotificationModel.Header = "Error";
               NotificationModel.Icon = "error";
          }
     }

     private async Task UpdatePatient()
     {
          try{
               var response = await Http.PutAsJsonAsync($"api/patients/{PatientModel.PatientID}", PatientModel);
               if (response.IsSuccessStatusCode)
               {
                    NotificationModel.Message = "Patient information updated successfully!";
                    NotificationModel.Header = "Success";
                    NotificationModel.Icon = "success";
                    await OnPatientUpdated.InvokeAsync();
                    await OnClose.InvokeAsync(); // Close modal after saving
               }
               else{
                    var errorText = await response.Content.ReadAsStringAsync();
                    NotificationModel.Message = $"Error: {errorText}";
                    NotificationModel.Header = "Information";
                    NotificationModel.Icon = "info";
               }
          }
          catch(Exception ex){
               NotificationModel.Message = $"An error occurred: {ex.Message}";
               NotificationModel.Header = "Error";
               NotificationModel.Icon = "error";
               throw ex;
          }
     }

    }
}