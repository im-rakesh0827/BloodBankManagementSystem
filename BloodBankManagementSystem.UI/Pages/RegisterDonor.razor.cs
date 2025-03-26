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
          private Donor NewDonor { get; set; } = new();
          private string Message { get; set; } = string.Empty;
          private string PageTitle { get; set; } = "Register as a Blood Donor";
          private string ButtonName{get;set;} = "Save";
          private string ClearResetButton{get;set;} = "Clear";
          private List<string> BloodGroupsList = new List<string>();
          private List<string> GenderList = new List<string>();
         protected override void OnInitialized()
        {
          BloodGroupsList = BloodBankHelper.GetAllBloodGroups();
          GenderList = BloodBankHelper.GetAllGenders();
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
                    NewDonor = new Donor();
               }
        }

     private void AssignDonorDataToBeUpdated(){

          NewDonor = new Donor{
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
               LastDonationDate = SelectedDonorData.LastDonationDate
          };
     }


     private async Task HandleSubmit()
        {
          IsLoading = true;
          if(SelectedDonorData!=null && SelectedDonorData.Id>0){
               await Task.Delay(1500);
               await UpdateDonor();
          }
          else{

               await CreateDonor();
          }
          await OnDonorAddedOrUpdated.InvokeAsync();
          IsLoading = false;
        }
        private async Task CreateDonor(){
          //   if (!CheckEligibility())
          //   {
          //       Message = "Donor is not eligible due to weight or recent donation.";
          //       return;
          //   }
            var response = await Http.PostAsJsonAsync("api/donors/register", NewDonor);
            if (response.IsSuccessStatusCode)
            {
               Message = "Donor registered successfully!";
               NewDonor = new Donor();
               // await OnDonorAddedOrUpdated.InvokeAsync();
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
               var response = await Http.PutAsJsonAsync($"/api/donors/update/{NewDonor.Id}", NewDonor);
               if (response.IsSuccessStatusCode)
               {
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
               NewDonor = new Donor();
               Message = string.Empty;
            }
        }

        private async Task HandleCancelOrClose()
    {
        OnClose.InvokeAsync();
    }

     //    private void HandleCancelButton()
     //    {
     //        Navigation.NavigateTo("/");
     //    }

        private bool CheckEligibility()
        {
            if (NewDonor.Weight < 50)
            {
                return false; // Not eligible due to low weight
            }

            if (NewDonor.LastDonationDate.HasValue)
            {
                var daysSinceLastDonation = (DateTime.Now - NewDonor.LastDonationDate.Value).TotalDays;
                if (daysSinceLastDonation < 90) // Must wait at least 3 months (90 days)
                {
                    return false;
                }
            }
            return true;
        }
     }
}