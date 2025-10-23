using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;
namespace Cartify.Presentation.Controllers;


[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class V1BaseController : ControllerBase
{
}