using WebAPITodoList.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;




namespace WebAPITodoList.Middlewares;
/// <summary>
/// Limita il numero di richieste che un client(IP/token) può fare in un certo periodo di tempo.
/// </summary>
public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;
    private readonly RateLimitSettings _settings;

    // Iniezione del Middlewaare
    public RateLimitingMiddleware(RequestDelegate next,
        IMemoryCache cache,
        IOptions<RateLimitSettings> settings)
    {
        _next = next;
        _cache = cache;
        _settings = settings.Value;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        string clientIpAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var cacheKey = $"RateLimit_{clientIpAddress}";

        if (!_cache.TryGetValue(cacheKey, out RequestCounter entry))
        {
            entry = new RequestCounter
            {
                Count = 1,
                WindowStart = DateTime.UtcNow
            };

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_settings.PERIOD)
            };

            _cache.Set(cacheKey, entry, cacheEntryOptions);
        }
        else
        {
            entry.Count++;

            //si aggiorna la cahce di memoria per assicurarsi che il valore sia aggiornato
            _cache.Set(cacheKey, entry, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(_settings.PERIOD)
            });
        }

        if (entry.Count > _settings.LIMIT)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            await context.Response.WriteAsync("Rate limit exceeded");
            return;
        }

        await _next(context);
    }

    private class RequestCounter
    {
        public int Count { get; set; }
        public DateTime WindowStart { get; set; }
    }
}
