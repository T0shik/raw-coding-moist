using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Moist.Core.Models
{
    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonIgnore]
        public ICollection<Schema> Schemas { get; } = new List<Schema>();

        [JsonIgnore]
        public ICollection<Employee> Employees { get; } = new List<Employee>();
        [JsonIgnore]
        public ICollection<Code> Codes { get; } = new List<Code>();

        // todo: list of employees

        // todo: address
    }
}