namespace Web.Controllers;

[ApiController]
[ServiceFilter(typeof(LogUserActivity))]
[Route("api/[controller]")]
public class BaseApiController : ControllerBase
{
}