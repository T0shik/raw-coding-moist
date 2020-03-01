using System;

namespace Moist.Core.Models
{
    public class DaysVisitedSchemaConfiguration : ISchema
    {
        public int Id { get; set; }
        public int Goal { get; set; }
        public bool Perpetual { get; set; }
        public DateTime ValidSince { get; set; }
        public DateTime ValidUntil { get; set; }

        public int ShopId { get; set; }
        public Shop Shop { get; set; }

        public bool Active(IDateTime dateTime)
        {
            if (Perpetual) return true;

            var now = dateTime.Now;

            return now >= ValidSince && now <= ValidUntil;
        }
    }
}