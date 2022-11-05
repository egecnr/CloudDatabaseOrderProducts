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
        Task CreateOrder(Order order);
        Task CheckoutAndShipOrder(Order o);
        Task<Order> ReturnFullOrderObjectById(Guid orderId);
        Task<bool> CheckIfOrderExistAndNotShipped(Guid orderId);
        Task AddOrderToQueue(CreateOrderDTO orderDto, Guid userId);
        Task CreateProductInOrder(Guid orderId, Guid productId);
        Task DeleteProductInOrder(Guid orderId, Guid productId);
        Task<bool> CheckIfOrderExist(Guid orderId);
        Task<GetOrderDTO> GetOrderByOrderId(Guid orderId);
        Task<IEnumerable<GetOrderDTO>> GetOrdersOfUser(Guid userId);
    };
}
