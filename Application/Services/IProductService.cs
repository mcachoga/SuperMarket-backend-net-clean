using SuperMarket.Domain;

namespace SuperMarket.Application.Services
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(Product product);

        Task<Product> UpdateProductAsync(Product product);

        Task<int> DeleteProductAsync(Product product);

        Task<Product> GetProductByIdAsync(int id);

        Task<List<Product>> GetProductListAsync();
    }
}