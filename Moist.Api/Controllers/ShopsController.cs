using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moist.Application.Services.Schema.Commands;
using Moist.Application.Services.Shop.Commands;
using Moist.Application.Services.Shop.Queries;
using Moist.Core.Models;

namespace Moist.Application.Api.Controllers
{
    [Route("api/[controller]")]
    public class ShopsController : BaseController
    {
        public ShopsController(IMediator mediator)
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

        public Task<Shop> Index()
        {
            return Mediator.Send(new GetProfileQuery());
        }

        [HttpPut]
        public Task<Response> UpdateProfile(ChangeShopProfileCommand command)
        {
            return Mediator.Send(command);
        }


        [HttpPut("{shopId}/redeem")]
        public Task<Response> CompleteRedemption([FromBody] CompleteRedemptionCommand command)
        {
            return Mediator.Send(command);
        }

        [HttpPost("{shopId}/reward/{schemaId}")]
        public Task<Response<string>> CreateRewardCode([FromRoute] GenerateRewardCommand command)
        {
            return Mediator.Send(command);
        }

        [HttpPut("{shopId}/reward/{schemaId}")]
        public Task<Response> RewardSomething([FromRoute] RewardCustomerCommand command)
        {
            return Mediator.Send(command);
        }

        [HttpPut("{shopId}")]
        public Task<Response> UpdateShopProfile([FromBody] ChangeShopProfileCommand command)
        {
            return Mediator.Send(command);
        }

        [HttpPost]
        public Task<Response> InitialiseShop()
        {
            return Mediator.Send(new InitialiseShopCommand());
        }

        [HttpPost("{shopId}/schemas/{schemaId}")]
        public Task<Response> ActivateShopSchema([FromRoute] CreateSchemaCommand command)
        {
            return Mediator.Send(command);
        }

        [HttpPut("{shopId}/schemas/{schemaId}")]
        public Task<Response> ActivateShopSchema([FromRoute] ActivateSchemaCommand command)
        {
            return Mediator.Send(command);
        }

        [HttpDelete("{shopId}/schemas/{schemaId}")]
        public Task<Response> CloseShopSchema([FromRoute] CloseSchemaCommand command)
        {
            return Mediator.Send(command);
        }
    }
}