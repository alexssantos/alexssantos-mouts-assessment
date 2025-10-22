using System;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.UpdateSale
{
    public class UpdateSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly UpdateSaleHandler _handler;

        public UpdateSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _handler = new UpdateSaleHandler(_saleRepository);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldUpdateSale()
        {
            // Arrange
            var saleNumber = 1;
            var existingSale = Sale.Create(saleNumber, "CUST123", "Test Customer", "BRANCH123", "Test Branch");
            
            var command = new UpdateSaleCommand
            {
                SaleNumber = saleNumber,
                Items = new List<UpdateSaleItemDto>
                {
                    new() { ProductId = "PROD1", ProductName = "Test Product", Quantity = 5, UnitPrice = 100 }
                }
            };

            _saleRepository.GetBySaleNumberAsync(saleNumber).Returns(existingSale);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            await _saleRepository.Received(1).UpdateAsync(Arg.Is<Sale>(s =>
                s.Id == existingSale.Id &&
                s.Items.Count == 1
            ));
        }

        [Fact]
        public async Task Handle_WithNonexistentSale_ShouldThrowException()
        {
            // Arrange
            var command = new UpdateSaleCommand
            {
                SaleNumber = 1,
                Items = new List<UpdateSaleItemDto>()
            };

            _saleRepository.GetBySaleNumberAsync(1).Returns((Sale)null);

            // Act & Assert
            await Assert.ThrowsAsync<ApplicationException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}