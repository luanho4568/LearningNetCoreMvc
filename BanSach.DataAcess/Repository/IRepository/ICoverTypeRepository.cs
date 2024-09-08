using WebStoreBook.Models;

namespace WebStoreBook.DataAcess.Repository.IRepository
{
    public interface ICoverTypeRepository : IRepository<CoverType>
    {
        void Update(CoverType coverType);
    }
}
