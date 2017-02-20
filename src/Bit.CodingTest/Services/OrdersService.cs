using System;
using System.Collections.Generic;
using System.Linq;
using Bit.CodingTest.Dtos;
using Bit.CodingTest.Entities;
using Bit.CodingTest.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Bit.CodingTest.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly NorthwindContext _context;

        public OrdersService(NorthwindContext context)
        {
            _context = context;
        }

        private IEnumerable<OrderDetailDto> GetOrderDetails(Orders order)
        {
            return order.OrderDetails.ToList().Select(x => new OrderDetailDto
            {
                Product = new SelectItem {
                    Id = x.ProductId,
                    Text = x.Product.ProductName
                },
                Discount = x.Discount,
                Quantity = x.Quantity,
                OrderId = x.OrderId,
                UnitPrice = x.UnitPrice
            });
        }

        public void EditOrderDetail(OrderDetailDto orderDetailDto)
        {
            var order = GetOrder(orderDetailDto.OrderId);
            var orderDetail = order.OrderDetails.SingleOrDefault(x => x.ProductId == orderDetailDto.Product.Id && x.OrderId == orderDetailDto.OrderId);
            var detailAlreadyExists = true;
            if (orderDetail == null)
            {
                detailAlreadyExists = false;
                var product = GetProduct(orderDetailDto.Product.Id);
                orderDetail = new OrderDetails
                {
                    Product = product
                };
            }

            orderDetail.Quantity = orderDetailDto.Quantity;
            orderDetail.Discount = orderDetailDto.Discount;
            orderDetail.UnitPrice = orderDetailDto.UnitPrice;

            if (!detailAlreadyExists)
            {
                order.OrderDetails.Add(orderDetail);
            }

            _context.SaveChanges();
        }

        #region EntityGetters

        //There are a lot of similar methods due to bad PK namings in db, it can be replaced with single method and "Id" fields implementing an interface "IEntityWithId"

        private Orders GetOrder(int orderId)
        {
            var order = _context.Orders
                .Include(x => x.Customer)
                .Include(x => x.Employee)
                .Include(x => x.ShipViaNavigation)
                .Include(x => x.OrderDetails)
                .ThenInclude(x => x.Product)
                .SingleOrDefault(x => x.OrderId == orderId);
            if (order == null)
            {
                throw new InvalidOperationException($"Order with id {orderId} not found");
            }

            return order;
        }

        private Products GetProduct(int productId)
        {
            var product = _context.Products
                .SingleOrDefault(x => x.ProductId == productId);
            if (product == null)
            {
                throw new InvalidOperationException($"Product with id {productId} not found");
            }

            return product;
        }

        private Customers GetCustomer(string customerId)
        {
            var customer = _context.Customers
                .SingleOrDefault(x => x.CustomerId == customerId);
            if (customer == null)
            {
                throw new InvalidOperationException($"Customer with id {customerId} not found");
            }

            return customer;
        }

        private Employees GetEmployee(int employeeId)
        {
            var employee = _context.Employees
                .SingleOrDefault(x => x.EmployeeId == employeeId);
            if (employee == null)
            {
                throw new InvalidOperationException($"Employe with id {employeeId} not found");
            }

            return employee;
        }

        private Shippers GetShipper(int shipperId)
        {
            var shipper = _context.Shippers
                .SingleOrDefault(x => x.ShipperId == shipperId);
            if (shipper == null)
            {
                throw new InvalidOperationException($"Shipper with id {shipperId} not found");
            }

            return shipper;
        }

        #endregion

        public int EditOrder(OrderDto orderDto)
        {
            Orders order;
            var isNewOrder = true;

            if (orderDto.OrderId.HasValue)
            {
                order = GetOrder(orderDto.OrderId.Value);
                isNewOrder = false;
            }
            else
            {
                order = new Orders();
            }

            order.Customer = orderDto.Customer != null ? GetCustomer(orderDto.Customer) : null;
            order.Employee = orderDto.Employee != null ? GetEmployee(orderDto.Employee.Id) : null;
            order.ShipViaNavigation = orderDto.ShipVia != null ? GetShipper(orderDto.ShipVia.Id) : null;
            order.Freight = orderDto.Freight;
            order.OrderDate = orderDto.OrderDate;
            order.RequiredDate = orderDto.RequiredDate;
            order.ShipAddress = orderDto.ShipAddress;
            order.ShipCity = orderDto.ShipCity;
            order.ShipCountry = orderDto.ShipCountry;
            order.ShippedDate = orderDto.ShippedDate;
            order.ShipName = orderDto.ShipName;
            order.ShipPostalCode = orderDto.ShipPostalCode;
            order.ShipRegion = orderDto.ShipRegion;

            if (isNewOrder)
            {
                _context.Orders.Add(order);
            }

            _context.SaveChanges();

            return order.OrderId;
        }

        private OrderDto GetOrderDto(Orders order)
        {
            return new OrderDto
            {
                Customer = order.CustomerId,
                Employee = order.EmployeeId.HasValue ? new SelectItem
                {
                    Id = order.EmployeeId.Value,
                    Text = GetFullName(order.Employee)
                } : null,
                Freight = order.Freight,
                OrderDate = order.OrderDate,
                OrderId = order.OrderId,
                OrderDetails = GetOrderDetails(order),
                RequiredDate = order.RequiredDate,
                ShipAddress = order.ShipAddress,
                ShipCity = order.ShipCity,
                ShipCountry = order.ShipCountry,
                ShippedDate = order.ShippedDate,
                ShipName = order.ShipName,
                ShipPostalCode = order.ShipPostalCode,
                ShipRegion = order.ShipRegion,
                ShipVia = order.ShipVia.HasValue ? new SelectItem
                {
                    Id = order.ShipVia.Value,
                    Text = order.ShipViaNavigation.CompanyName
                } : null
            };
        }

        public OrderDto GetOrderDto(int orderId)
        {
            var order = GetOrder(orderId);

            return GetOrderDto(order);
        }

        public IEnumerable<string> GetCustomers()
        {
            return _context.Customers.Select(x => x.CustomerId).ToList();
        }

        public IEnumerable<SelectItem> GetEmployees()
        {
            return _context.Employees.Select(x => new SelectItem
            {
                Id = x.EmployeeId,
                Text = GetFullName(x)
            });
        }

        public IEnumerable<SelectItem> GetShipViaItems()
        {
            return _context.Shippers.Select(x => new SelectItem
            {
                Id = x.ShipperId,
                Text = x.CompanyName
            });
        }

        public IEnumerable<SelectItem> GetProducts()
        {
            return _context.Products.Select(x => new SelectItem
            {
                Id = x.ProductId,
                Text = x.ProductName
            }).ToList();
        }

        public void DeleteOrderDetail(int orderId, int productId)
        {
            var orderDetail = _context.OrderDetails.Single(x => x.OrderId == orderId && x.ProductId == productId);
            _context.Remove(orderDetail);

            _context.SaveChanges();
        }

        public void DeleteOrder(int orderId)
        {
            var orders = _context.Orders.Single(m => m.OrderId == orderId);
            _context.Orders.Remove(orders);

            _context.SaveChanges();
        }

        public IEnumerable<OrderDto> GetOrderList()
        {
            return _context.Orders
                .Include(x => x.Employee)
                .Include(x => x.ShipViaNavigation)
                .Select(x => GetOrderDto(x))
                .ToList();
        }

        private static string GetFullName(Employees employee)
        {
            return $"{employee.FirstName} {employee.LastName}";
        }
    }
}
