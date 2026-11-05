using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace TaskApi.Controllers;

public class BaseApiController : ControllerBase
{
    protected int UserID => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
