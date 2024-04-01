using Microsoft.EntityFrameworkCore.Storage;
using SuperMarket.Application.Services;
using SuperMarket.Infrastructure.Context;

namespace SuperMarket.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private IDbContextTransaction _currentTransaction = null;
        private bool disposed;

        public IMarketRepository Markets { get; }

        public IProductRepository Products { get; }

        public IOrderRepository Orders { get; }

        public bool HasActiveTransaction => _currentTransaction is not null;

        public UnitOfWork(ApplicationDbContext dbContext, 
            IMarketRepository markets, 
            IProductRepository products, 
            IOrderRepository orders)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

            Markets = markets;
            Products = products;
            Orders = orders;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }

            disposed = true;
        }


        public async Task<int> Commit()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> Commit(CancellationToken cancellationToken)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken);
        }


        public async Task BeginTransactionAsync(CancellationToken cancellationToken)
        {
            if (_currentTransaction is not null)
            {
                throw new InvalidOperationException("A transaction is already in progress.");
            }

            _currentTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();

                _currentTransaction?.Commit();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_currentTransaction is not null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_currentTransaction is null)
            {
                throw new InvalidOperationException("A transaction must be in progress to execute rollback.");
            }

            try
            {
                await _currentTransaction.RollbackAsync();
            }
            finally
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
    }
}