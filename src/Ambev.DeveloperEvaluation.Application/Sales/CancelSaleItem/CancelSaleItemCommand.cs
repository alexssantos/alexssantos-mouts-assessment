using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem
{
    public record CancelSaleItemCommand : IRequest
    {
        public int SaleNumber { get; init; }
        public string ProductId { get; init; }
    }
}