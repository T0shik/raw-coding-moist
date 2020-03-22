using Moist.Core.Models;

namespace Moist.Core.Schemas
{
    public abstract class BaseSchema
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
    }
}