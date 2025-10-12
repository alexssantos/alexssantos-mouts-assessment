using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    /// <summary>
    /// Repository interface for Sale entity operations
    /// </summary>
    public interface ISaleRepository : IRepository<Sale>
    {
        /// <summary>
        /// Retrieves a sale by its unique sale number
        /// </summary>
        /// <param name="saleNumber">The unique sale number to search for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The sale if found, null otherwise</returns>
        Task<Sale?> GetBySaleNumberAsync(int saleNumber, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all sales for a specific customer
        /// </summary>
        /// <param name="customerId">The unique identifier of the customer</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A collection of sales for the customer</returns>
        Task<IEnumerable<Sale>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all sales for a specific branch
        /// </summary>
        /// <param name="branchId">The unique identifier of the branch</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A collection of sales for the branch</returns>
        Task<IEnumerable<Sale>> GetByBranchIdAsync(string branchId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves all sales within a specific date range
        /// </summary>
        /// <param name="startDate">The start date of the range</param>
        /// <param name="endDate">The end date of the range</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A collection of sales within the date range</returns>
        Task<IEnumerable<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the next available sale number to be used
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The next sale number</returns>
        Task<int> GetNextSaleNumberAsync(CancellationToken cancellationToken = default);
    }
}