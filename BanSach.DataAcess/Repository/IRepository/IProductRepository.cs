using WebStoreBook.Models;

namespace WebStoreBook.DataAcess.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);
    }
}
