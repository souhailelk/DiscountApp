using System.Threading.Tasks;
using System;
using Grpc.Net.Client;

namespace DiscountApp.ConsoleClient;
public class Program
{
    public static async Task Main(string[] argv)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:5185");
        var client = new DiscountApp.DiscountAppClient(channel);
        var reply = await client.GenerateAsync(
                          new GenerateRequest { Count = 1, Length = 2 });
        Console.WriteLine("Greeting: " + reply.Result);
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}