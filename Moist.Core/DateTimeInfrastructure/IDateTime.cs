using System;

namespace Moist.Core.DateTimeInfrastructure {
    public interface IDateTime
    {
        DateTime Now { get; }
    }
}