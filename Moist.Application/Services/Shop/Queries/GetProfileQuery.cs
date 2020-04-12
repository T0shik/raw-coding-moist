using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moist.Core;

namespace Moist.Application.Services.Shop.Queries
{
    public class GetProfileQuery : IRequest<Core.Models.Shop>
    {
        public string UserId { get; set; }
    }

    public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, Core.Models.Shop>
    {
        private readonly IShopStore _store;

        public GetProfileQueryHandler(IShopStore store)
        {
            _store = store;
        }

        public Task<Core.Models.Shop> Handle(
            GetProfileQuery request,
            CancellationToken cancellationToken)
        {
            return _store.GetProfile(request.UserId, shop => shop);
        }
    }
}