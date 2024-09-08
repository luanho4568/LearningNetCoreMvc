using WebStoreBook.DataAcess.Repository.IRepository;
using WebStoreBook.Models;

namespace WebStoreBook.DataAcess.Repository
{
    internal class CoverTypeRepository : Repository<CoverType>, ICoverTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public CoverTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(CoverType coverType)
        {
            _db.CoverTypes.Update(coverType);
        }
    }
}
