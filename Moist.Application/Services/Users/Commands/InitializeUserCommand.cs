using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moist.Core.Models;
using Moist.Database;

namespace Moist.Application.Services.Users.Commands
{
    public class InitializeUserCommand : UserRequest, ICommand<Empty> {}

    public class InitializeUserCommandHandler : IHandler<InitializeUserCommand, Empty>
    {
        private readonly AppDbContext _ctx;

        public InitializeUserCommandHandler(AppDbContext ctx) => _ctx = ctx;

        public async Task<Response<Empty>> Handle(InitializeUserCommand request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrWhiteSpace(request.UserId)) return Response.Fail("Invalid User Id");

            var userExists = _ctx.Users.Any(x => x.UserId.Equals(request.UserId));

            if(userExists) return Response.Fail("User Already Exists");

            _ctx.Users.Add(new User {UserId = request.UserId});

            await _ctx.SaveChangesAsync(cancellationToken);

            return Response.Ok("User Initialized");
        }
    }
}