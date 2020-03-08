using System.Threading.Tasks;
using Moist.Core;

namespace Moist.Configuration {
    public class ChangeShopProfileContext
    {
        private readonly IShopStore _shopStore;

        public class Form
        {
            public string UserId { get; set; }
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }
        
        public ChangeShopProfileContext(IShopStore shopStore)
        {
            _shopStore = shopStore;
        }

        public async Task<bool> Change(Form form)
        {
            if (!await _shopStore.UserCanChangeProfile(form.UserId, form.Id))
            {
                return false;
            }

            var profile = await _shopStore.GetProfile(form.Id);

            profile.Name = form.Name;
            profile.Description = form.Description;
            
            return true;
        }
    }
}