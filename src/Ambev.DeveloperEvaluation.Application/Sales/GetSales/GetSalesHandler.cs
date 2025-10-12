using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales
{
    public class GetSalesHandler :
        IRequestHandler<GetAllSalesQuery, IEnumerable<SaleResult>>,
        IRequestHandler<GetSaleByNumberQuery, SaleResult>,
        IRequestHandler<GetSalesByCustomerQuery, IEnumerable<SaleResult>>,
        IRequestHandler<GetSalesByBranchQuery, IEnumerable<SaleResult>>,
        IRequestHandler<GetSalesByDateRangeQuery, IEnumerable<SaleResult>>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public GetSalesHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SaleResult>> Handle(GetAllSalesQuery request, CancellationToken cancellationToken)
        {
            var sales = await _saleRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<SaleResult>>(sales);
        }

        public async Task<SaleResult> Handle(GetSaleByNumberQuery request, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetBySaleNumberAsync(request.SaleNumber, cancellationToken);
            return _mapper.Map<SaleResult>(sale);
        }

        public async Task<IEnumerable<SaleResult>> Handle(GetSalesByCustomerQuery request, CancellationToken cancellationToken)
        {
            var sales = await _saleRepository.GetByCustomerIdAsync(request.CustomerId, cancellationToken);
            return _mapper.Map<IEnumerable<SaleResult>>(sales);
        }

        public async Task<IEnumerable<SaleResult>> Handle(GetSalesByBranchQuery request, CancellationToken cancellationToken)
        {
            var sales = await _saleRepository.GetByBranchIdAsync(request.BranchId, cancellationToken);
            return _mapper.Map<IEnumerable<SaleResult>>(sales);
        }

        public async Task<IEnumerable<SaleResult>> Handle(GetSalesByDateRangeQuery request, CancellationToken cancellationToken)
        {
            var sales = await _saleRepository.GetByDateRangeAsync(request.StartDate, request.EndDate, cancellationToken);
            return _mapper.Map<IEnumerable<SaleResult>>(sales);
        }
    }
}