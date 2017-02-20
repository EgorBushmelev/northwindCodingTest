using System.Collections.Generic;
using Bit.CodingTest.Dtos;

namespace Bit.CodingTest.ViewModels
{
    public class OrderEditViewModel
    {
        public OrderDto Order { get; set; }
        public IEnumerable<string> Customers { get; set; }
        public IEnumerable<SelectItem> Employees { get; set; }
        public IEnumerable<SelectItem> ShipVia { get; set; }
    }
}
