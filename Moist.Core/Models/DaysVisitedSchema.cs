using System;

namespace Moist.Core.Models
{
    public class DaysVisitedSchemaSchema : BaseSchema
    {
        public bool Perpetual { get; set; }
        public int Goal { get; set; }
        public DateTime ValidSince { get; set; }
        public DateTime ValidUntil { get; set; }


        public bool Active(IDateTime dateTime)
        {
            if (Perpetual) return true;

            var now = dateTime.Now;

            return now >= ValidSince && now <= ValidUntil;
        }
    }
}