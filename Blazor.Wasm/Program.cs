using Blazor.Wasm;
using Blazor.Wasm.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
//using System.Text;

//Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
//var test = Encoding.GetEncoding("Windows-1252");
//var test2 = Encoding.GetEncoding(1252);

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<FontServices>();

await builder.Build().RunAsync();
