using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BloodBankManagementSystem.Core.Models;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using BloodBankManagementSystem.UI.Helpers;
using BloodBankManagementSystem.Core.Enums;
using System.Linq;
using Microsoft.JSInterop;

namespace BloodBankManagementSystem.UI.Shared
{
     public partial class CustomActionComponent{
          [Parameter] public EventCallback<string> OnFilterOptionChanged { get; set; }
          [Parameter] public EventCallback<string> OnExportData { get; set; }
          [Parameter] public EventCallback OnCreateRecord { get; set; }
          [Parameter] public string PageTitle{get; set;}
          public string ButtonName{get; set;} = "Create";

          
          private Dictionary<string, string> FilterOptions = new Dictionary<string, string>();
          private string FilterOption {get; set;} = "Active";
          protected override async Task OnInitializedAsync()
          {
               FilterOptions = FilterOptionsHelper.AllFilterOption;
          }
          public void ExportData(string exportType){
               OnExportData.InvokeAsync(exportType);
          }
          public void ApplyFilterData(){
               OnFilterOptionChanged.InvokeAsync(FilterOption);
          }

          public void CrateOrUpdate(){
               OnCreateRecord.InvokeAsync();
          }
     }
}