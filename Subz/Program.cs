using Subz.Components;
using Subz.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.InitLogsWithSerilog();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapGet("/", context =>
{
    context.Response.Redirect("/configuration");
    return Task.CompletedTask;
});

app.UseHttpsRedirection();

app.UseCors("AllowStremio");


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();