using System;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public record CreateSaleCommand : IRequest<SaleResult>
    {
        public required string CustomerId { get; init; }
        public required string CustomerName { get; init; }
        public required string BranchId { get; init; }
        public required string BranchName { get; init; }
        public required List<CreateSaleItemDto> Items { get; init; } = new();
    }

    public record CreateSaleItemDto
    {
        public required string ProductId { get; init; }
        public required string ProductName { get; init; }
        public required int Quantity { get; init; }
        public required decimal UnitPrice { get; init; }
    }
}