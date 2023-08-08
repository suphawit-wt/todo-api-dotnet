using Microsoft.AspNetCore.Mvc.Filters;
using TodoApp.Core.Exceptions;

namespace TodoApp.Api.Filters
{
    public class ModelStateFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.ToDictionary(
                  entry => entry.Key,
                  entry => entry.Value?.Errors.Select(error => error.ErrorMessage).ToList()
                );

                throw new BadRequestError("Invalid JSON Body", errors);
            }
        }

    }
}