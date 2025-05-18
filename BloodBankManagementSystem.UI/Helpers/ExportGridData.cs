using ClosedXML.Excel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace BloodBankManagementSystem.UI.Helpers
{
    public static class ExportGridData
    {
          public static byte[] ExportToExcelBytes<T>(IEnumerable<T> data, string sheetName = "Data")
          {
               using var workbook = new XLWorkbook();
               var worksheet = workbook.Worksheets.Add(sheetName);
               var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
               // Write headers
               for (int i = 0; i < properties.Length; i++)
               {
                    worksheet.Cell(1, i + 1).Value = properties[i].Name;
                    worksheet.Cell(1, i + 1).Style.Font.Bold = true;
                    worksheet.Cell(1, i + 1).Style.Fill.BackgroundColor = XLColor.LightGray;
               }
               // Write data
               int row = 2;
               foreach (var item in data)
               {
                    for (int col = 0; col < properties.Length; col++)
                    {
                         var value = properties[col].GetValue(item);
                         worksheet.Cell(row, col + 1).Value = value?.ToString() ?? "";
                    }
                    row++;
               }
               worksheet.Columns().AdjustToContents();
               using var ms = new MemoryStream();
               workbook.SaveAs(ms);
               return ms.ToArray();
          }


        public static async Task ExportGridToCsv<T>(IEnumerable<T> data, IJSRuntime jsRuntime, string fileName = "export.csv")
        {
            if (data == null || !data.Any())
                return;

            var csvBuilder = new StringBuilder();

            var properties = typeof(T).GetProperties();

            // Write CSV header
            csvBuilder.AppendLine(string.Join(",", properties.Select(p => p.Name)));

            // Write CSV rows
            foreach (var item in data)
            {
                var values = properties.Select(p =>
                {
                    var value = p.GetValue(item, null);
                    if (value == null) return "";

                    var stringValue = value.ToString();

                    if (stringValue.Contains(",") || stringValue.Contains("\""))
                    {
                        stringValue = $"\"{stringValue.Replace("\"", "\"\"")}\"";
                    }
                    return stringValue;
                });

                csvBuilder.AppendLine(string.Join(",", values));
            }

            var csvBytes = Encoding.UTF8.GetBytes(csvBuilder.ToString());

            await jsRuntime.InvokeVoidAsync("BlazorDownloadFile", fileName, "text/csv", Convert.ToBase64String(csvBytes));
        }

    }
}
