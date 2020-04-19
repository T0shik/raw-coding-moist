﻿using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moist.Core;
using Moist.Core.DateTimeInfrastructure;
using Moist.Core.Schemas;

namespace Moist.Application.Services.Shop.Commands
{
    public class RewardCustomerCommand : IRequest<Response>
    {
        public string UserId { get; set; }
        public int SchemaId { get; set; }
        public string Code { get; set; }
    }
    public class RewardCustomerCommandHandler : IRequestHandler<RewardCustomerCommand, Response>
    {
        private readonly IShopStore _shopStore;
        private readonly IUserStore _userStore;
        private readonly ICodeStore _codeStore;
        private readonly IDateTime _dateTime;

        public RewardCustomerCommandHandler(
            IShopStore shopStore,
            IUserStore userStore,
            ICodeStore codeStore,
            IDateTime dateTime)
        {
            _shopStore = shopStore;
            _userStore = userStore;
            _codeStore = codeStore;
            _dateTime = dateTime;
        }

        public async Task<Response> Handle(RewardCustomerCommand request, CancellationToken cancellationToken)
        {
            var progress = await _userStore.GetProgressAsync(request.UserId, request.SchemaId);

            var schema = await _shopStore.GetSchema(progress.SchemaId);
            var assignedSchema = SchemaFactory.Resolve(schema);
            if (!assignedSchema.Valid(_dateTime))
                return Response.Fail<Response>(null,  "Schema not valid");

            var result = await _codeStore.ValidateRewardCode(schema.ShopId, request.Code);
            if (!result.Success)
                return Response.Fail<Response>(null,  "Invalid Code");

            progress.Progress++;
            return Response.Ok("Complete");
        }
    }
}