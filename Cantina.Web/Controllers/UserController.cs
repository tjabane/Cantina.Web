using Cantina.Application.UseCase.User.Commands.CreateUser;
using Cantina.Application.UseCase.User.Queries.Login;
using Cantina.Web.Dto;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cantina.Web.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            var result = await _mediator.Send(new CreateUserCommand(userDto.FullName, userDto.Email, userDto.Password));
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto userDto)
        {
            var result = await _mediator.Send(new LoginQuery(userDto.Email, userDto.Password));
            if (result.IsFailed)
                return BadRequest(result.Errors);
            return Ok(new { token = result.Value });
        }
    }
}
