using Interfaces;
using Models;

namespace Strategies
{
    public class LocalStrategy : IShippingStrategy
    {
        public string CarrierName => "Transportadora Local";

        public decimal Calculate(ShippingInfo info)
            => 8.00m + info.Weight * 1.50m;

        public int GetDeliveryTime(ShippingInfo info) => 1;

        public bool IsAvailable(ShippingInfo info)
            => info.Destination.Contains("São Paulo-SP");
    }
}