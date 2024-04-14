namespace SuperMarket.Application.Services.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        public IMarketRepository Markets { get; }

        public IProductRepository Products { get; }

        public IOrderRepository Orders { get; }


        bool HasActiveTransaction { get; }


        Task BeginTransactionAsync(CancellationToken cancellationToken);

        Task CommitTransactionAsync();

        Task RollbackTransactionAsync();


        Task<int> Commit(CancellationToken cancellationToken);

        Task<int> Commit();
    }
}