
namespace Moist.Core.Models
{
    public abstract class BaseSchema
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; } 
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
    }
}