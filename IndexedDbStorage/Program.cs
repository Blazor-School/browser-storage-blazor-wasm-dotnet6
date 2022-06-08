using IndexedDbStorage;
using IndexedDbStorage.Utils;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IndexedDbAccessor>();

var host = builder.Build();
using var scope = host.Services.CreateScope();
await using var indexedDB = scope.ServiceProvider.GetService<IndexedDbAccessor>();

if (indexedDB is not null)
{
    await indexedDB.InitializeAsync();
}

await host.RunAsync();