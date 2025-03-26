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

namespace BloodBankManagementSystem.UI.Pages{

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
         protected override void OnInitialized()
        {
          BloodGroupsList = BloodBankHelper.GetAllBloodGroups();
          GenderList = BloodBankHelper.GetAllGenders();
          HealthIssues = BloodBankHelper.GetHealthIssue();
        }

        protected override void OnParametersSet()
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

     private void AssignDonorDataToBeUpdated(){

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


     private async Task HandleSubmit()
        {
          IsLoading = true;
          DonorModel.IsEligible = CheckEligibility();
          if(SelectedDonorData!=null && SelectedDonorData.Id>0){
               await Task.Delay(1500);
               await UpdateDonor();
          }
          else{
               await CreateDonor();
          }
          IsLoading = false;
        }
        private async Task CreateDonor(){
            var response = await Http.PostAsJsonAsync("api/donors/register", DonorModel);
            if (response.IsSuccessStatusCode)
            {
               Message = "Donor registered successfully!";
               DonorModel = new Donor();
               await OnDonorAddedOrUpdated.InvokeAsync();
               await OnClose.InvokeAsync();
            }
            else
            {
               Message = "Error registering donor. Please try again.";
            }
        }

        private async Task UpdateDonor(){
          if (SelectedDonorData != null)
          {
               var response = await Http.PutAsJsonAsync($"/api/donors/update/{DonorModel.Id}", DonorModel);
               if (response.IsSuccessStatusCode)
               {
                    Message = "Donor updated successfully!";
                    await OnDonorAddedOrUpdated.InvokeAsync();
                    await OnClose.InvokeAsync();
               }
               else
               {
                    Console.WriteLine("Error updating user");
               }
          }

        }

        private void HandleClearRestore()
        {
            if(IsCreateUpdateDonorPopup){
               AssignDonorDataToBeUpdated();
            }
            else{
               DonorModel = new Donor();
            }
            Message = string.Empty;
        }

        private async Task HandleCancelOrClose()
    {
        OnClose.InvokeAsync();
    }

     //    private void HandleCancelButton()
     //    {
     //        Navigation.NavigateTo("/");
     //    }
        public bool CheckEligibility()
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
     }
}