using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    /// <summary>
    /// Base repository implementation using Entity Framework Core
    /// </summary>
    /// <typeparam name="T">The entity type that inherits from BaseEntity</typeparam>
    public abstract class Repository<T> : IRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// The database context
        /// </summary>
        protected readonly DefaultContext _context;

        /// <summary>
        /// Initializes a new instance of Repository
        /// </summary>
        /// <param name="context">The database context</param>
        protected Repository(DefaultContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves an entity by its unique identifier
        /// </summary>
        /// <param name="id">The unique identifier of the entity</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The entity if found</returns>
        /// <exception cref="InvalidOperationException">Thrown when the entity is not found</exception>
        public virtual async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Set<T>().FindAsync(new object[] { id }, cancellationToken);
            if (entity == null)
                throw new InvalidOperationException($"Entity of type {typeof(T).Name} with id {id} was not found");
            return entity;
        }

        /// <summary>
        /// Retrieves all entities of type T
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A collection of all entities</returns>
        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Adds a new entity to the database
        /// </summary>
        /// <param name="entity">The entity to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The added entity</returns>
        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>().AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        /// <summary>
        /// Updates an existing entity in the database
        /// </summary>
        /// <param name="entity">The entity to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            entity.UpdateLastModified();
            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Deletes an entity from the database
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}