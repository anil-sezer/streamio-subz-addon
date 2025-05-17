using Scalar.AspNetCore;
using Subz.Components;
using Subz.Consts;
using Subz.Infrastructure.ConstantsAndEnums;
using Subz.Infrastructure.Extensions;
using Subz.Middlewares;
using Subz.Models;
using Subz.Services;

var builder = WebApplication.CreateBuilder(args);

builder.InitLogsWithSerilog();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddMemoryCache();
builder.Services.AddHttpClient();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowStremio", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddSingleton<CacheService>();
builder.Services.AddSingleton<UserProfileAccessor>();

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
    context.Response.Redirect(PageNameConsts.Index);
    return Task.CompletedTask;
});

app.MapOpenApi();
app.MapScalarApiReference();

app.UseHttpsRedirection();

app.UseCors("AllowStremio");

bool IsApiRequest(HttpContext context)
{
    return context.Request.Path.StartsWithSegments($"/{RouteConsts.Api}");
}
app.UseWhen(context => IsApiRequest(context), appBuilder => 
{
    appBuilder.UseMiddleware<ConfigCacheMiddleware>();
    appBuilder.UseMiddleware<RequestLoggingMiddleware>();
});

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();

app.Run();