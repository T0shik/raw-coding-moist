using System;

namespace Moist.Core.Models {
    public class DaysVisitedSchemaConfiguration
    {
        public int Id { get; set; }
        public int Goal { get; set; }
        public bool Perpetual { get; set; }
        public DateTime ValidSince { get; set; }
        public DateTime ValidUntil { get; set; }
    }
}