using System;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.CancelSale
{
    public class CancelSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly CancelSaleHandler _handler;

        public CancelSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _handler = new CancelSaleHandler(_saleRepository);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldCancelSale()
        {
            // Arrange
            var saleNumber = 1;
            var existingSale = Sale.Create(saleNumber, "CUST123", "Test Customer", "BRANCH123", "Test Branch");
            existingSale.AddItem("PROD1", "Test Product", 5, 100);

            var command = new CancelSaleCommand { SaleNumber = saleNumber };
            
            _saleRepository.GetBySaleNumberAsync(saleNumber).Returns(existingSale);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(existingSale.IsCancelled);
            Assert.All(existingSale.Items, item => Assert.True(item.IsCancelled));
            await _saleRepository.Received(1).UpdateAsync(existingSale);
        }

        [Fact]
        public async Task Handle_WithNonexistentSale_ShouldThrowException()
        {
            // Arrange
            var command = new CancelSaleCommand { SaleNumber = 1 };
            _saleRepository.GetBySaleNumberAsync(1).Returns((Sale)null);

            // Act & Assert
            await Assert.ThrowsAsync<ApplicationException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}