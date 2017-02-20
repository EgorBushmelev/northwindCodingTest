using System.Collections.Generic;
using Bit.CodingTest.Dtos;

namespace Bit.CodingTest.Interfaces
{
    public interface IOrdersService
    {
        IEnumerable<SelectItem> GetProducts();
        OrderDto GetOrderDto(int orderId);
        void EditOrderDetail(OrderDetailDto orderDetailDto);
        void DeleteOrderDetail(int orderId, int productId);
        IEnumerable<string> GetCustomers();
        IEnumerable<SelectItem> GetEmployees();
        IEnumerable<SelectItem> GetShipViaItems();
        int EditOrder(OrderDto orderDto);
        IEnumerable<OrderDto> GetOrderList();
        void DeleteOrder(int orderId);
    }
}
