using Microsoft.AspNetCore.Mvc.Filters;

namespace ImageApiWithCache.Infrastructure.Interfaces.Cache;

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
        var cacheService = context.HttpContext.
        await next();
        
        
    }
}