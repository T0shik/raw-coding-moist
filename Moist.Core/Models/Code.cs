using System;

namespace Moist.Core.Models
{
    public class Code
    {
        public Code()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public DateTime Redeemed { get; set; }

        public string CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int SchemaId { get; set; }
        public Schema Schema { get; set; }

        public int ShopId { get; set; }
        public Shop Shop { get; set; }
    }
}