using System;

namespace Moist.Core.Models
{
    public class User
    {
        public User()
        {
            UserId = Guid.NewGuid().ToString();
        }

        public string UserId { get; set; }
    }
}