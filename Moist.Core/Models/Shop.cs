using System.Collections.Generic;

namespace Moist.Core.Models
{
    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Schema> Schemas { get; } = new List<Schema>();
        public ICollection<Employee> Employees { get; } = new List<Employee>();
        public ICollection<Code> Codes { get; } = new List<Code>();

        // todo: list of employees

        // todo: address
    }
}