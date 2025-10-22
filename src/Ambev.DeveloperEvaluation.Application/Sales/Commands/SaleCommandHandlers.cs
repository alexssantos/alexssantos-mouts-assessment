using System;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Commands
{
    public class SaleCommandHandlers : 
        IRequestHandler<CreateSaleCommand, SaleResult>,
        IRequestHandler<UpdateSaleCommand>,
        IRequestHandler<CancelSaleCommand>,
        IRequestHandler<CancelSaleItemCommand>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public SaleCommandHandlers(ISaleRepository saleRepository, IMapper mapper)
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

        public async Task Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetBySaleNumberAsync(request.SaleNumber, cancellationToken);
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

            await _saleRepository.UpdateAsync(sale, cancellationToken);
        }

        public async Task Handle(CancelSaleCommand request, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetBySaleNumberAsync(request.SaleNumber, cancellationToken);
            if (sale == null)
                throw new ApplicationException($"Sale {request.SaleNumber} not found");

            sale.Cancel();
            await _saleRepository.UpdateAsync(sale, cancellationToken);
        }

        public async Task Handle(CancelSaleItemCommand request, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetBySaleNumberAsync(request.SaleNumber, cancellationToken);
            if (sale == null)
                throw new ApplicationException($"Sale {request.SaleNumber} not found");

            sale.CancelItem(request.ProductId);
            await _saleRepository.UpdateAsync(sale, cancellationToken);
        }
    }
}