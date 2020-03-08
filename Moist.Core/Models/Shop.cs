using System.Collections.Generic;

namespace Moist.Core.Models
{
    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public ICollection<DaysVisitedSchemaSchema> DaysVisitedSchemas { get; set; }
        
        // todo: list of employees
        
        // todo: address
        
    }
}