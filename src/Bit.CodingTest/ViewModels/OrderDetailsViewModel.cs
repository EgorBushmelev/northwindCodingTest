using System.Collections.Generic;
using Bit.CodingTest.Dtos;

namespace Bit.CodingTest.ViewModels
{
    public class OrderDetailsViewModel
    {
        public IEnumerable<SelectItem> Products { get; set; }
        public OrderDto Order { get; set; }
    }
}
