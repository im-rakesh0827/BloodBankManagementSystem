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
using Microsoft.AspNetCore.Components;
using System.Reflection;
namespace BloodBankManagementSystem.UI.Shared{
     public partial class GenericTable<TModel> : ComponentBase
{
    [Parameter] public List<TModel> Data { get; set; } = new();
    [Parameter] public List<string> Columns { get; set; } = new();
    [Parameter] public EventCallback<TModel> OnEdit { get; set; }
    [Parameter] public EventCallback<TModel> OnDelete { get; set; }

    private object GetPropertyValue(TModel item, string propertyName)
    {
        Console.WriteLine($"🔍 Getting value for property '{propertyName}' from item: {item}");

        if (item == null)
        {
            Console.WriteLine("❌ Item is null");
            return string.Empty;
        }

        var property = typeof(TModel).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

        if (property == null)
        {
            Console.WriteLine($"❌ Property '{propertyName}' not found in {typeof(TModel).Name}");
            return string.Empty;
        }

        var value = property.GetValue(item);
        Console.WriteLine($"✅ Found property '{propertyName}', value: {value}");
        return value ?? string.Empty;
    }
    
}
}