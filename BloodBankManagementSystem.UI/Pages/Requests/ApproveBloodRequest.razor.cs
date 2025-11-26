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
     public partial class ApproveBloodRequest
     {
          private List<BloodRequest> AllRequestsList = new List<BloodRequest>();
          private List<BloodRequest> FilteredBlooodRequestsList = new List<BloodRequest>();
          private Notification NotificationModel = new Notification();
          private BloodRequest SelectedRequest = new BloodRequest();
          private List<int> ApproveListId{get;set;} = new List<int>();
          private List<int> RejectListId{get; set;} = new List<int>();
          private Dictionary<string, string> FilterOptions = new Dictionary<string, string>();

          private bool ShowPendingOnly = true;
          private bool IsCreateUpdatePopup = false;
          private bool IsAdmin = true;
          private bool IsLoading{get; set;} = false;
          private string searchString{get; set;}
          private bool IsReadOnly{get; set;} = false;
          
          private bool IsButtonEnabled{get; set;} = false;
          private string FilterBasedOn {get; set;} = "Active";

          private bool IsApproved = false;
          private bool IsRejected = false;
          private bool BloodRequestApprovalPermissionYN = true;
          private bool IsAnyRecordSelected => FilteredBlooodRequestsList?.Any(r => r.IsApprovedSelected || r.IsRejectedSelected) == true;
          private int TotalRequestCount{get; set;} = 0;





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
                    FilteredBlooodRequestsList = AllRequestsList;
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
                    FilteredBlooodRequestsList = AllRequestsList;
                    FilteredBlooodRequestsList = AllRequestsList.Where(r => r.Status == "Pending" && r.ActiveYN).ToList();
                    TotalRequestCount = AllRequestsList.Count;
               }
               else
               {
                    FilteredBlooodRequestsList = AllRequestsList;
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
                    RequesterType = request.RequesterType,
                    PatientName = request.PatientName,
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

               AllRequestsList = AllRequestsList.Where(p=>p.Status=="Pending").ToList();
               switch (FilterBasedOn)
            {
              case "Active":
                   FilteredBlooodRequestsList = AllRequestsList.Where(p => p.ActiveYN).ToList();
              break;
              case "Last7Days":
                  var sevenDaysAgo = DateTime.Now.AddDays(-7);
                  FilteredBlooodRequestsList = AllRequestsList.Where(p => p.RequestedDate >= sevenDaysAgo).ToList();
              break;
              case "Last30Days":
                    var thirtyDaysAgo = DateTime.Now.AddDays(-30);
                    FilteredBlooodRequestsList = AllRequestsList.Where(p => p.RequestedDate >= thirtyDaysAgo).ToList();
                    break;
              case "Last1Year":
                    var oneYearAgo = DateTime.Now.AddYears(-1);
                    FilteredBlooodRequestsList = AllRequestsList.Where(p => p.RequestedDate >= oneYearAgo).ToList();
              break;
              case "All":
                  FilteredBlooodRequestsList = AllRequestsList.ToList();
              break;
              default:
                  FilteredBlooodRequestsList = AllRequestsList.ToList();
              break;
            }
          StateHasChanged();
          }

          private async Task ExportCSV(){

               await ExportGridData.ExportGridToCsv(FilteredBlooodRequestsList, JSRuntime, "RequestsList.csv");
          }

          private async Task ExportExcel()
          {
               var fileBytes = ExportGridData.ExportToExcelBytes(FilteredBlooodRequestsList);
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


     private void OnApproveChanged(BloodRequest request)
     {
          if (request.IsApprovedSelected)
          {
               request.IsRejectedSelected = false;
          }
     }

private void OnRejectChanged(BloodRequest request)
{
    if (request.IsRejectedSelected)
    {
        request.IsApprovedSelected = false;
    }
}


private async Task SubmitApprovalDecisions()
{
    var selectedRequests = FilteredBlooodRequestsList
        .Where(r => r.IsApprovedSelected || r.IsRejectedSelected)
        .ToList();

    if (!selectedRequests.Any())
    {
        await JSRuntime.InvokeVoidAsync("alert", "Please select at least one request to approve or reject.");
        return;
    }

    var invalidRejections = selectedRequests
        .Where(r => r.IsRejectedSelected && string.IsNullOrWhiteSpace(r.Notes))
        .ToList();

    if (invalidRejections.Any())
    {
        await JSRuntime.InvokeVoidAsync("ShowToastAlert", "Rejection note is required for all rejected requests.", "Warning", "warning");

        return;
    }

    var updates = selectedRequests.Select(r => new BloodRequestStatusUpdateModel
    {
        Id = r.Id,
        NewStatus = r.IsApprovedSelected ? "Approved"
                   : r.IsRejectedSelected ? "Rejected"
                   : "Pending",
        Notes = r.Notes
    }).ToList();

    IsLoading = true;

    await Task.Delay(500);
    var response = await Http.PostAsJsonAsync("api/bloodrequest/updateStatus", updates);
    if (response.IsSuccessStatusCode)
    {
        NotificationModel.Message = "Blood request status updated successfully!";
        NotificationModel.Header = "Success";
        NotificationModel.Icon = "success";
    }
    else
    {
        NotificationModel.Message = "Something went wrong!";
        NotificationModel.Header = "Failed";
        NotificationModel.Icon = "error";
    }
    
    IsLoading = false;
    await RefreshRequestList();
    await JSRuntime.InvokeVoidAsync("ShowToastAlert", NotificationModel.Message, NotificationModel.Header, NotificationModel.Icon);
}
}

}