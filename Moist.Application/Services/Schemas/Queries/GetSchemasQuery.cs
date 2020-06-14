using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moist.Core.Models;
using Moist.Database;

namespace Moist.Application.Services.Schemas.Queries
{
    public class GetSchemasQuery : UserRequest, IQuery<IEnumerable<Schema>>
    {
    }

    public class GetSchemasQueryHandler : IHandler<GetSchemasQuery, IEnumerable<Schema>>
    {
        private readonly AppDbContext _ctx;

        public GetSchemasQueryHandler(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public Task<Response<IEnumerable<Schema>>> Handle(GetSchemasQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}