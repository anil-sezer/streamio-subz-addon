using System.Text;
using Microsoft.Extensions.Primitives;
using Serilog;

namespace Subz.Middlewares;

// TODO: SHOULD ONLY LOG REQUESTS THAT COMES TO CONTROLLERS 
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        LogRequest(context);
            
        await _next(context);
    }

    private void LogRequest(HttpContext context)
    {
        var request = context.Request;
            
        var requestInfo = new StringBuilder();
        requestInfo.AppendLine("=== Request Details ===");
        requestInfo.AppendLine($"Method: {request.Method}");
        requestInfo.AppendLine($"Path: {request.Path}");
        requestInfo.AppendLine($"QueryString: {request.QueryString}");
        requestInfo.AppendLine($"Protocol: {request.Protocol}");
        requestInfo.AppendLine($"Client IP: {context.Connection.RemoteIpAddress}");
        requestInfo.AppendLine($"User Agent: {request.Headers["User-Agent"]}");
            
        requestInfo.AppendLine("Headers:");
        foreach (KeyValuePair<string, StringValues> header in request.Headers)
        {
            requestInfo.AppendLine($"  {header.Key}: {header.Value}");
        }

        // // Log cookies if any
        // if (request.Cookies.Any())
        // {
        //     requestInfo.AppendLine("Cookies:");
        //     foreach (var cookie in request.Cookies)
        //     {
        //         requestInfo.AppendLine($"  {cookie.Key}: {cookie.Value}");
        //     }
        // }

        Log.Information(requestInfo.ToString());
    }
}