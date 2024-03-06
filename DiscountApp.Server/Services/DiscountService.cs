using DiscountApp.Server.Controller;
using DiscountApp.Servrer.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DiscountApp.Server.Services;

public class DiscountService : DiscountApp.DiscountAppBase
{
    private readonly ILogger<DiscountService> _logger;
    private readonly DiscountCodeGenertor codeGenerator;
    private readonly DiscountCodeController discountCodeController;
    private readonly DiscountCodeContext context;

    public DiscountService(ILogger<DiscountService> logger)
    {
        _logger = logger;
        codeGenerator = new DiscountCodeGenertor();
        context = new DiscountCodeContext();
        discountCodeController = new DiscountCodeController(context);
    }

    public override async Task<GenerateReply> Generate(GenerateRequest request, ServerCallContext context)
    {
        var codes = codeGenerator.Generate(request.Count, request.Length);
        var existingCode = 0;
        foreach (var code in codes)
        {
            _logger.LogInformation(code.Code);
            if (discountCodeController.DiscountCodeExists(code)) existingCode++;
            else await discountCodeController.SaveDiscountCode(code).ConfigureAwait(false);
        }
        return await Task.FromResult(new GenerateReply
        {
            Result = existingCode == 0
        });
    }
    public override async Task<UseCodeReply> UseCode(UseCodeRequest request, ServerCallContext context)
    {
        if (!discountCodeController.DiscountCodeExists(new DiscountCode(request.Code, false)))
            return await Task.FromResult(new UseCodeReply
            {
                Result = 1
            });
        var discountCode = await discountCodeController.GetDiscountCode(new DiscountCode(request.Code, false)).ConfigureAwait(false);
        if (discountCode.IsUsed)
            return await Task.FromResult(new UseCodeReply
            {
                Result = 2
            });
        discountCode.IsUsed = true;
        await discountCodeController.UseDiscountCode(discountCode).ConfigureAwait(false);
        return await Task.FromResult(new UseCodeReply
            {
                Result = 0
            });
    }
}
