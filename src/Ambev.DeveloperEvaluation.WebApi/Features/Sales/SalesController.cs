using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CancelSale;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SalesController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SalesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all sales
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A collection of sales</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseWithData<IEnumerable<GetSaleResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllSalesQuery(), cancellationToken);
            var response = _mapper.Map<IEnumerable<GetSaleResponse>>(result);
            return Ok(new ApiResponseWithData<IEnumerable<GetSaleResponse>> 
            { 
                Success = true,
                Data = response 
            });
        }

        /// <summary>
        /// Retrieves a sale by its sale number
        /// </summary>
        /// <param name="request">The get sale request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The sale if found</returns>
        [HttpGet("{saleNumber}")]
        [ProducesResponseType(typeof(ApiResponseWithData<GetSaleResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBySaleNumber([FromRoute] GetSaleRequest request, CancellationToken cancellationToken)
        {
            var validator = new GetSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(new ApiResponse 
                { 
                    Success = false,
                    Message = "Validation failed",
                    Errors = validationResult.Errors.Select(e => new ValidationErrorDetail 
                    { 
                        Error = e.PropertyName,
                        Detail = e.ErrorMessage 
                    })
                });

            var query = _mapper.Map<GetSaleByNumberQuery>(request);
            var result = await _mediator.Send(query, cancellationToken);

            if (result == null)
                return NotFound(new ApiResponse 
                { 
                    Success = false,
                    Message = "Sale not found",
                    Errors = new[] { new ValidationErrorDetail 
                    { 
                        Error = "NotFound",
                        Detail = "Sale not found" 
                    }}
                });

            var response = _mapper.Map<GetSaleResponse>(result);
            return Ok(new ApiResponseWithData<GetSaleResponse> 
            { 
                Success = true,
                Data = response 
            });
        }

        /// <summary>
        /// Retrieves all sales for a customer
        /// </summary>
        /// <param name="customerId">The customer identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A collection of sales for the customer</returns>
        [HttpGet("customer/{customerId}")]
        [ProducesResponseType(typeof(ApiResponseWithData<IEnumerable<GetSaleResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByCustomer(string customerId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetSalesByCustomerQuery { CustomerId = customerId }, cancellationToken);
            var response = _mapper.Map<IEnumerable<GetSaleResponse>>(result);
            return Ok(new ApiResponseWithData<IEnumerable<GetSaleResponse>> 
            { 
                Success = true,
                Data = response 
            });
        }

        /// <summary>
        /// Retrieves all sales for a specific branch
        /// </summary>
        /// <param name="branchId">The branch identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A collection of sales for the branch</returns>
        [HttpGet("branch/{branchId}")]
        [ProducesResponseType(typeof(ApiResponseWithData<IEnumerable<GetSaleResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByBranch(string branchId, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetSalesByBranchQuery { BranchId = branchId }, cancellationToken);
            var response = _mapper.Map<IEnumerable<GetSaleResponse>>(result);
            return Ok(new ApiResponseWithData<IEnumerable<GetSaleResponse>>
            {
                Success = true,
                Data = response
            });
        }

        /// <summary>
        /// Retrieves all sales within a date range
        /// </summary>
        /// <param name="startDate">The start date</param>
        /// <param name="endDate">The end date</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A collection of sales within the date range</returns>
        [HttpGet("date-range")]
        [ProducesResponseType(typeof(ApiResponseWithData<IEnumerable<GetSaleResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetSalesByDateRangeQuery 
            { 
                StartDate = startDate,
                EndDate = endDate
            }, cancellationToken);
            var response = _mapper.Map<IEnumerable<GetSaleResponse>>(result);
            return Ok(new ApiResponseWithData<IEnumerable<GetSaleResponse>>
            {
                Success = true,
                Data = response
            });
        }

        /// <summary>
        /// Creates a new sale
        /// </summary>
        /// <param name="request">The sale creation request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created sale details</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseWithData<CreateSaleResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateSaleRequest request, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleRequestValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
                return BadRequest(new ApiResponse 
                { 
                    Success = false,
                    Message = "Validation failed",
                    Errors = validationResult.Errors.Select(e => new ValidationErrorDetail 
                    { 
                        Error = e.PropertyName,
                        Detail = e.ErrorMessage 
                    })
                });

            var command = _mapper.Map<CreateSaleCommand>(request);
            var result = await _mediator.Send(command, cancellationToken);
            var response = _mapper.Map<CreateSaleResponse>(result);

            return CreatedAtAction(
                nameof(GetBySaleNumber), 
                new { saleNumber = response.SaleNumber },
                new ApiResponseWithData<CreateSaleResponse> 
                { 
                    Success = true,
                    Data = response 
                });
        }

        [HttpPut("{saleNumber}")]
        public async Task<IActionResult> Update(int saleNumber, UpdateSaleCommand command)
        {
            if (saleNumber != command.SaleNumber)
                return BadRequest();

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("{saleNumber}/cancel")]
        public async Task<IActionResult> CancelSale(int saleNumber)
        {
            await _mediator.Send(new CancelSaleCommand { SaleNumber = saleNumber });
            return NoContent();
        }

        [HttpPost("{saleNumber}/items/{productId}/cancel")]
        public async Task<IActionResult> CancelSaleItem(int saleNumber, string productId)
        {
            await _mediator.Send(new CancelSaleItemCommand 
            { 
                SaleNumber = saleNumber,
                ProductId = productId
            });
            return NoContent();
        }
    }
}