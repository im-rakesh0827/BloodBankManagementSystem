using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BloodBankManagementSystem.Core.Models;
using Microsoft.AspNetCore.Components;
using BloodBankManagementSystem.Core.Helpers;
using BloodBankManagementSystem.UI.Helpers;
using BloodBankManagementSystem.Core.Enums;
using System.Collections.Generic;
using System.Linq;
using Microsoft.JSInterop;
namespace BloodBankManagementSystem.UI.Pages.Requests
{
     public partial class ManageBloodRequest
     {
          private List<BloodRequest> AllRequestsList = new();
          private List<BloodRequest> FilteredRequestsList = new();
          private BloodRequest SelectedRequest = new();
          private bool ShowPendingOnly = true;
          private bool IsCreateUpdatePopup = false;
          private bool IsAdmin = true;
          private bool IsLoading{get; set;} = false;
          private Notification NotificationModel = new Notification();
          private string searchString{get; set;}
          private bool IsReadOnly{get; set;} = false;
          private List<int> ApproveListId{get;set;} = new List<int>();
          private List<int> RejectListId{get; set;} = new List<int>();
          private bool IsButtonEnabled{get; set;} = false;
          private Dictionary<string, string> FilterOptions = new Dictionary<string, string>();
          private string FilterBasedOn {get; set;} = "Active";


          protected override async Task OnInitializedAsync()
          {
               FilterOptions = FilterOptionsHelper.AllFilterOption;
               await LoadRequestsAsync();
          }

          private async Task RefreshBloodRequestList()
          {
               IsCreateUpdatePopup = false;
               await LoadRequestsAsync();
               StateHasChanged(); 
          }

          private async Task LoadRequestsAsync()
          {
               try
               {
                    var url = $"{ServerConstants.APICallNames.GetAllBloodRequests.GetStringValue()}";
                    AllRequestsList = await Http.GetFromJsonAsync<List<BloodRequest>>(url);
                    FilteredRequestsList = AllRequestsList;
                    ApplyFilter();
               }
               catch (Exception ex)
               {
                    Console.WriteLine($"Error fetching blood requests: {ex.Message}");
               }
          }


          private void ApplyFilter()
          {
               if (ShowPendingOnly)
               {
                    FilteredRequestsList = AllRequestsList;
                    //   FilteredRequestsList = AllRequestsList.Where(r => r.Status == "Pending").ToList();
               }
               else
               {
                    FilteredRequestsList = AllRequestsList;
               }
          }

          private void ViewRequestDetails(BloodRequest request)
          {

          }

          private void OpenCreatePopUp()
          {
               SelectedRequest = new BloodRequest();
               IsCreateUpdatePopup = true;
          }

          private void OpenEditOrUpdatePopUp(BloodRequest request)
          {
               SelectedRequest = new BloodRequest
               {
                    Id = request.Id,
                    //   RequesterId = request.RequesterId,
                    RequesterType = request.RequesterType,
                    PatientName = request.PatientName,
                    PatientId = request.PatientId,
                    DonorId = request.DonorId,
                    Gender = request.Gender,
                    BloodGroup = request.BloodGroup,
                    UnitsRequired = request.UnitsRequired,
                    Location = request.Location,
                    ContactNumber = request.ContactNumber,
                    Email = request.Email,
                    FullAddress = request.FullAddress,
                    Description = request.Description,
                    RequestedDate = request.RequestedDate,
                    ApprovedDate = request.ApprovedDate,
                    CreatedBy = request.CreatedBy,
                    UpdatedBy = request.UpdatedBy,
                    UpdatedAt = request.UpdatedAt,
                    Status = request.Status
               };
               IsCreateUpdatePopup = true;
          }

          private void HandlePopupClose()
          {
               IsCreateUpdatePopup = false;
          }

          private async Task RefreshRequestList()
          {
               IsCreateUpdatePopup = false;
               await LoadRequestsAsync();
          }

          private async Task DeleteRequest(int id)
          {
               var confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete this request?");
               if (confirmed)
               {
                    var url = $"{ServerConstants.APICallNames.DeleteRequest.GetStringValue()}{id}";
                    await Http.DeleteAsync(url);
                    await LoadRequestsAsync();
               }
          }

          private async Task UpdateRequestStatus(int requestId, string statusValue)
          {
               try
               {
                    IsLoading = true;
                    bool IsApprove = statusValue=="Approve"?true:false;
                    await Task.Delay(500);
                    var content = new StringContent(""); 
                    var url = $"{ServerConstants.APICallNames.UpdateBloodRequestStatus.GetStringValue()}{requestId}?status={IsApprove}";
                    var response = await Http.PutAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                         string _status = IsApprove?"approved":"rejected";
                         NotificationModel.Message = $"Blood request {_status} successfully!";
                         NotificationModel.Header = "Success";
                         NotificationModel.Icon = "success";
                         await RefreshRequestList();
                         IsLoading = false;
                         await JSRuntime.InvokeVoidAsync("ShowToastAlert", NotificationModel.Message, NotificationModel.Header, NotificationModel.Icon);
                    }
                    else
                    {
                         await JSRuntime.InvokeVoidAsync("alert", "Failed to approve request.");
                    }
               }
               catch (System.Exception)
               {
                    throw;
               }
          }

          private void HandleCreateUpdatePopup()
          {
               IsCreateUpdatePopup = false;
          }

          private void SaveButton()
          {

          }

          private void OnRowSelect(BloodRequest request){

          }




          public void ApplyFiltereBloodRequest()
          {

          }

          private async Task ExportCSV(){

               await ExportGridData.ExportGridToCsv(FilteredRequestsList, JSRuntime, "RequestsList.csv");
          }

          private async Task ExportExcel()
          {
               var fileBytes = ExportGridData.ExportToExcelBytes(FilteredRequestsList);
               var base64 = Convert.ToBase64String(fileBytes);
               await JSRuntime.InvokeVoidAsync("BlazorDownloadFile", "RequestsList.xlsx", 
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
               ApplyFiltereBloodRequest();
          }
     }
}