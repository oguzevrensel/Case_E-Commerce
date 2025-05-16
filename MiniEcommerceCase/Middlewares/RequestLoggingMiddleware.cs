using Serilog;
using System.Diagnostics;

namespace MiniEcommerceCase.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private const string CorrelationIdHeader = "X-Correlation-ID";

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            // Correlation ID 
            if (!context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                context.Request.Headers[CorrelationIdHeader] = correlationId;
            }

            context.Response.Headers[CorrelationIdHeader] = correlationId;

            try
            {
                await _next(context);
                stopwatch.Stop();

                Log.Information("HTTP {Method} {Path} responded {StatusCode} in {Elapsed:0.0000} ms [CorrelationId: {CorrelationId}]",
                    context.Request.Method,
                    context.Request.Path,
                    context.Response.StatusCode,
                    stopwatch.Elapsed.TotalMilliseconds,
                    correlationId);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                Log.Error(ex, "HTTP {Method} {Path} failed in {Elapsed:0.0000} ms [CorrelationId: {CorrelationId}]",
                    context.Request.Method,
                    context.Request.Path,
                    stopwatch.Elapsed.TotalMilliseconds,
                    correlationId);

                throw;
            }
        }
    }
}
