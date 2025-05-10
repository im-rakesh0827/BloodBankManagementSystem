using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BloodBankManagementSystem.UI;
using Blazored.LocalStorage; // ✅ Ensure this is added
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BloodBankManagementSystem.UI.Services;
using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");



// builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5045/") });
// ✅ Register Blazored.LocalStorage
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<JwtAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<JwtAuthenticationStateProvider>());
builder.Services.AddScoped<AuthService>();
builder.Services.AddMudServices();


await builder.Build().RunAsync();

