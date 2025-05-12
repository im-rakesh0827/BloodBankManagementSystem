using System;
using System.Net.Http;
using BloodBankManagementSystem.Core.Models;
using Microsoft.AspNetCore.Components;
using BloodBankManagementSystem.Core.Enums;
using System.Collections.Generic;
using System.Linq;
using Microsoft.JSInterop;
using Blazorise.DataGrid;
namespace BloodBankManagementSystem.UI.Pages.Patients
{
     public partial class PatientHistoryComponent{
          [Parameter] public bool IsVisible { get; set; }
          [Parameter] public EventCallback OnClose { get; set; }
          [Parameter] public Patient? Patient { get; set; }
          [Parameter] public List<PatientHistory> HistoryList { get; set; } = new();
          private List<PatientHistory> filteredHistory = new();
          private List<PatientHistory> pagedHistory = new();

          private void Close()
          {
               OnClose.InvokeAsync();
          }
          private Task OnReadDataAsync(DataGridReadDataEventArgs<PatientHistory> e)
          {
               var query = HistoryList.AsQueryable();
               if (e.Columns != null)
               {
                    foreach (var col in e.Columns)
                    {
                         if (col.SearchValue is not string sv || string.IsNullOrWhiteSpace(sv))
                              continue;
                         var search = sv.Trim().ToLowerInvariant();
                         switch (col.Field)
                         {
                              case nameof(PatientHistory.ActionDate):
                              query = query.Where(x =>
                                   x.ActionDate.ToString("g", System.Globalization.CultureInfo.InvariantCulture)
                                                  .ToLowerInvariant()
                                                  .Contains(search));
                              break;
                              case nameof(PatientHistory.ActionType):
                              query = query.Where(x =>
                                   !string.IsNullOrEmpty(x.ActionType) &&
                                   x.ActionType.ToLowerInvariant().Contains(search));
                              break;
                              case nameof(PatientHistory.ActionUser):
                              query = query.Where(x =>
                                   !string.IsNullOrEmpty(x.ActionUser) &&
                                   x.ActionUser.ToLowerInvariant().Contains(search));
                              break;
                              case nameof(PatientHistory.ActionNote):
                              query = query.Where(x =>
                                   !string.IsNullOrEmpty(x.ActionNote) &&
                                   x.ActionNote.ToLowerInvariant().Contains(search));
                              break;
                         }
                    }
               }
               filteredHistory = query.ToList();
               pagedHistory = filteredHistory
                    .Skip((e.Page - 1) * e.PageSize)
                    .Take(e.PageSize)
                    .ToList();
               return Task.CompletedTask;
          }
     }
}