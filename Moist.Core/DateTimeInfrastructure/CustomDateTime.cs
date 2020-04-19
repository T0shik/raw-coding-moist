using System;

namespace Moist.Core.DateTimeInfrastructure {
    public class CustomDateTime : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}