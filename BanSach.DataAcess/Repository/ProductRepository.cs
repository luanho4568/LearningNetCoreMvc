using WebStoreBook.DataAcess.Repository.IRepository;
using WebStoreBook.Models;

namespace WebStoreBook.DataAcess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
            //_db.Products.Include(x => x.Category).Include(x => x.CoverType);
        }
        public void Update(Product product)
        {
            var objFromDb = _db.Products.SingleOrDefault(x => x.Id == product.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = product.Title;
                objFromDb.ISBN = product.ISBN;
                objFromDb.Description = product.Description;
                objFromDb.ListPrice = product.ListPrice;
                objFromDb.Price50 = product.Price50;
                objFromDb.Price100 = product.Price100;
                objFromDb.CategoryId = product.CategoryId;
                objFromDb.Author = product.Author;
                objFromDb.CoverTypeId = product.CoverTypeId;
                if (product.ImageUrl != null)
                {
                    objFromDb.ImageUrl = product.ImageUrl;
                }
            }
            _db.Products.Update(product);
        }
    }
}
