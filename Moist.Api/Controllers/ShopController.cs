using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moist.Application.Services.Shop.Commands;
using Moist.Application.Services.Shop.Queries;
using Moist.Core.Models;

namespace Moist.Application.Api.Controllers
{
    [Route("/shops")]
    public class ShopController : BaseController
    {
        public ShopController(IMediator mediator)
            : base(mediator) { }

        [HttpGet]
        public Task<IAsyncEnumerable<Shop>> GetShops()
        {
            return Mediator.Send(new GetShopsQuery());
        }

        [HttpGet("{id}")]
        public Shop GetShop(int id)
        {
            return new Shop {Id = id};
        }

        [HttpGet("code")]
        public Task<Response<string>> ShopCode(GenerateRewardCommand command)
        {
            return Mediator.Send(command);
        }

        public Task<Core.Models.Shop> Index()
        {
            return Mediator.Send(new GetProfileQuery());
        }

        [HttpPut]
        public Task<Response> UpdateProfile(ChangeShopProfileCommand command)
        {
            return Mediator.Send(command);
        }
    }
}