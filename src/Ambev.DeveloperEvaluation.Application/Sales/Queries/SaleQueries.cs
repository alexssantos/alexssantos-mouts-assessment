using System;
using System.Collections.Generic;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries
{
    public record GetSaleByNumberQuery : IRequest<SaleDto>
    {
        public int SaleNumber { get; init; }
    }

    public record GetSalesByCustomerQuery : IRequest<IEnumerable<SaleDto>>
    {
        public string CustomerId { get; init; }
    }

    public record GetSalesByBranchQuery : IRequest<IEnumerable<SaleDto>>
    {
        public string BranchId { get; init; }
    }

    public record GetSalesByDateRangeQuery : IRequest<IEnumerable<SaleDto>>
    {
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }

    public record GetAllSalesQuery : IRequest<IEnumerable<SaleDto>>;

    public class SaleDto
    {
        public Guid Id { get; set; }
        public int SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string BranchId { get; set; }
        public string BranchName { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsCancelled { get; set; }
        public List<SaleItemDto> Items { get; set; } = new();
    }

    public class SaleItemDto
    {
        public Guid Id { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsCancelled { get; set; }
    }
}