using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Moist.Core.Models;
using Moist.Database;

namespace Moist.Application.Services.ShopServices.Queries
{
    public class GetShopSchemasQuery : UserRequest, IQuery<IEnumerable<Schema>>
    {
        public SchemaStateFilter StateFilter { get; set; }
    }

    public class GetShopSchemasQueryHandler : IHandler<GetShopSchemasQuery, IEnumerable<Schema>>
    {
        private readonly AppDbContext _ctx;

        public GetShopSchemasQueryHandler(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Response<IEnumerable<Schema>>> Handle(
            GetShopSchemasQuery request,
            CancellationToken cancellationToken)
        {
            var profile = await _ctx.GetProfile(request.UserId, shop => shop);
            if (profile == null)
                return Response.Fail<IEnumerable<Schema>>("Profile Not Setup");

            var schemas = _ctx.Schemas.Where(x => request.StateFilter.Equals(SchemaStateFilter.All)
                                                  || x.State.Equals(request.StateFilter)).AsEnumerable();

            return Response.Ok(schemas);
        }
    }

    public enum SchemaStateFilter
    {
        All = -1,
        Started = 0,
        Active = 1,
        Closed = 2,
    }
}