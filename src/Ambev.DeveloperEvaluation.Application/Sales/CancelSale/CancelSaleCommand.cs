using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    public record CancelSaleCommand : IRequest
    {
        public int SaleNumber { get; init; }
    }
}