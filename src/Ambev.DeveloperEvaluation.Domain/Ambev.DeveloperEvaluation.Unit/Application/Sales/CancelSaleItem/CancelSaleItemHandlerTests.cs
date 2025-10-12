using System;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.CancelSaleItem
{
    public class CancelSaleItemHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly CancelSaleItemHandler _handler;

        public CancelSaleItemHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _handler = new CancelSaleItemHandler(_saleRepository);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldCancelSaleItem()
        {
            // Arrange
            var saleNumber = 1;
            var productId = "PROD1";
            var existingSale = Sale.Create(saleNumber, "CUST123", "Test Customer", "BRANCH123", "Test Branch");
            existingSale.AddItem(productId, "Test Product", 5, 100);

            var command = new CancelSaleItemCommand 
            { 
                SaleNumber = saleNumber,
                ProductId = productId
            };
            
            _saleRepository.GetBySaleNumberAsync(saleNumber).Returns(existingSale);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(existingSale.Items.Single().IsCancelled);
            await _saleRepository.Received(1).UpdateAsync(existingSale);
        }

        [Fact]
        public async Task Handle_WithNonexistentSale_ShouldThrowException()
        {
            // Arrange
            var command = new CancelSaleItemCommand
            {
                SaleNumber = 1,
                ProductId = "PROD1"
            };
            _saleRepository.GetBySaleNumberAsync(1).Returns((Sale)null);

            // Act & Assert
            await Assert.ThrowsAsync<ApplicationException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}