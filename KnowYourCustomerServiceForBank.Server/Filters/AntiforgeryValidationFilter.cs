using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KnowYourCustomerServiceForBank.Server.Filters;

public class AntiforgeryValidationFilter(IAntiforgery antiforgery) : IAsyncActionFilter
{
    private readonly IAntiforgery _antiforgery = antiforgery;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var httpContext = context.HttpContext;
        var method = httpContext.Request.Method;

        // Skip safe HTTP methods
        if (!HttpMethods.IsGet(method) &&
            !HttpMethods.IsHead(method) &&
            !HttpMethods.IsOptions(method) &&
            !HttpMethods.IsTrace(method))
        {
            try
            {
                await _antiforgery.ValidateRequestAsync(httpContext);

            }
            catch (AntiforgeryValidationException)
            {
                context.Result = new BadRequestObjectResult("Invalid or missing anti-forgery token.");
                return;
            }
        }

        await next();
    }
}