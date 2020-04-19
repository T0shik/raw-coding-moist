using System;
using Moist.Core.DateTimeInfrastructure;

namespace Moist.Core.Schemas
{
    public class DaysVisitedSchemaSchema : BaseSchema
    {
        public bool Perpetual { get; set; }
        public int Goal { get; set; }
        public DateTime ValidSince { get; set; }
        public DateTime ValidUntil { get; set; }

        public override bool Valid(IDateTime dateTime)
        {
            if (Perpetual) return true;

            var now = dateTime.Now;

            return now >= ValidSince && now <= ValidUntil;
        }

        public override bool ReachedGoal(object progress)
        {
            if (progress is int daysVisited)
            {
                return daysVisited == Goal;
            }
            return false;
        }
    }
}