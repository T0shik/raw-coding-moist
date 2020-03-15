namespace Moist.Core.Models
{
    public class Employee
    {
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
        public string UserId { get; set; }
        public bool CanChangeProfile { get; set; }
    }
}