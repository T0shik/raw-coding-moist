using System.Threading.Tasks;

namespace Moist.Database.Stores
{
    public abstract class BaseStore
    {
        protected readonly AppDbContext Db;
        protected BaseStore(AppDbContext ctx) => Db = ctx;

        public async Task<bool> Save() => await Db.SaveChangesAsync() > 0;
    }
}