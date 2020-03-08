using System;
using System.Threading.Tasks;
using Moist.Core;
using Moist.Core.Models;
using Moq;
using Xunit;
using static Xunit.Assert;

namespace Moist.Configuration.Tests
{
    public class CreateSchemaTests
    {
        private readonly Mock<IShopStore> _shopMock = new Mock<IShopStore>();
        private readonly CreateSchemaContext _command;

        public CreateSchemaTests()
        {
            _command = new CreateSchemaContext(_shopMock.Object);
        }

        [Fact]
        public async Task SavesDaysVisitedSchema()
        {
            var form = new CreateSchemaContext.Form
            {
                Type = SchemaType.DaysVisited,
            };

            _shopMock.Setup(x => x.SaveDaysVisitedSchema(It.IsAny<DaysVisitedSchemaSchema>()))
                     .ReturnsAsync(true);

            var result = await _command.Create(form);

            True(result);
            _shopMock.Verify(x => x.SaveDaysVisitedSchema(It.IsAny<DaysVisitedSchemaSchema>()));
        }
    }
}