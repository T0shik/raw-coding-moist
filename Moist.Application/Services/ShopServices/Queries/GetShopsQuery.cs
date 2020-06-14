using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moist.Core;

namespace Moist.Application.Services.ShopServices.Queries
{
    public class GetShopsQuery : IRequest<IAsyncEnumerable<Core.Models.Shop>> { }

    public class GetShopsQueryHandler : IRequestHandler<GetShopsQuery, IAsyncEnumerable<Core.Models.Shop>>
    {
        private readonly IShopStore _store;

        public GetShopsQueryHandler(IShopStore store)
        {
            _store = store;
        }

        public Task<IAsyncEnumerable<Core.Models.Shop>> Handle(
            GetShopsQuery request,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(_store.GetShops(shop => shop));
        }
    }
}