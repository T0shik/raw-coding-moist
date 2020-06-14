using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moist.Application.Services.ShopServices.Commands;
using Moist.Application.Services.ShopServices.Queries;
using Moist.Core.Models;

namespace Moist.Application.Api.Controllers
{
    [Route("api/[controller]")]
    public class ShopsController : BaseController
    {
        public ShopsController(IMediator mediator)
            : base(mediator) { }

        [HttpPost]
        public Task<Response<Shop>> CreateShopProfile([FromBody] CreateShopProfileCommand command)
        {
            return Mediator.Send(command);
        }

        [HttpGet]
        public Task<IAsyncEnumerable<Shop>> GetShops()
        {
            return Mediator.Send(new GetShopsQuery());
        }

        [HttpGet("me")]
        public Task<Response<Shop>> MyShopProfile()
        {
            return Mediator.Send(new GetProfileQuery());
        }


        [HttpGet("me/schemas")]
        public Task<Response<IEnumerable<Schema>>> MyShopSchemas([FromQuery] GetShopSchemasQuery query)
        {
            return Mediator.Send(query);
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


        [HttpPut]
        public Task<Response<Empty>> UpdateProfile(ChangeShopProfileCommand command)
        {
            return Mediator.Send(command);
        }


        [HttpPut("{shopId}/redeem")]
        public Task<Response<Empty>> CompleteRedemption([FromBody] CompleteRedemptionCommand command)
        {
            return Mediator.Send(command);
        }

        [HttpPost("{shopId}/reward/{schemaId}")]
        public Task<Response<string>> CreateRewardCode([FromRoute] GenerateRewardCommand command)
        {
            return Mediator.Send(command);
        }

        [HttpPut("{shopId}/reward/{schemaId}")]
        public Task<Response<Empty>> RewardSomething([FromRoute] RewardCustomerCommand command)
        {
            return Mediator.Send(command);
        }

        [HttpPut("{shopId}")]
        public Task<Response<Empty>> UpdateShopProfile([FromBody] ChangeShopProfileCommand command)
        {
            return Mediator.Send(command);
        }

    }
}