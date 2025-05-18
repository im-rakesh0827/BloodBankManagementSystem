using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BloodBankManagementSystem.Core.Models;
using BloodBankManagementSystem.UI.Pages;
using BloodBankManagementSystem.UI.Helpers;
using BloodBankManagementSystem.Core.Enums;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Linq;
using Microsoft.JSInterop;
namespace BloodBankManagementSystem.UI.Pages.Users
{
    public partial class ManageUsers
    {
        private List<User> AllUsersList { get; set; } = new();
        private List<User> FilteredUsersList { get; set; } = new();
        private string Message {get; set;} = string.Empty;
        private bool IsCreateUpdatePopup {get; set;} = false;
        private User SelectedUser;
        private bool IsShowActiveOnly{get; set;} = true;
        private bool ShowHistoryPopup = false;
        private Dictionary<string, string> FilterOptions = new Dictionary<string, string>();
        private string FilterBasedOn {get; set;} = "Active";

        protected override async Task OnInitializedAsync()
        {
            FilterOptions = FilterOptionsHelper.AllFilterOption;
            await FetchAllUsersList();
        }
    
        private async Task FetchAllUsersList()
        {
            try
            {
                var url = $"{ServerConstants.APICallNames.GetAllUsers.GetStringValue()}";
                var response = await Http.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    var errorText = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"API Error: {response.StatusCode}, Response: {errorText}");
                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        Message = "No users found.";
                    }
                    else
                    {
                        Message = $"Error fetching users: {errorText}";
                    }
                    return;
                }
                //var responseText = await response.Content.ReadAsStringAsync();
                //Console.WriteLine($"Raw API Response: {responseText}"); // Logs raw response for debugging
                AllUsersList = await response.Content.ReadFromJsonAsync<List<User>>();
                ApplyFilterUser();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching users: {ex.Message}");
                Message = "An error occurred while fetching users.";
            }
        }


        private void OpenCreatePopUp()
        {
            SelectedUser = new User();
            IsCreateUpdatePopup = true;
        }

        private void CloseAddUserModal()
        {
            IsCreateUpdatePopup = false;
            SelectedUser = null;
        }

        private async Task RefreshUserList()
        {
            await FetchAllUsersList();
            ApplyFilterUser();
            StateHasChanged();
        }


        private void OpenEditOrUpdatePopUp(User user)
        {
            SelectedUser = user;
            IsCreateUpdatePopup = true;
        }

        private int userIdToDelete {get;set;} 
        private DotNetObjectReference<ManageUsers> objRef;
        protected override void OnInitialized()
        {
            objRef = DotNetObjectReference.Create(this);
        }
        private async Task DeleteUser(int userId)
        {
            userIdToDelete = userId;
            await JSRuntime.InvokeVoidAsync("ShowDeleteConfirmation", objRef, "User");
        }
        [JSInvokable]
        public async Task PerformDelete()
        {
            try
            {
                var url = $"{ServerConstants.APICallNames.DeleteUser.GetStringValue()}{userIdToDelete}";
                var response = await Http.DeleteAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    AllUsersList.Remove(AllUsersList.FirstOrDefault(p => p.Id == userIdToDelete));
                    await RefreshUserList();
                }
                else
                {
                    var errorText = await response.Content.ReadAsStringAsync();
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

        public void ApplyFilterUser(){
            if(IsShowActiveOnly){
                FilteredUsersList = AllUsersList.Where(p => p.IsActive && p.IsAlive).ToList();
            }
            else{
                FilteredUsersList = AllUsersList.Where(p => p.IsActive).ToList();
            }
        }

        private async Task ShowUsertHistory(User user)
        {
            SelectedUser = user;
            ShowHistoryPopup = true;
        }

        private void CloseHistoryPopup()
        {
            ShowHistoryPopup = false;
        }

        private async Task ExportCSV(){

               await ExportGridData.ExportGridToCsv(FilteredUsersList, JSRuntime, "UsersList.csv");
          }

          private async Task ExportExcel()
          {
               var fileBytes = ExportGridData.ExportToExcelBytes(FilteredUsersList);
               var base64 = Convert.ToBase64String(fileBytes);
               await JSRuntime.InvokeVoidAsync("BlazorDownloadFile", "UsersList.xlsx", 
               "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", base64);
          }

          private async Task HandleExportData(string exportType){
               switch(exportType.ToUpper()){
                    case "EXCEL":
                         ExportExcel();
                    break;
                    case "CSV":
                         ExportCSV();
                    break;
                    default:
                         ExportExcel();
                    break;
               }

          }

          public void HandleApplyFilter(string filterOption){
               FilterBasedOn = filterOption;
               ApplyFilterUser();
          }
    }
}