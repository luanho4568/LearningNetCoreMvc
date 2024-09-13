using WebStoreBook.DataAcess.Repository.IRepository;
using WebStoreBook.Models;

namespace WebStoreBook.DataAcess.Repository
{
    public class OrderDetailRepository : Repository<OrderDetails>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderDetailRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderDetails orderDetails)
        {
            _db.OrderDetails.Update(orderDetails);
        }
    }
}
