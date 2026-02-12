// DESAFIO: Sistema de Cálculo de Frete com Múltiplas Transportadoras
// PROBLEMA: Um e-commerce precisa calcular frete usando diferentes transportadoras (Correios, 
// FedEx, DHL, Transportadora Local), cada uma com sua própria lógica de cálculo. O código atual
// usa condicionais para escolher o algoritmo, violando o Open/Closed Principle

using System;

namespace DesignPatternChallenge
{
    // Contexto: Sistema de e-commerce que calcula frete baseado em peso, destino e urgência
    // Cada transportadora tem regras próprias e precisa ser facilmente intercambiável
    
    public class ShippingInfo
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public decimal Weight { get; set; }
        public bool IsExpress { get; set; }
    }

    // Problema: Classe com múltiplos algoritmos misturados
    public class ShippingCalculator
    {
        public decimal CalculateShipping(ShippingInfo info, string carrier)
        {
            Console.WriteLine($"\n=== Calculando Frete ===");
            Console.WriteLine($"Transportadora: {carrier}");
            Console.WriteLine($"Origem: {info.Origin}");
            Console.WriteLine($"Destino: {info.Destination}");
            Console.WriteLine($"Peso: {info.Weight}kg");
            Console.WriteLine($"Expresso: {(info.IsExpress ? "Sim" : "Não")}");

            decimal cost = 0;

            // Problema: Switch gigante com lógica de cada algoritmo
            switch (carrier.ToLower())
            {
                case "correios":
                    // Lógica específica dos Correios
                    cost = 15.00m; // Taxa base
                    cost += info.Weight * 2.50m; // Por kg
                    
                    if (info.IsExpress)
                        cost += 25.00m; // Taxa SEDEX
                    
                    // Desconto para mesmo estado
                    if (info.Origin.Split('-')[1] == info.Destination.Split('-')[1])
                        cost *= 0.85m;
                    
                    Console.WriteLine($"→ Cálculo Correios: R$ {cost:N2}");
                    break;

                case "fedex":
                    // Lógica específica FedEx
                    cost = 30.00m; // Taxa base internacional
                    cost += info.Weight * 5.00m;
                    
                    if (info.IsExpress)
                        cost *= 1.8m; // 80% a mais para expresso
                    
                    // Taxa adicional para destinos remotos
                    if (info.Destination.Contains("Norte") || info.Destination.Contains("Nordeste"))
                        cost += 20.00m;
                    
                    Console.WriteLine($"→ Cálculo FedEx: R$ {cost:N2}");
                    break;

                case "dhl":
                    // Lógica específica DHL
                    cost = 25.00m;
                    cost += info.Weight * 4.50m;
                    
                    // DHL cobra por faixa de peso
                    if (info.Weight > 10)
                        cost += (info.Weight - 10) * 2.00m;
                    
                    if (info.IsExpress)
                        cost += 35.00m;
                    
                    Console.WriteLine($"→ Cálculo DHL: R$ {cost:N2}");
                    break;

                case "local":
                    // Lógica da transportadora local
                    cost = 8.00m;
                    cost += info.Weight * 1.50m;
                    
                    // Não cobra expresso (sempre é rápido)
                    if (info.IsExpress)
                        Console.WriteLine("   ℹ️ Transportadora local sempre entrega rápido");
                    
                    // Só atende região metropolitana
                    if (!info.Destination.Contains("São Paulo-SP"))
                    {
                        Console.WriteLine("   ❌ Não atende esta região!");
                        return 0;
                    }
                    
                    Console.WriteLine($"→ Cálculo Local: R$ {cost:N2}");
                    break;

                default:
                    Console.WriteLine($"❌ Transportadora '{carrier}' não suportada!");
                    return 0;
            }

            return cost;
        }

        // Problema: Método separado para obter prazo também usa switch
        public int GetDeliveryTime(ShippingInfo info, string carrier)
        {
            switch (carrier.ToLower())
            {
                case "correios":
                    return info.IsExpress ? 3 : 7;
                case "fedex":
                    return info.IsExpress ? 2 : 5;
                case "dhl":
                    return info.IsExpress ? 1 : 4;
                case "local":
                    return 1;
                default:
                    return 0;
            }
        }

        // Problema: Método para verificar disponibilidade também usa switch
        public bool IsAvailable(ShippingInfo info, string carrier)
        {
            switch (carrier.ToLower())
            {
                case "correios":
                    return true; // Atende todo Brasil
                case "fedex":
                case "dhl":
                    return info.Weight <= 50; // Limite internacional
                case "local":
                    return info.Destination.Contains("São Paulo-SP");
                default:
                    return false;
            }
        }

        // Problema: Adicionar nova transportadora = modificar TODOS esses métodos
        // Problema: Lógica de negócio complexa espalhada em switches
        // Problema: Difícil testar cada algoritmo isoladamente
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Sistema de Cálculo de Frete ===");

            var calculator = new ShippingCalculator();

            var shipping1 = new ShippingInfo
            {
                Origin = "São Paulo-SP",
                Destination = "Rio de Janeiro-RJ",
                Weight = 5.0m,
                IsExpress = false
            };

            var shipping2 = new ShippingInfo
            {
                Origin = "São Paulo-SP",
                Destination = "Manaus-AM",
                Weight = 8.0m,
                IsExpress = true
            };

            // Testando diferentes transportadoras
            var correiosCost = calculator.CalculateShipping(shipping1, "correios");
            var correiosTime = calculator.GetDeliveryTime(shipping1, "correios");
            Console.WriteLine($"Prazo: {correiosTime} dias úteis\n");

            var fedexCost = calculator.CalculateShipping(shipping2, "fedex");
            var fedexTime = calculator.GetDeliveryTime(shipping2, "fedex");
            Console.WriteLine($"Prazo: {fedexTime} dias úteis\n");

            var dhlCost = calculator.CalculateShipping(shipping1, "dhl");
            var dhlTime = calculator.GetDeliveryTime(shipping1, "dhl");
            Console.WriteLine($"Prazo: {dhlTime} dias úteis\n");

            var localCost = calculator.CalculateShipping(shipping1, "local");
            
            // Testando transportadora inexistente
            calculator.CalculateShipping(shipping1, "transportadora-nova");

            Console.WriteLine("\n=== Comparando Opções ===");
            var carriers = new[] { "correios", "fedex", "dhl", "local" };
            
            foreach (var carrier in carriers)
            {
                if (calculator.IsAvailable(shipping1, carrier))
                {
                    var cost = calculator.CalculateShipping(shipping1, carrier);
                    var time = calculator.GetDeliveryTime(shipping1, carrier);
                    // Saída já impressa no método
                }
            }

            Console.WriteLine("\n=== PROBLEMAS ===");
            Console.WriteLine("✗ Switch/case repetido em múltiplos métodos");
            Console.WriteLine("✗ Adicionar transportadora = modificar vários métodos");
            Console.WriteLine("✗ Algoritmos não são intercambiáveis em runtime facilmente");
            Console.WriteLine("✗ Difícil testar cada algoritmo isoladamente");
            Console.WriteLine("✗ Viola Open/Closed Principle");
            Console.WriteLine("✗ Lógica de negócio complexa misturada");
            Console.WriteLine("✗ Não é possível combinar ou decorar algoritmos");

            Console.WriteLine("\n=== Requisitos Não Atendidos ===");
            Console.WriteLine("• Trocar algoritmo em runtime sem recompilar");
            Console.WriteLine("• Adicionar nova transportadora sem modificar código existente");
            Console.WriteLine("• Testar cada algoritmo independentemente");
            Console.WriteLine("• Configurar transportadoras via arquivo/banco de dados");
            Console.WriteLine("• Cliente escolher melhor opção sem conhecer detalhes");
            Console.WriteLine("• Compor múltiplos cálculos (ex: frete + seguro)");

            // Perguntas para reflexão:
            // - Como encapsular cada algoritmo em sua própria classe?
            // - Como tornar algoritmos intercambiáveis?
            // - Como adicionar novos algoritmos sem modificar código existente?
            // - Como permitir que cliente escolha algoritmo em runtime?
        }
    }
}
