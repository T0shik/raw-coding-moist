using System.Threading.Tasks;
using Moist.Core.Models;

namespace Moist.Core {
    public interface IUserStore
    {
        Task<SchemaProgress> GetProgressAsync(string customerId, int progressId);
        Task<bool> InSchemaAsync(string customerId, int schemaId);
        Task<SchemaProgress> CreateSchemaProgressAsync(string customerId, int schemaId);
    }
}