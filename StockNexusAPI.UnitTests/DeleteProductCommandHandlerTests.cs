using FluentAssertions;
using Xunit;
using StockNexusAPI.Infrastructure.Persistence;
using MediatR;
using StockNexusAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using StockNexusAPI.Application.Features.Product.Commands.DeleteProduct;
using StockNexusAPI.Application.Features.Product.Commands.RegisterProduct;

namespace StockNexusAPI.UnitTests
{
    public class DeleteProductCommandHandlerTests
    {

        private ApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldDeleteProductAndReturnUnitValue()
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

            var deleteHandler = new DeleteProductCommandHandler(context);

            var deleteCommand = new DeleteProductCommand
            {
                Id = 1
            };

            // Act
            var result = await deleteHandler.Handle(deleteCommand, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();

            var isProductDeleted = await context.Products.FindAsync(deleteCommand.Id) == null;
            isProductDeleted.Should().BeTrue();

        }

        [Fact]
        public async Task Handle_InvalidCommand_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var context = CreateInMemoryContext();

            var handler = new DeleteProductCommandHandler(context);

            var command = new DeleteProductCommand()
            {
                Id = 999
            };

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage("Product with Id 999 not found.");

            var doesProductExist = await context.Products.FindAsync(command.Id) != null;
            doesProductExist.Should().BeFalse();
        }

        
    }
}
