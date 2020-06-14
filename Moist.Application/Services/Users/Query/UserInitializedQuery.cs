using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moist.Database;

namespace Moist.Application.Services.Users.Query
{
    public class UserInitializedQuery : UserRequest, IQuery<Empty> { }

    public class UserInitializedQueryHandler : IHandler<UserInitializedQuery, Empty>
    {
        private readonly IMediator _mediator;
        private readonly AppDbContext _ctx;

        public UserInitializedQueryHandler(
            IMediator mediator,
            AppDbContext ctx)
        {
            _mediator = mediator;
            _ctx = ctx;
        }

        public Task<Response<Empty>> Handle(UserInitializedQuery request, CancellationToken cancellationToken)
        {
            var userExists = _ctx.Users.Any(x => x.UserId.Equals(request.UserId));

            return Task.FromResult(Response.Empty(userExists));
        }
    }
}