using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moist.Application.Services.Schemas.Commands;
using Moist.Core.Models;

namespace Moist.Application.Api.Controllers
{
    [Route("api/[controller]")]
    public class SchemasController : BaseController
    {
        public SchemasController(IMediator mediator) : base(mediator) { }

        [HttpPost("{shopId}/schemas/{schemaId}")]
        public Task<Response<Schema>> ActivateShopSchema([FromRoute] CreateSchemaCommand command)
        {
            return Mediator.Send(command);
        }

        [HttpPut("{shopId}/schemas/{schemaId}")]
        public Task<Response<Empty>> ActivateShopSchema([FromRoute] ActivateSchemaCommand command)
        {
            return Mediator.Send(command);
        }

        [HttpDelete("{shopId}/schemas/{schemaId}")]
        public Task<Response<Empty>> CloseShopSchema([FromRoute] CloseSchemaCommand command)
        {
            return Mediator.Send(command);
        }
    }
}