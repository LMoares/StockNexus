using FluentAssertions;
using Xunit;
using StockNexusAPI.Application.Features.Product.Commands.RegisterProduct;
using StockNexusAPI.Infrastructure.Persistence;
using MediatR;
using StockNexusAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace StockNexusAPI.UnitTests
{
    public class RegisterProductCommandHandlerTests
    {

        private ApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task Handle_ValidCommand_ShouldAddProductAndReturnId()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var handler = new RegisterProductCommandHandler(context);

            var command = new RegisterProductCommand
            {
                Name = "Test Product",
                Description = "Test Description",
                UnitPrice = 10.0m
            };

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);

            // Verify the product was added to the database
            var addedProduct = await context.Products.FirstOrDefaultAsync(p => p.Name == "Test Product");
            addedProduct.Should().NotBeNull();
            addedProduct.Description.Should().Be("Test Description");
            addedProduct.UnitPrice.Should().Be(10.0m);
        }

        [Fact]
        public async Task Handle_DuplicateProductName_ShouldThrowInvalidOperationException()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var handler = new RegisterProductCommandHandler(context);

            var existingProduct = new Product
            {
                Name = "Existing Product",
                Description = "Existing Description",
                UnitPrice = 20.0m
            };


            context.Products.Add(existingProduct);
            await context.SaveChangesAsync();
            var command = new RegisterProductCommand
            {
                Name = "Existing Product", 
                Description = "New Description",
                UnitPrice = 15.0m
            };

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>()
                .WithMessage("Product with name 'Existing Product' already exists.");

            var productsInDb = await context.Products
                .Where(x => x.Name == "Existing Product")
                .CountAsync();
            productsInDb.Should().Be(1);
        }
    }
}
