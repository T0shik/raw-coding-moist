using Moist.Core.DateTimeInfrastructure;
using Moist.Core.Models.Enums;

namespace Moist.Core.Schemas
{
    public abstract class BaseSchema
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public SchemaState State { get; set; }
        public abstract bool Valid(IDateTime dateTime);
        public abstract bool ReachedGoal(object progress);
    }
}