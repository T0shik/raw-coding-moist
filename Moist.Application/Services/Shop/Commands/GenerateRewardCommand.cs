using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moist.Core;

namespace Moist.Application.Services.Shop.Commands
{
    public class GenerateRewardCommand : IRequest<Response<string>>
    {
        public string UserId { get; set; }
        public int ShopId { get; set; }
        public int SchemaId { get; set; }
    }

    public class GenerateRewardCommandHandler : IRequestHandler<GenerateRewardCommand, Response<string>>
    {
        private readonly IShopStore _store;
        private readonly ICodeStore _codeStore;

        public GenerateRewardCommandHandler(
            IShopStore store,
            ICodeStore codeStore)
        {
            _store = store;
            _codeStore = codeStore;
        }
        public async Task<Response<string>> Handle(GenerateRewardCommand request, CancellationToken cancellationToken)
        {
            if (!await _store.UserCanGenerateSchemaCode(request.UserId, request.ShopId))
            {
                return Response.Fail<string>(null, "Not allowed to generate rewards");
            }

            var code = await _codeStore.CreateSchemaRewardCode(request.ShopId, request.SchemaId);

            return Response.Ok(code, "Code created");
        }
    }
}