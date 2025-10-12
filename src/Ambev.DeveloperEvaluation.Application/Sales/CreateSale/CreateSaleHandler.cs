using System;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, SaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<SaleResult> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            var saleNumber = await _saleRepository.GetNextSaleNumberAsync(cancellationToken);
            
            var sale = Sale.Create(
                saleNumber,
                request.CustomerId,
                request.CustomerName,
                request.BranchId,
                request.BranchName);

            foreach (var item in request.Items)
            {
                sale.AddItem(
                    item.ProductId,
                    item.ProductName,
                    item.Quantity,
                    item.UnitPrice);
            }

            await _saleRepository.AddAsync(sale, cancellationToken);
            return _mapper.Map<SaleResult>(sale);
        }
    }
}