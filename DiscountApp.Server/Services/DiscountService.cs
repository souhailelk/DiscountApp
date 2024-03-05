using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DiscountApp.Server.Services;

public class DiscountService : DiscountApp.DiscountAppBase
{
    private readonly ILogger<DiscountService> _logger;
    public DiscountService(ILogger<DiscountService> logger)
    {
        _logger = logger;
    }

    public override Task<GenerateReply> Generate(GenerateRequest request, ServerCallContext context)
    {
        return Task.FromResult(new GenerateReply
        {
            Result = true
        });
    }
        public override Task<UseCodeReply> UseCode(UseCodeRequest request, ServerCallContext context)
    {
        Console.WriteLine($"The code is {request.Code}");
        return Task.FromResult(new UseCodeReply
        {
            Result = 1
        });
    }
}
