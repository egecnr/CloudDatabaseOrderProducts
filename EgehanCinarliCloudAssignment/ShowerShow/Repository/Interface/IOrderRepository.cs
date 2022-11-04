using UserAndOrdersFunction.DTO;
using UserAndOrdersFunction.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAndOrdersFunction.Repository.Interface
{
    public interface IOrderRepository
    {
        public Task CreateOrder(CreateOrderDTO orderDTO, Guid userId);
        public Task AddProductToOrder(Guid orderId, Guid productId);
        public Task RemoveProductFromOrder(Guid orderId, Guid productId);
        public Task ShipOrder(Guid orderId);
        public Task<Order> GetOrderById(Guid orderId);
        public Task<IEnumerable<Order>> GetOrderByUser(Guid userId);
    };
}
