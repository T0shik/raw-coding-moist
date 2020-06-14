using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Moist.Core.Models.Enums;

namespace Moist.Core.Models
{
    public class Schema
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public SchemaState State { get; set; }
        public SchemaType Type { get; set; }
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
        public bool Perpetual { get; set; }
        public int Goal { get; set; }
        public DateTime ValidSince { get; set; }
        public DateTime ValidUntil { get; set; }

        [JsonIgnore]
        public ICollection<Code> Codes { get; }
    }
}