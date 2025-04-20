using Cantina.Core.Dto;
using Cantina.Core.UseCase.User.Commands.CreateUser;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Cantina.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            var result = await  _mediator.Send(new CreateUserCommand(userDto.FullName, userDto.Email, userDto.Password));
            if(!result.Succeeded)
                return BadRequest(result.Errors);
            return Ok("User registered successfully.");
        }
    }
}
