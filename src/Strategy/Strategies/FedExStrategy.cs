using Interfaces;
using Models;

namespace Strategies
{
    public class FedExStrategy : IShippingStrategy
    {
        public string CarrierName => "FedEx";

        public decimal Calculate(ShippingInfo info)
        {
            var cost = 30.00m + info.Weight * 5.00m;

            if (info.IsExpress)
                cost *= 1.8m;

            if (info.Destination.Contains("Norte") || info.Destination.Contains("Nordeste"))
                cost += 20.00m;

            return cost;
        }

        public int GetDeliveryTime(ShippingInfo info)
            => info.IsExpress ? 2 : 5;

        public bool IsAvailable(ShippingInfo info)
            => info.Weight <= 50;
    }
}