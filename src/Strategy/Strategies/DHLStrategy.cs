using Interfaces;
using Models;

namespace Strategies
{
    public class DHLStrategy : IShippingStrategy
    {
        public string CarrierName => "DHL";

        public decimal Calculate(ShippingInfo info)
        {
            var cost = 25.00m + info.Weight * 4.50m;

            if (info.Weight > 10)
                cost += (info.Weight - 10) * 2.00m;

            if (info.IsExpress)
                cost += 35.00m;

            return cost;
        }

        public int GetDeliveryTime(ShippingInfo info)
            => info.IsExpress ? 1 : 4;

        public bool IsAvailable(ShippingInfo info)
            => info.Weight <= 50;
    }
}