using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddHttpClient("ApiService", client =>
{
    client.BaseAddress = new Uri("https://localhost:7478/"); // WebAPI 주소
});

await builder.Build().RunAsync();