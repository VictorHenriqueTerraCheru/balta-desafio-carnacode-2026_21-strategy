using Models;

namespace Interfaces
{
    public interface IShippingStrategy
    {
        string CarrierName { get; }
        decimal Calculate(ShippingInfo info);
        int GetDeliveryTime(ShippingInfo info);
        bool IsAvailable(ShippingInfo info);
    }
}