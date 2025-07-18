using Api.Entities;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly LoginServices _loginServices;

    public AuthController(LoginServices loginServices)
    {
        _loginServices = loginServices;
    }




    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var token = await _loginServices.LoginAppUser(model);

        if (token == null)
        {
            return Unauthorized();
        }

        return Ok(new { Token = token });
    }
}


