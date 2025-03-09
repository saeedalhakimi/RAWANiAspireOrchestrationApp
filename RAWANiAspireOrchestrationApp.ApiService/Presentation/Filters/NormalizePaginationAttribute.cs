using Microsoft.AspNetCore.Mvc.Filters;

namespace RAWANiAspireOrchestrationApp.ApiService.Presentation.Filters
{
    public class NormalizePaginationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Get ILogger from the service provider
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<NormalizePaginationAttribute>>();

            // Check and normalize pageNumber
            if (context.ActionArguments.TryGetValue("pageNumber", out var pageNumberObj) && pageNumberObj is int pageNumber)
            {
                if (pageNumber <= 0)
                {
                    logger.LogDebug("PageNumber {PageNumber} is invalid, defaulting to 1", pageNumber);
                    context.ActionArguments["pageNumber"] = 1; // Override with default
                }
            }

            // Check and normalize pageSize
            if (context.ActionArguments.TryGetValue("pageSize", out var pageSizeObj) && pageSizeObj is int pageSize)
            {
                if (pageSize <= 0)
                {
                    logger.LogDebug("PageSize {PageSize} is invalid, defaulting to 10", pageSize);
                    context.ActionArguments["pageSize"] = 10; // Override with default
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
