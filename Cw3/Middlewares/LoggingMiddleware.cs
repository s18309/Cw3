using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cw3.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();

            StringBuilder sb = new StringBuilder();
            sb.Append(httpContext.Request.Method);
            sb.Append("\n");
            sb.Append(httpContext.Request.Path);
            sb.Append("\n");
            var bodyStream = string.Empty;
            using (var reader = new StreamReader( httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
            {
                bodyStream = await reader.ReadToEndAsync();
            }
            sb.Append(bodyStream);
            sb.Append("\n");
            sb.Append(httpContext.Request.Query);
            sb.Append("\n");

            File.AppendAllText("requestsLog.txt", sb.ToString());


            httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
            await _next(httpContext);
        }
    }
}
