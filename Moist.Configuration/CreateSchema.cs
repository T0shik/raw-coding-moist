using System;
using System.Threading.Tasks;
using Moist.Core;
using Moist.Core.Models;

namespace Moist.Configuration
{
    public class CreateSchema
    {
        private readonly IShopStore _shopStore;

        public CreateSchema(IShopStore shopStore)
        {
            _shopStore = shopStore;
        }

        public class Form
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public SchemaType Type { get; set; }
            public bool Perpetual { get; set; }
            public int Goal { get; set; }
            public DateTime ValidSince { get; set; }
            public DateTime ValidUntil { get; set; }
        }

        public Task<bool> Create(Form form)
        {
            if (form.Type == SchemaType.DaysVisited)
            {
                return _shopStore.SaveDaysVisitedSchema(new DaysVisitedSchemaSchema
                {
                    Title = form.Title,
                    Description = form.Description,
                    Perpetual = form.Perpetual,
                    Goal = form.Goal,
                    ValidSince = form.ValidSince,
                    ValidUntil = form.ValidUntil
                });
            }

            return Task.FromResult(false);
        }
    }
}