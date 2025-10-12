using System;
using System.Collections.Generic;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales
{
    public record GetAllSalesQuery : IRequest<IEnumerable<SaleResult>>;

    public record GetSaleByNumberQuery : IRequest<SaleResult>
    {
        public int SaleNumber { get; init; }
    }

    public record GetSalesByCustomerQuery : IRequest<IEnumerable<SaleResult>>
    {
        public string CustomerId { get; init; }
    }

    public record GetSalesByBranchQuery : IRequest<IEnumerable<SaleResult>>
    {
        public string BranchId { get; init; }
    }

    public record GetSalesByDateRangeQuery : IRequest<IEnumerable<SaleResult>>
    {
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }
}