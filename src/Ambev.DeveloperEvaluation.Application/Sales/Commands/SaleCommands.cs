using System;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands
{
    public record CreateSaleCommand : IRequest<SaleResult>
    {
        public required string CustomerId { get; init; }
        public required string CustomerName { get; init; }
        public required string BranchId { get; init; }
        public required string BranchName { get; init; }
        public required List<SaleItemDto> Items { get; init; } = new();
    }

    public record UpdateSaleCommand : IRequest
    {
        public required int SaleNumber { get; init; }
        public required List<SaleItemDto> Items { get; init; } = new();
    }

    public record CancelSaleCommand : IRequest
    {
        public required int SaleNumber { get; init; }
    }

    public record CancelSaleItemCommand : IRequest
    {
        public required int SaleNumber { get; init; }
        public required string ProductId { get; init; }
    }

    public record SaleItemDto
    {
        public required string ProductId { get; init; }
        public required string ProductName { get; init; }
        public required int Quantity { get; init; }
        public required decimal UnitPrice { get; init; }
    }
}