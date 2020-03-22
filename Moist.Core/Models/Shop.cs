using System.Collections.Generic;

namespace Moist.Core.Models
{
    public class Shop
    {
        public Shop()
        {
            Employees = new List<Employee>();
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        
        public ICollection<Schema> Schemas { get; set; }
        public ICollection<Employee> Employees { get; set; }

        // todo: list of employees
        
        // todo: address
        
    }
}