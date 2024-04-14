using SuperMarket.Domain;

namespace SuperMarket.Application.Services.Contracts
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }

        Task<List<Product>> GetListAsync();

        Task<Product> GetByIdAsync(int productId);

        Task<Product> InsertAsync(Product product);

        Task<Product> UpdateAsync(Product product);

        Task<int> DeleteAsync(Product product);
    }
}