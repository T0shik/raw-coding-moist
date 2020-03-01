using System.Collections.Generic;

namespace Moist.Core.Models
{
    public class Customer
    {
        public string Id { get; set; }

        public ICollection<SchemaProgress> SchemaProgresses { get; set; }
    }
}