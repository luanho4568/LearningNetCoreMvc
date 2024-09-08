using WebStoreBook.Models;

namespace WebStoreBook.DataAcess.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        void Update(Category category);
    }
}
