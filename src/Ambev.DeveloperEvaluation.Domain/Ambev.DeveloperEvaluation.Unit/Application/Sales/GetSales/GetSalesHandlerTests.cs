using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.GetSales
{
    public class GetSalesHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly GetSalesHandler _handler;
        private readonly Sale _testSale;

        public GetSalesHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetSalesHandler(_saleRepository, _mapper);

            _testSale = Sale.Create(1, "CUST123", "Test Customer", "BRANCH123", "Test Branch");
            _testSale.AddItem("PROD1", "Test Product", 5, 100);
        }

        [Fact]
        public async Task Handle_GetAllSalesQuery_ShouldReturnAllSales()
        {
            // Arrange
            var sales = new List<Sale> { _testSale };
            var expectedResult = new List<SaleResult> { new() };

            _saleRepository.GetAllAsync().Returns(sales);
            _mapper.Map<IEnumerable<SaleResult>>(sales).Returns(expectedResult);

            // Act
            var result = await _handler.Handle(new GetAllSalesQuery(), CancellationToken.None);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task Handle_GetSaleByNumberQuery_ShouldReturnSale()
        {
            // Arrange
            var query = new GetSaleByNumberQuery { SaleNumber = 1 };
            var expectedResult = new SaleResult();

            _saleRepository.GetBySaleNumberAsync(query.SaleNumber).Returns(_testSale);
            _mapper.Map<SaleResult>(_testSale).Returns(expectedResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task Handle_GetSalesByCustomerQuery_ShouldReturnCustomerSales()
        {
            // Arrange
            var query = new GetSalesByCustomerQuery { CustomerId = "CUST123" };
            var sales = new List<Sale> { _testSale };
            var expectedResult = new List<SaleResult> { new() };

            _saleRepository.GetByCustomerIdAsync(query.CustomerId).Returns(sales);
            _mapper.Map<IEnumerable<SaleResult>>(sales).Returns(expectedResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task Handle_GetSalesByBranchQuery_ShouldReturnBranchSales()
        {
            // Arrange
            var query = new GetSalesByBranchQuery { BranchId = "BRANCH123" };
            var sales = new List<Sale> { _testSale };
            var expectedResult = new List<SaleResult> { new() };

            _saleRepository.GetByBranchIdAsync(query.BranchId).Returns(sales);
            _mapper.Map<IEnumerable<SaleResult>>(sales).Returns(expectedResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task Handle_GetSalesByDateRangeQuery_ShouldReturnSalesInRange()
        {
            // Arrange
            var query = new GetSalesByDateRangeQuery 
            { 
                StartDate = DateTime.UtcNow.AddDays(-1),
                EndDate = DateTime.UtcNow
            };
            var sales = new List<Sale> { _testSale };
            var expectedResult = new List<SaleResult> { new() };

            _saleRepository.GetByDateRangeAsync(query.StartDate, query.EndDate).Returns(sales);
            _mapper.Map<IEnumerable<SaleResult>>(sales).Returns(expectedResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(expectedResult, result);
        }
    }
}