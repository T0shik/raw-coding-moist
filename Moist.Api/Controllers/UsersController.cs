using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moist.Application.Services.Users.Commands;
using Moist.Application.Services.Users.Query;

namespace Moist.Application.Api.Controllers
{
    [Route("api/users")]
    public class UsersController : BaseController
    {
        public UsersController(IMediator mediator) : base(mediator) { }


        [HttpGet("me/init")]
        public Task<Response<Empty>> UserInitialized()
        {
            return Mediator.Send(new UserInitializedQuery());
        }

        [HttpPost("me/init")]
        public Task<Response<Empty>> InitializeUser()
        {
            return Mediator.Send(new InitializeUserCommand());
        }
    }
}