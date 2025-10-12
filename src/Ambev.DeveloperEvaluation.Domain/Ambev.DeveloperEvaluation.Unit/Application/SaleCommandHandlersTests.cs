using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Sales.Commands;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application
{
    public class SaleCommandHandlersTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly SaleCommandHandlers _handlers;

        public SaleCommandHandlersTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            _handlers = new SaleCommandHandlers(_saleRepository, _mapper);
        }

        [Fact]
        public async Task Handle_CreateSaleCommand_ShouldCreateSaleAndReturnSaleResult()
        {
            // Arrange
            var command = new CreateSaleCommand
            {
                CustomerId = "CUST123",
                CustomerName = "Test Customer",
                BranchId = "BRANCH123",
                BranchName = "Test Branch",
                Items = new List<SaleItemDto>
                {
                    new() { ProductId = "PROD1", ProductName = "Test Product", Quantity = 5, UnitPrice = 100 }
                }
            };

            _saleRepository.GetNextSaleNumberAsync().Returns(1);

            var expectedSale = Sale.Create(1, command.CustomerId, command.CustomerName, command.BranchId, command.BranchName);
            var expectedResult = new SaleResult 
            { 
                Id = expectedSale.Id,
                SaleNumber = expectedSale.SaleNumber,
                SaleDate = expectedSale.SaleDate,
                CustomerId = expectedSale.CustomerId,
                CustomerName = expectedSale.CustomerName,
                BranchId = expectedSale.BranchId,
                BranchName = expectedSale.BranchName,
                TotalAmount = expectedSale.TotalAmount,
                IsCancelled = expectedSale.IsCancelled,
                Items = new List<SaleItemResult>()
            };

            _mapper.Map<SaleResult>(Arg.Any<Sale>()).Returns(expectedResult);

            // Act
            var result = await _handlers.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(expectedResult, result);
            await _saleRepository.Received(1).AddAsync(Arg.Is<Sale>(s =>
                s.CustomerId == command.CustomerId &&
                s.CustomerName == command.CustomerName &&
                s.BranchId == command.BranchId &&
                s.BranchName == command.BranchName
            ));
        }

        [Fact]
        public async Task Handle_CancelSaleCommand_ShouldCancelSaleAndUpdate()
        {
            // Arrange
            var saleNumber = 1;
            var command = new CancelSaleCommand { SaleNumber = saleNumber };
            var sale = Sale.Create(saleNumber, "CUST123", "Test Customer", "BRANCH123", "Test Branch");
            
            _saleRepository.GetBySaleNumberAsync(saleNumber).Returns(sale);

            // Act
            await _handlers.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(sale.IsCancelled);
            await _saleRepository.Received(1).UpdateAsync(sale);
        }

        [Fact]
        public async Task Handle_CancelSaleCommand_WithNonexistentSale_ShouldThrowException()
        {
            // Arrange
            var command = new CancelSaleCommand { SaleNumber = 1 };
            _saleRepository.GetBySaleNumberAsync(1).Returns(Task.FromResult<Sale?>(null));

            // Act & Assert
            await Assert.ThrowsAsync<ApplicationException>(() =>
                _handlers.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_CancelSaleItemCommand_ShouldCancelItemAndUpdateSale()
        {
            // Arrange
            var saleNumber = 1;
            var productId = "PROD1";
            var command = new CancelSaleItemCommand { SaleNumber = saleNumber, ProductId = productId };
            
            var sale = Sale.Create(saleNumber, "CUST123", "Test Customer", "BRANCH123", "Test Branch");
            sale.AddItem(productId, "Test Product", 5, 100);
            
            _saleRepository.GetBySaleNumberAsync(saleNumber).Returns(sale);

            // Act
            await _handlers.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(sale.Items.Single().IsCancelled);
            await _saleRepository.Received(1).UpdateAsync(sale);
        }
    }
}