namespace Web.Filters;

public class LogUserActivity : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var resultContext = await next();
        if (resultContext.HttpContext.User.Identity is {IsAuthenticated: false}) return;

        var usedId = resultContext.HttpContext.User.GetUserId();

        var repositoryUnit = resultContext.HttpContext.RequestServices.GetService<IRepositoryUnit>();
        if (repositoryUnit?.UserRepository == null) return;

        var user = await repositoryUnit.UserRepository.GetUserByIdAsync(usedId);
        user.LastActive = DateTime.UtcNow;

        await repositoryUnit.SaveChangesAsync();
    }
}