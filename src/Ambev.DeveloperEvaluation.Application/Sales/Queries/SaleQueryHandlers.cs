using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.Queries
{
    public class SaleQueryHandlers :
        IRequestHandler<GetSaleByNumberQuery, SaleDto>,
        IRequestHandler<GetSalesByCustomerQuery, IEnumerable<SaleDto>>,
        IRequestHandler<GetSalesByBranchQuery, IEnumerable<SaleDto>>,
        IRequestHandler<GetSalesByDateRangeQuery, IEnumerable<SaleDto>>,
        IRequestHandler<GetAllSalesQuery, IEnumerable<SaleDto>>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public SaleQueryHandlers(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<SaleDto> Handle(GetSaleByNumberQuery request, CancellationToken cancellationToken)
        {
            var sale = await _saleRepository.GetBySaleNumberAsync(request.SaleNumber, cancellationToken);
            return _mapper.Map<SaleDto>(sale);
        }

        public async Task<IEnumerable<SaleDto>> Handle(GetSalesByCustomerQuery request, CancellationToken cancellationToken)
        {
            var sales = await _saleRepository.GetByCustomerIdAsync(request.CustomerId, cancellationToken);
            return _mapper.Map<IEnumerable<SaleDto>>(sales);
        }

        public async Task<IEnumerable<SaleDto>> Handle(GetSalesByBranchQuery request, CancellationToken cancellationToken)
        {
            var sales = await _saleRepository.GetByBranchIdAsync(request.BranchId, cancellationToken);
            return _mapper.Map<IEnumerable<SaleDto>>(sales);
        }

        public async Task<IEnumerable<SaleDto>> Handle(GetSalesByDateRangeQuery request, CancellationToken cancellationToken)
        {
            var sales = await _saleRepository.GetByDateRangeAsync(request.StartDate, request.EndDate, cancellationToken);
            return _mapper.Map<IEnumerable<SaleDto>>(sales);
        }

        public async Task<IEnumerable<SaleDto>> Handle(GetAllSalesQuery request, CancellationToken cancellationToken)
        {
            var sales = await _saleRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<SaleDto>>(sales);
        }
    }
}