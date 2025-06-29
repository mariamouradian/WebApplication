using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Seminar4Application.Abstraction;
using Seminar4Application.DataStore.Entity;

namespace Seminar4Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(string login, string password)
        {
            var token = _userService.CheckUserRole(login, password);

            if(!token.IsNullOrEmpty())

            return Ok(token);

            return NotFound("User not found");
        }

        


        
    }
}
