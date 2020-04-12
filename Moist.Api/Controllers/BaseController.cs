using MediatR;

namespace Moist.Application.Api.Controllers
{
    public class BaseController
    {
        protected readonly IMediator Mediator;

        protected BaseController(IMediator mediator)
        {
            Mediator = mediator;
        }
    }
}