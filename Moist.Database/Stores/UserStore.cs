using System.Threading.Tasks;
using Moist.Core;
using Moist.Core.Models;

namespace Moist.Database.Stores
{
    public class UserStore : BaseStore, IUserStore
    {
        public UserStore(AppDbContext ctx)
            : base(ctx) { }

        public Task<SchemaProgress> GetProgressAsync(string customerId, int progressId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> InSchemaAsync(string customerId, int schemaId)
        {
            throw new System.NotImplementedException();
        }

        public Task<SchemaProgress> CreateSchemaProgressAsync(string customerId, int schemaId)
        {
            throw new System.NotImplementedException();
        }
    }
}