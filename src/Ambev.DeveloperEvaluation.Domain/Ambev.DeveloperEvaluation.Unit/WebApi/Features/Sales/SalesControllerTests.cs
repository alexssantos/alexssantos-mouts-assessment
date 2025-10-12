using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Features.Sales
{
    public class SalesControllerTests
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly SalesController _controller;

        public SalesControllerTests()
        {
            _mediator = Substitute.For<IMediator>();
            
            var config = new MapperConfiguration(cfg => 
            {
                cfg.AddProfile<GetSaleProfile>();
                cfg.AddProfile<CreateSaleProfile>();
                cfg.AddProfile<CancelSaleProfile>();
            });
            _mapper = config.CreateMapper();
            
            _controller = new SalesController(_mediator, _mapper);
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllSales()
        {
            // Arrange
            var sales = new List<SaleResult> { new(), new() };
            _mediator.Send(Arg.Any<GetAllSalesQuery>(), Arg.Any<CancellationToken>())
                .Returns(sales);

            // Act
            var result = await _controller.GetAll(CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponseWithData<IEnumerable<GetSaleResponse>>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(sales.Count, response.Data?.Count());
        }

        [Fact]
        public async Task GetBySaleNumber_WithExistingSale_ShouldReturnSale()
        {
            // Arrange
            var sale = new SaleResult { SaleNumber = 1 };
            _mediator.Send(Arg.Any<GetSaleByNumberQuery>(), Arg.Any<CancellationToken>())
                .Returns(sale);

            var request = new GetSaleRequest { SaleNumber = 1 };

            // Act
            var result = await _controller.GetBySaleNumber(request, CancellationToken.None);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ApiResponseWithData<GetSaleResponse>>(okResult.Value);
            Assert.True(response.Success);
            Assert.Equal(sale.SaleNumber, response.Data?.SaleNumber);
        }

        [Fact]
        public async Task GetBySaleNumber_WithNonexistentSale_ShouldReturnNotFound()
        {
            // Arrange
            _mediator.Send(Arg.Any<GetSaleByNumberQuery>(), Arg.Any<CancellationToken>())
                .Returns((SaleResult?)null);

            var request = new GetSaleRequest { SaleNumber = 1 };

            // Act
            var result = await _controller.GetBySaleNumber(request, CancellationToken.None);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var response = Assert.IsType<ApiResponse>(notFoundResult.Value);
            Assert.False(response.Success);
            Assert.Equal("Sale not found", response.Message);
        }

        [Fact]
        public async Task GetBySaleNumber_WithInvalidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var request = new GetSaleRequest { SaleNumber = 0 }; // Invalid sale number

            // Act
            var result = await _controller.GetBySaleNumber(request, CancellationToken.None);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ApiResponse>(badRequestResult.Value);
            Assert.False(response.Success);
            Assert.Equal("Validation failed", response.Message);
            Assert.NotEmpty(response.Errors);
        }

        [Fact]
        public async Task Create_WithValidRequest_ShouldReturnCreatedResponse()
        {
            // Arrange
            var request = new CreateSaleRequest
            {
                CustomerId = "CUST001",
                BranchId = "BRANCH001",
                SaleDate = DateTime.UtcNow,
                Items = new List<CreateSaleItemRequest>
                {
                    new() { ProductId = "PROD001", Quantity = 1, UnitPrice = 10.0m }
                }
            };

            var expectedId = Guid.NewGuid();
            var expectedSaleNumber = 1;

            _mediator.Send(Arg.Any<CreateSaleCommand>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(expectedSaleNumber));

            // Act
            var response = await _controller.Create(request, CancellationToken.None);

            // Assert
            var createdAtResult = Assert.IsType<CreatedAtActionResult>(response);
            var apiResponse = Assert.IsType<ApiResponseWithData<CreateSaleResponse>>(createdAtResult.Value);
            
            Assert.True(apiResponse.Success);
            Assert.NotNull(apiResponse.Data);
            Assert.Equal(expectedId, apiResponse.Data.Id);
            Assert.Equal(expectedSaleNumber, apiResponse.Data.SaleNumber);
            Assert.Equal(request.CustomerId, apiResponse.Data.CustomerId);
        }

        [Fact]
        public async Task CancelSale_WithValidSaleNumber_ShouldReturnNoContent()
        {
            // Arrange
            var saleNumber = 1;

            // Act
            var result = await _controller.CancelSale(saleNumber);

            // Assert
            Assert.IsType<NoContentResult>(result);
            await _mediator.Received(1).Send(
                Arg.Is<CancelSaleCommand>(c => c.SaleNumber == saleNumber),
                Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task CancelSaleItem_WithValidData_ShouldReturnNoContent()
        {
            // Arrange
            var saleNumber = 1;
            var productId = "PROD1";

            // Act
            var result = await _controller.CancelSaleItem(saleNumber, productId);

            // Assert
            Assert.IsType<NoContentResult>(result);
            await _mediator.Received(1).Send(
                Arg.Is<CancelSaleItemCommand>(c => 
                    c.SaleNumber == saleNumber && 
                    c.ProductId == productId),
                Arg.Any<CancellationToken>());
        }
    }
}