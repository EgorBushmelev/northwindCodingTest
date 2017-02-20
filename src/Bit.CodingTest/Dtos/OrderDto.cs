using System;
using System.Collections.Generic;

namespace Bit.CodingTest.Dtos
{
    public class OrderDto
    {
        public int? OrderId { get; set; }
        public string Customer { get; set; }
        public SelectItem Employee { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public SelectItem ShipVia { get; set; }
        public decimal? Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }

        public IEnumerable<OrderDetailDto> OrderDetails { get; set; }
    }
}
