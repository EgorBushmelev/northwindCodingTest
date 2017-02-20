using Bit.CodingTest.Dtos;
using Microsoft.AspNetCore.Mvc;
using Bit.CodingTest.Interfaces;
using Bit.CodingTest.ViewModels;

namespace Bit.CodingTest.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        public IActionResult Index()
        {
            var orders = _ordersService.GetOrderList();

            return View(orders);
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = _ordersService.GetOrderDto(id.Value);

            return View(new OrderDetailsViewModel
            {
                Order = order,
                Products = _ordersService.GetProducts()
            });
        }

        public IActionResult Create()
        {
            return View("Edit", new OrderEditViewModel
            {
                Customers = _ordersService.GetCustomers(),
                Employees = _ordersService.GetEmployees(),
                ShipVia = _ordersService.GetShipViaItems()
            });
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var orderDto = _ordersService.GetOrderDto(id.Value);
            return View(new OrderEditViewModel
            {
                Customers = _ordersService.GetCustomers(),
                Employees = _ordersService.GetEmployees(),
                Order = orderDto,
                ShipVia = _ordersService.GetShipViaItems()
            });
        }

        [HttpPost]
        public JsonResult Edit([FromBody] OrderDto orderDto)
        {
            var orderId = _ordersService.EditOrder(orderDto);

            return new JsonResult(new { Success = true, OrderId = orderId });
        }

        [HttpPost]
        public JsonResult Delete([FromBody] int id)
        {
            _ordersService.DeleteOrder(id);

            return new JsonResult(new { Success = true });
        }
    }
}
