using System.Threading.Tasks;

namespace Moist.Core
{
    public interface IStore
    {
        public Task<bool> Save();
    }
}