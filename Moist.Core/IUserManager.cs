using System.Threading.Tasks;
using Moist.Core.Models;

namespace Moist.Core {
    public interface IUserManager
    {
        Task<SchemaProgress> GetProgressAsync(string customer, in int progressId);
    }
}