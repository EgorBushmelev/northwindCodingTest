namespace Bit.CodingTest.Dtos
{
    public class OrderDetailDto
    {
        public int OrderId { get; set; }
        public SelectItem Product { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }
    }
}
