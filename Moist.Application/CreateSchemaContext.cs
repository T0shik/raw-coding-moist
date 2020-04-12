using System;
using System.Threading.Tasks;
using Moist.Core;
using Moist.Core.Models;

namespace Moist.Configuration
{
    public class CreateSchemaContext
    {
        private readonly IShopStore _shopStore;

        public CreateSchemaContext(IShopStore shopStore)
        {
            _shopStore = shopStore;
        }

        public class Form
        {
            public string UserId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public SchemaType Type { get; set; }
            public bool Perpetual { get; set; }
            public int Goal { get; set; }
            public DateTime ValidSince { get; set; }
            public DateTime ValidUntil { get; set; }
        }

        public async Task<bool> Create(Form form)
        {
            var shopId = await _shopStore.GetUsersShopId(form.UserId);

            if (form.Type == SchemaType.DaysVisited)
            {
                return await _shopStore.SaveSchema(new Schema
                {
                    ShopId = shopId,
                    Title = form.Title,
                    Description = form.Description,
                    Perpetual = form.Perpetual,
                    Goal = form.Goal,
                    ValidSince = form.ValidSince,
                    ValidUntil = form.ValidUntil
                });
            }

            return false;
        }
    }
}