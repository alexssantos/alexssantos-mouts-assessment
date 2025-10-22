using System;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem
{
    public class CancelSaleItemHandler : IRequestHandler<CancelSaleItemCommand>
    {
        private readonly ISaleRepository _saleRepository;

        public CancelSaleItemHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task Handle(CancelSaleItemCommand request, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetBySaleNumberAsync(request.SaleNumber);
            if (sale == null)
                throw new ApplicationException($"Sale {request.SaleNumber} not found");

            sale.CancelItem(request.ProductId);
            await _saleRepository.UpdateAsync(sale);
        }
    }
}