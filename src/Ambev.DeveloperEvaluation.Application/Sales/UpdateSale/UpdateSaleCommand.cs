using System.Collections.Generic;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public record UpdateSaleCommand : IRequest
    {
        public int SaleNumber { get; init; }
        public List<UpdateSaleItemDto> Items { get; init; } = new();
    }

    public record UpdateSaleItemDto
    {
        public string ProductId { get; init; }
        public string ProductName { get; init; }
        public int Quantity { get; init; }
        public decimal UnitPrice { get; init; }
    }
}