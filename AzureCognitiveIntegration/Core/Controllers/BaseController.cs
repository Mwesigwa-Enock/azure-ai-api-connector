using Microsoft.AspNetCore.Mvc;

namespace AzureCognitiveIntegration.Core.Controllers;

/// <summary>
/// BaseController
/// </summary>
[Produces("application/json")]
[ProducesResponseType(typeof(OkResult), 200)]
[ProducesResponseType(typeof(NotFoundResult), 400)]
[ProducesResponseType(typeof(UnauthorizedResult), 401)]
[ProducesResponseType(500)]
[ApiController]
public class BaseController : Controller
{
    
}