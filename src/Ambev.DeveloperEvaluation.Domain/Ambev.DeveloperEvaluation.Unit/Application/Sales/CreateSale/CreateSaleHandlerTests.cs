using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.CreateSale
{
    public class CreateSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly CreateSaleHandler _handler;

        public CreateSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new CreateSaleHandler(_saleRepository, _mapper);
        }

        [Fact]
        public async Task Handle_WithValidCommand_ShouldCreateSaleAndReturnSaleResult()
        {
            // Arrange
            var command = new CreateSaleCommand
            {
                CustomerId = "CUST123",
                CustomerName = "Test Customer",
                BranchId = "BRANCH123",
                BranchName = "Test Branch",
                Items = new List<CreateSaleItemDto>
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
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(expectedResult, result);
            await _saleRepository.Received(1).AddAsync(Arg.Is<Sale>(s =>
                s.CustomerId == command.CustomerId &&
                s.CustomerName == command.CustomerName &&
                s.BranchId == command.BranchId &&
                s.BranchName == command.BranchName
            ));
        }
    }
}