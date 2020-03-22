using System;
using Moist.Core.Schemas;

namespace Moist.Core.Models
{
    public class Schema
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
        public bool Perpetual { get; set; }
        public int Goal { get; set; }
        public DateTime ValidSince { get; set; }
        public DateTime ValidUntil { get; set; }
    }
}