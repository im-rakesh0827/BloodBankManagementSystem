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
using BloodBankManagementSystem.UI.Helpers;
using BloodBankManagementSystem.UI.Services;
namespace BloodBankManagementSystem.UI.Pages.Patients
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
        [Parameter] public bool IsCreateNewPatient{get;set;} = true;
        private bool IsLoading{get; set;} = false;
        private List<string> BloodGroupsList = new List<string>();
        private List<string> GenderList = new List<string>();
        private Notification NotificationModel = new Notification();
        [Inject] private PincodeAddressService PincodeService { get; set; }
        
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
               await Task.Delay(500);
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
               var url = $"{ServerConstants.APICallNames.RegisterPatient.GetStringValue()}";
               var response = await Http.PostAsJsonAsync(url, PatientModel);
               if (response.IsSuccessStatusCode)
               {
                    NotificationModel.Message = "Patient registered successfully!";
                    NotificationModel.Header = "Success";
                    NotificationModel.Icon = "success";

                    PatientModel = new(); 
                    await OnPatientUpdated.InvokeAsync();
                    await OnClose.InvokeAsync();
               }
               else
               {
                    var errorText = await response.Content.ReadAsStringAsync();
                    NotificationModel.Message = $"Error: {errorText}";
                    NotificationModel.Header = "Information";
                    NotificationModel.Icon = "info";
               }
               
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
               var url = $"{ServerConstants.APICallNames.UpdatePatient.GetStringValue()}{PatientModel.PatientID}";
               var response = await Http.PutAsJsonAsync(url, PatientModel);
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



private async Task LoadAddressFromPinCode()
{
    var postOffice = await PincodeService.GetAddressFromPinCodeAsync(PatientModel.PinCode);

    if (postOffice is not null)
    {
        PatientModel.District = postOffice.District;
        PatientModel.State = postOffice.State;
        PatientModel.Country = postOffice.Country;
        PatientModel.Address = $"{postOffice.Country}, {postOffice.State}, {postOffice.District}";
        StateHasChanged();
    }
    else
    {
     //    await JSRuntime.InvokeVoidAsync("alert", "Invalid pincode or no post office found.");
          NotificationModel.Message = $"An error occurred";
          NotificationModel.Header = "Error";
          NotificationModel.Icon = "error";
          // await JSRuntime.InvokeVoidAsync("ShowToastAlert", NotificationModel.Message, NotificationModel.Header, NotificationModel.Icon);

    }
}

    }
}