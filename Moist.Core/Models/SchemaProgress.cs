using System;

namespace Moist.Core.Models {
    public class SchemaProgress
    {
        public int Id { get; set; }
        public string CustomerId { get; set; }
        public int SchemaId { get; set; }
        public int Progress { get; set; }
        
        public bool Completed { get; set; }
        public DateTime CompletedOn { get; set; }
    }
}