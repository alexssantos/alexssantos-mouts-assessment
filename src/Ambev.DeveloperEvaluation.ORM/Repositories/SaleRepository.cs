using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    /// <summary>
    /// Implementation of ISaleRepository using Entity Framework Core
    /// </summary>
    public class SaleRepository : Repository<Sale>, ISaleRepository
    {
        /// <summary>
        /// Initializes a new instance of SaleRepository
        /// </summary>
        /// <param name="context">The database context</param>
        public SaleRepository(DefaultContext context) : base(context)
        {
        }

        /// <summary>
        /// Retrieves a sale by its sale number
        /// </summary>
        /// <param name="saleNumber">The sale number to search for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The sale if found, null otherwise</returns>
        public async Task<Sale?> GetBySaleNumberAsync(int saleNumber, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Sale>()
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.SaleNumber == saleNumber, cancellationToken);
        }

        /// <summary>
        /// Retrieves all sales for a specific customer
        /// </summary>
        /// <param name="customerId">The customer identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A collection of sales for the customer</returns>
        public async Task<IEnumerable<Sale>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Sale>()
                .Include(s => s.Items)
                .Where(s => s.CustomerId == customerId)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves all sales for a specific branch
        /// </summary>
        /// <param name="branchId">The branch identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A collection of sales for the branch</returns>
        public async Task<IEnumerable<Sale>> GetByBranchIdAsync(string branchId, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Sale>()
                .Include(s => s.Items)
                .Where(s => s.BranchId == branchId)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves all sales within a specified date range
        /// </summary>
        /// <param name="startDate">The start date of the range</param>
        /// <param name="endDate">The end date of the range</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A collection of sales within the date range</returns>
        public async Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Sale>()
                .Include(s => s.Items)
                .Where(s => s.SaleDate >= startDate && s.SaleDate <= endDate)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Gets the next available sale number
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The next available sale number</returns>
        public async Task<int> GetNextSaleNumberAsync(CancellationToken cancellationToken = default)
        {
            var lastSale = await _context.Set<Sale>()
                .OrderByDescending(s => s.SaleNumber)
                .FirstOrDefaultAsync(cancellationToken);

            return (lastSale?.SaleNumber ?? 0) + 1;
        }

        /// <summary>
        /// Retrieves a sale by its unique identifier, including its items
        /// </summary>
        /// <param name="id">The unique identifier of the sale</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The sale with its items</returns>
        /// <exception cref="InvalidOperationException">Thrown when the sale is not found</exception>
        public override async Task<Sale> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var sale = await _context.Set<Sale>()
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
                
            if (sale == null)
                throw new InvalidOperationException($"Sale with id {id} was not found");
                
            return sale;
        }

        /// <summary>
        /// Retrieves all sales including their items
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A collection of all sales with their items</returns>
        public override async Task<IEnumerable<Sale>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<Sale>()
                .Include(s => s.Items)
                .ToListAsync(cancellationToken);
        }
    }
}