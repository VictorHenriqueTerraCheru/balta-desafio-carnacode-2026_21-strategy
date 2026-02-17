using Interfaces;
using Models;
using Strategies;

public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== Strategy Pattern - Calculo de Frete ===\n");

        var calculator = new ShippingCalculator();

        var shipping1 = new ShippingInfo
        {
            Origin = "Sao Paulo-SP",
            Destination = "Rio de Janeiro-RJ",
            Weight = 5.0m,
            IsExpress = false
        };

        var shipping2 = new ShippingInfo
        {
            Origin = "Sao Paulo-SP",
            Destination = "Manaus-AM",
            Weight = 8.0m,
            IsExpress = true
        };

        Console.WriteLine("Usando Correios\n");
        calculator.SetStrategy(new CorreiosStrategy());
        calculator.Calculate(shipping1);

        Console.WriteLine("\nTrocando para FedEx em runtime\n");
        calculator.SetStrategy(new FedExStrategy());
        calculator.Calculate(shipping2);

        Console.WriteLine("\nTrocando para DHL em runtime\n");
        calculator.SetStrategy(new DHLStrategy());
        calculator.Calculate(shipping1);

        Console.WriteLine("\nLocal (fora da area)\n");
        calculator.SetStrategy(new LocalStrategy());
        calculator.Calculate(shipping2); // Manaus nao atende!

        Console.WriteLine("\nLocal (dentro da area)\n");
        calculator.Calculate(shipping1); // SP atende!

        // Comparar todas de uma vez!
        var allStrategies = new List<IShippingStrategy>
        {
            new CorreiosStrategy(),
            new FedExStrategy(),
            new DHLStrategy(),
            new LocalStrategy()
        };

        calculator.CompareAll(shipping1, allStrategies);
        calculator.CompareAll(shipping2, allStrategies);

        Console.WriteLine("\nBeneficios\n");
        Console.WriteLine("ANTES: 3 switches com logica de 4 transportadoras");
        Console.WriteLine("DEPOIS: 4 classes independentes, ShippingCalculator sem switch!");
        Console.WriteLine("Nova transportadora UPS? So criar UPSStrategy!");
        Console.WriteLine("Testar Correios? So instanciar CorreiosStrategy isolado!");
    }
}