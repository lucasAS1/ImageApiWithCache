using System.Diagnostics.CodeAnalysis;
using System.Text;
using ImageApiWithCache.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;


namespace ImageApiWithCache.Infrastructure.Agents;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CachedAttribute : Attribute, IAsyncActionFilter
{
    private readonly int _timeInCache;

    public CachedAttribute(int timeInCache)
    {
        _timeInCache = timeInCache;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
        
        var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
        var cachedResponse = await cacheService.GetCachedResponseAsync(cacheKey);

        if (!string.IsNullOrEmpty(cachedResponse))
        {
            var contentResult = new ContentResult
            {
                Content = cachedResponse,
                ContentType = "application/json",
                StatusCode = 200
            };

            context.Result = contentResult;
            
            return;
        }

        var executedContext = await next();

        if (executedContext.Result is OkObjectResult okObjectResult)
        {
            await cacheService.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeInCache));
        }
    }

    private static string GenerateCacheKeyFromRequest(HttpRequest request)
    {
        var keyBuilder = new StringBuilder();

        keyBuilder.Append($"{request.Path}");

        return keyBuilder.ToString();
    }
}