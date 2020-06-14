using System.Threading;
using System.Threading.Tasks;
using Moist.Application.Services.Schemas.Commands;
using Moist.Core.Models;
using Moq;
using Xunit;
using static Xunit.Assert;

namespace Moist.Core.Tests
{
    public class CreateSchemaTests
    {
        private readonly Mock<IShopStore> _shopMock = new Mock<IShopStore>();
        private readonly CreateSchemaCommandHandler _handler;

        public CreateSchemaTests()
        {
            _handler = new CreateSchemaCommandHandler(_shopMock.Object);
        }

        [Fact]
        public async Task SavesDaysVisitedSchema()
        {
            var command = new CreateSchemaCommand
            {
                Type = SchemaType.DaysVisited,
            };

            _shopMock.Setup(x => x.SaveSchema(It.IsAny<Schema>()))
                     .ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            False(result.Error);
            _shopMock.Verify(x => x.SaveSchema(It.IsAny<Schema>()));
        }
    }
}