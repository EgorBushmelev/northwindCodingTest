using Bit.CodingTest.Dtos;
using Bit.CodingTest.Interfaces;
using Microsoft.AspNetCore.Mvc;
namespace Bit.CodingTest.Controllers
{
    public class OrderDetailsController : Controller
    {
        private readonly IOrdersService _ordersService;

        public OrderDetailsController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        [HttpPost]
        public JsonResult Edit([FromBody] OrderDetailDto orderDetail)
        {
            if (orderDetail == null)
            {
                return new JsonResult(new {Success = false});
            }

            _ordersService.EditOrderDetail(orderDetail);

            return new JsonResult(new { Success = true });
        }

        [HttpPost]
        public JsonResult Delete([FromBody] OrderDetailDto orderDetail)
        {
            if (orderDetail == null)
            {
                return new JsonResult(new { Success = false });
            }

            _ordersService.DeleteOrderDetail(orderDetail.OrderId, orderDetail.Product.Id);

            return new JsonResult(new { Success = true });
        }
    }
}