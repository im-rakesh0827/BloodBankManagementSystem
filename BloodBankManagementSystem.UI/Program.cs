using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BloodBankManagementSystem.UI;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using BloodBankManagementSystem.UI.Services;
using MudBlazor.Services;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using System;
using System.Net.Http;
using BloodBankManagementSystem.Infrastructure.Services;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Blazorise setup
builder.Services
    .AddBlazorise(options => { options.Immediate = true; })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

// HTTP Client base address - update to your API base URL
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5045/") });



builder.Services.AddScoped<PincodeAddressService>();
// Your AuthService
builder.Services.AddScoped<AuthService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<JwtAuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<JwtAuthStateProvider>());



// MudBlazor services
builder.Services.AddMudServices();

await builder.Build().RunAsync();
