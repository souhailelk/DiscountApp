using System.Threading.Tasks;
using System;
using Grpc.Net.Client;
using CommandLine;

namespace DiscountApp.ConsoleClient;
[Verb("generate")]
class GenerateOptions
{
    [Option('c', "count", Required = true, HelpText = "Number of codes to generate.")]
    public int Count { get; set; }

    [Option('l', "length", Required = true, HelpText = "Length of each code.")]
    public int Length { get; set; }
}
[Verb("useCode")]
class UseCodeOptions
{
    [Option('c', "code", Required = true, HelpText = "Code to use.")]
    public required string Code { get; set; }
}
[Verb("getUnusedCodes")]
class GetUnusedCodesOptions
{
    [Option('c', "count", Required = true, HelpText = "Number of unused codes to retrieve.")]
    public int Count { get; set; }
}

public class Program
{
    public static void Main(string[] args) => _ = Parser.Default.ParseArguments<GenerateOptions, UseCodeOptions, GetUnusedCodesOptions>(args)
                .WithParsed<GenerateOptions>(
                     (opts) => Generate(opts).GetAwaiter().GetResult())
                .WithParsed<UseCodeOptions>(
                   (opts) =>  UseCode(opts).GetAwaiter().GetResult())
                .WithParsed<GetUnusedCodesOptions>(
                   (opts) =>  GetUnusedCodes(opts).GetAwaiter().GetResult());

    static async Task<int> Generate(GenerateOptions opts)
    {
        Console.WriteLine($"Generating {opts.Count} codes of length {opts.Length}.");
        using var channel = GrpcChannel.ForAddress("http://localhost:5185");
        var client = new DiscountApp.DiscountAppClient(channel);
        var reply = await client.GenerateAsync(new GenerateRequest { Count = opts.Count, Length = opts.Length }).ConfigureAwait(false);
        Console.WriteLine("reply is " + reply.Result);
        return 0;
    }

    static async Task<int> UseCode(UseCodeOptions opts)
    {
        Console.WriteLine($"Using code: {opts.Code}");
        using var channel = GrpcChannel.ForAddress("http://localhost:5185");
        var client = new DiscountApp.DiscountAppClient(channel);
        var reply = await client.UseCodeAsync(new UseCodeRequest { Code = opts.Code }).ConfigureAwait(false);
        Console.WriteLine("reply is " + reply.Result);
        return 0;
    }

    static async Task<int> GetUnusedCodes(GetUnusedCodesOptions opts)
    {
        Console.WriteLine($"Retrieving {opts.Count} unused codes.");
        using var channel = GrpcChannel.ForAddress("http://localhost:5185");
        var client = new DiscountApp.DiscountAppClient(channel);
        var reply = await client.GetUnusedCodesAsync(new GetUnusedCodesRequest { Count = opts.Count }).ConfigureAwait(false);
        Console.WriteLine("The unuesed codes are:");
        foreach (var code in reply.Codes) Console.WriteLine($"\t- {code}");
        return 0;
    }
}
