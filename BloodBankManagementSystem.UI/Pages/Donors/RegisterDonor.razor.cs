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

namespace BloodBankManagementSystem.UI.Pages.Donors{

     public partial class RegisterDonor{
          [Inject]
          private NavigationManager Navigation { get; set; } = default!;
          [Parameter] 
          public  Donor? SelectedDonorData{get; set;}
          [Parameter] 
          public EventCallback OnClose { get; set; }
          [Parameter] 
          public EventCallback OnDonorAddedOrUpdated { get; set; }
          [Parameter] 
          public bool IsCreateUpdateDonorPopup{get; set;}
          private bool IsLoading{get; set;} = false;
          private Donor DonorModel { get; set; } = new();
          private string Message { get; set; } = string.Empty;
          private string PageTitle { get; set; } = "Register as a Blood Donor";
          private string ButtonName{get;set;} = "Save";
          private string ClearResetButton{get;set;} = "Clear";
          private List<string> BloodGroupsList = new List<string>();
          private List<string> GenderList = new List<string>();
          private List<string> HealthIssues { get; set; } = new();
          private Notification NotificationModel = new Notification();
         protected override void OnInitialized()
        {
          try
          {
               BloodGroupsList = BloodBankHelper.GetAllBloodGroups();
               GenderList = BloodBankHelper.GetAllGenders();
               HealthIssues = BloodBankHelper.GetHealthIssue();
          }
          catch (System.Exception)
          {
               
               throw;
          }
        }

        protected override void OnParametersSet()
        {
          try
          {
               if (SelectedDonorData != null && SelectedDonorData.Id>0)
               {
                    ButtonName = "Update";
                    ClearResetButton = "Reset";
                    PageTitle = "Update Donor";
                    AssignDonorDataToBeUpdated();
               }
               else{
                    DonorModel = new Donor();
               }
          }
          catch (System.Exception)
          {
               
               throw;
          }
        }

     private void AssignDonorDataToBeUpdated(){

          try
          {
               DonorModel = new Donor{
                    Id = SelectedDonorData.Id,
                    FirstName = SelectedDonorData.FirstName,
                    LastName = SelectedDonorData.LastName,
                    Email = SelectedDonorData.Email,
                    Phone = SelectedDonorData.Phone,
                    BloodGroup = SelectedDonorData.BloodGroup,
                    Gender = SelectedDonorData.Gender,
                    Age = SelectedDonorData.Age,
                    Weight = SelectedDonorData.Weight,
                    Country = SelectedDonorData.Country,
                    State = SelectedDonorData.State,
                    District = SelectedDonorData.District,
                    Address = SelectedDonorData.Address,
                    PinCode = SelectedDonorData.PinCode,
                    LastDonationDate = SelectedDonorData.LastDonationDate,
                    MedicalHistory = SelectedDonorData.MedicalHistory,
                    IsActive = SelectedDonorData.IsActive,
                    IsAlive = SelectedDonorData.IsAlive,
                    IsEligible = SelectedDonorData.IsEligible
               };
          }
          catch (System.Exception)
          {
               
               throw;
          }
     }


     private async Task HandleSubmit()
     {
          try
          {
               NotificationModel = new Notification();
               IsLoading = true;
               DonorModel.IsEligible = CheckEligibility();
               if(SelectedDonorData!=null && SelectedDonorData.Id>0){
                    await Task.Delay(500);
                    await UpdateDonor();
               }
               else{
                    await CreateDonor();
               }
               await JSRuntime.InvokeVoidAsync("ShowToastAlert", NotificationModel.Message, NotificationModel.Header, NotificationModel.Icon);
               IsLoading = false;
          }
          catch (System.Exception)
          {
               
               throw;
          }
          
     }
     private async Task CreateDonor()
     {
          try
          {
               var url = $"{ServerConstants.APICallNames.RegisterDonor.GetStringValue()}";
               var response = await Http.PostAsJsonAsync(url, DonorModel);
               if (response.IsSuccessStatusCode)
               {
                    NotificationModel.Message = "Donor registered successfully!";
                    NotificationModel.Header = "Success";
                    NotificationModel.Icon = "success";
                    DonorModel = new Donor();
                    await OnDonorAddedOrUpdated.InvokeAsync();
                    await OnClose.InvokeAsync();
               }
               else
               {
                    NotificationModel.Message = "Error occured while registering donor. Please try again.";
                    NotificationModel.Header = "Information";
                    NotificationModel.Icon = "info";
               }
          }
          catch (System.Exception)
          {
               
               throw;
          }
            
     }

        private async Task UpdateDonor(){
          try{
               if (SelectedDonorData != null)
          {
               var url = $"{ServerConstants.APICallNames.UpdateDonor.GetStringValue()}{DonorModel.Id}";
               var response = await Http.PutAsJsonAsync(url, DonorModel);
               if (response.IsSuccessStatusCode)
               {
                    NotificationModel.Message = "Donor information updated successfully!";
                    NotificationModel.Header = "Success";
                    NotificationModel.Icon = "success";
                    await OnDonorAddedOrUpdated.InvokeAsync();
                    await OnClose.InvokeAsync();
               }
               else
               {
                    NotificationModel.Message = "Error occured while updating the donor";
                    NotificationModel.Header = "Information";
                    NotificationModel.Icon = "info";
               }
          }
          }
          catch(Exception ex){
               NotificationModel.Message = ex.Message;
               NotificationModel.Header = "Error";
               NotificationModel.Icon = "error";
               throw;
          }
        }

        private void HandleClearRestore()
        {
            try
            {
               if(IsCreateUpdateDonorPopup){
                    AssignDonorDataToBeUpdated();
               }
               else{
                    DonorModel = new Donor();
               }
               Message = string.Empty;
            }
            catch (System.Exception)
            {
               
               throw;
            }
        }

     private async Task HandleCancelOrClose()
     {
        OnClose.InvokeAsync();
     }
        
     public bool CheckEligibility()
     {
          try
          {
               Donor donor = DonorModel!=null? DonorModel:new Donor();
               if(donor!=null){
                    if (!donor.IsAlive || donor.Age < 18 || donor.Age > 65 || donor.Weight < 50 || donor.MedicalHistory.ToString()!="None") return false;
                    if (donor.LastDonationDate.HasValue && (DateTime.Now - donor.LastDonationDate.Value).TotalDays / 30 < 3) return false;
                    return true;
               }
               else{
                    return false;
               }
          }
          catch (System.Exception)
          {
               
               throw;
          }
     }
     }
}