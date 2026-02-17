using Interfaces;
using Models;

public class ShippingCalculator
{
    private IShippingStrategy _strategy;

    public void SetStrategy(IShippingStrategy strategy)
        => _strategy = strategy;

    public void Calculate(ShippingInfo info)
    {
        if (!_strategy.IsAvailable(info))
        {
            Console.WriteLine($"[{_strategy.CarrierName}] Nao atende esta regiao!");
            return;
        }

        var cost = _strategy.Calculate(info);
        var days = _strategy.GetDeliveryTime(info);

        Console.WriteLine($"[{_strategy.CarrierName}] R$ {cost:N2} | Prazo: {days} dias uteis");
    }

    public void CompareAll(ShippingInfo info, IEnumerable<IShippingStrategy> strategies)
    {
        Console.WriteLine("\n=== Comparando Transportadoras ===");
        Console.WriteLine($"Origem: {info.Origin} | Destino: {info.Destination}");
        Console.WriteLine($"Peso: {info.Weight}kg | Expresso: {(info.IsExpress ? "Sim" : "Nao")}\n");

        foreach (var strategy in strategies)
        {
            if (!strategy.IsAvailable(info))
            {
                Console.WriteLine($"[{strategy.CarrierName}] Nao disponivel para este envio");
                continue;
            }

            var cost = strategy.Calculate(info);
            var days = strategy.GetDeliveryTime(info);
            Console.WriteLine($"[{strategy.CarrierName}] R$ {cost:N2} | {days} dias uteis");
        }
    }
}