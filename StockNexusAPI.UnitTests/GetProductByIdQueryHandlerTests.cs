using FluentAssertions;
using Xunit;
using StockNexusAPI.Application.Features.Product.Commands.RegisterProduct;
using StockNexusAPI.Infrastructure.Persistence;
using MediatR;
using StockNexusAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using StockNexusAPI.Application.Features.Product.Queries.GetProductById;

namespace StockNexusAPI.UnitTests
{
    public class GetProductByIdQueryHandlerTests
    {
        private ApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task Handle_ValidId_ShouldReturnProductDto()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var registerHandler = new RegisterProductCommandHandler(context);

            var registerCommand = new RegisterProductCommand
            {
                Name = "Test Product",
                Description = "Test Description",
                UnitPrice = 10.0m
            };

            await registerHandler.Handle(registerCommand, CancellationToken.None);

            var queryHandler = new GetProductByIdQueryHandler(context);

            var queryCommand = new GetProductByIdQuery
            {
                Id = 1
            };

            // Act
            var result = await queryHandler.Handle(queryCommand, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Name.Should().Be("Test Product");
            result.Description.Should().Be("Test Description");
            result.UnitPrice.Should().Be(10.0m);
        }

        [Fact]
        public async Task Handle_InvalidId_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var queryHandler = new GetProductByIdQueryHandler(context);

            var command = new GetProductByIdQuery
            {
                Id = 999
            };

            // Act
            Func<Task> act = async () => await queryHandler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>().WithMessage("Product with Id 999 not found.");

            var doesProductExist = await context.Products.AnyAsync(x => x.Id == 999);
            doesProductExist.Should().BeFalse();

        }
    }
}
