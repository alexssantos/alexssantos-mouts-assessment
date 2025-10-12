using System;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand>
    {
        private readonly ISaleRepository _saleRepository;

        public UpdateSaleHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetBySaleNumberAsync(request.SaleNumber);
            if (sale == null)
                throw new ApplicationException($"Sale {request.SaleNumber} not found");

            foreach (var item in request.Items)
            {
                sale.AddItem(
                    item.ProductId,
                    item.ProductName,
                    item.Quantity,
                    item.UnitPrice);
            }

            await _saleRepository.UpdateAsync(sale);
        }
    }
}