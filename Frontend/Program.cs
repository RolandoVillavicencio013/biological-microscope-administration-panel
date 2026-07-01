using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using UCR.EB.BioMicroscopeAdmin.Frontend;
using UCR.EB.BioMicroscopeAdmin.Frontend.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();
builder.Services.AddFrontendServices(
    builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5000/");

await builder.Build().RunAsync();
