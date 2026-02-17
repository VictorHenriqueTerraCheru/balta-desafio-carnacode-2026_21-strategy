namespace Models
{
    public class ShippingInfo
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public decimal Weight { get; set; }
        public bool IsExpress { get; set; }
    }
}
