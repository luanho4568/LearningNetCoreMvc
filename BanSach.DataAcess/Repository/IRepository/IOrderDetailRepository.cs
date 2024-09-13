using WebStoreBook.Models;

namespace WebStoreBook.DataAcess.Repository.IRepository
{
    public interface IOrderDetailRepository : IRepository<OrderDetails>
    {
        void Update(OrderDetails orderDetails);
    }
}
