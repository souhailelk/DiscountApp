using DiscountApp.Server.Controller;
using DiscountApp.Servrer.Models;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace DiscountApp.Server.Services;

public class DiscountService : DiscountApp.DiscountAppBase
{
    private readonly ILogger<DiscountService> logger;
    private readonly IDiscountCodeGeneretor codeGenerator;
    private readonly IDiscountCodeController discountCodeController;
    public DiscountService(ILogger<DiscountService> logger)
    {
        this.logger = logger;
        codeGenerator = new DiscountCodeGenertor();
        discountCodeController = new DiscountCodeController(new DiscountCodeContext());
    }
    public DiscountService(ILogger<DiscountService> logger, IDiscountCodeGeneretor codeGenerator, IDiscountCodeController discountCodeController)
    {
        this.logger = logger;
        this.codeGenerator = codeGenerator;
        this.discountCodeController = discountCodeController;
    }

    public override async Task<GenerateReply> Generate(GenerateRequest request, ServerCallContext context)
    {
        logger.LogInformation($"new Generate request count={request.Count}, length={request.Length} ");
        var codes = codeGenerator.Generate(request.Count, request.Length);
        var existingCode = 0;
        foreach (var code in codes)
        {
            if (!await discountCodeController.SaveDiscountCodeIfItDontExist(code).ConfigureAwait(false)) existingCode++;
        }
        return await Task.FromResult(new GenerateReply
        {
            Result = existingCode == 0
        });
    }
    public override async Task<UseCodeReply> UseCode(UseCodeRequest request, ServerCallContext context)
    {
        logger.LogInformation($"new UseCode request code={request.Code} ");
        bool discountCodeExists = await discountCodeController.DiscountCodeExists(new DiscountCode(request.Code, false)).ConfigureAwait(false);
        if (!discountCodeExists)
            return await Task.FromResult(new UseCodeReply
            {
                Result = UseCodeResponse.Doesnotexist
            });
        logger.LogInformation($"new UseCode request code={request.Code}, the code  exists!");
        var discountCode = await discountCodeController.GetDiscountCode(new DiscountCode(request.Code, false)).ConfigureAwait(false);
        if (discountCode.IsUsed)
            return await Task.FromResult(new UseCodeReply
            {
                Result = UseCodeResponse.Used
            });

        logger.LogInformation($"new UseCode request code={request.Code}, the code isn't used!");
        discountCode.IsUsed = true;
        await discountCodeController.UseDiscountCode(discountCode).ConfigureAwait(false);
        return await Task.FromResult(new UseCodeReply
        {
            Result = UseCodeResponse.Success
        });
    }
    public override async Task<GetUnusedCodesReply> GetUnusedCodes(GetUnusedCodesRequest request, ServerCallContext context)
    {
        logger.LogInformation($"Get unused codes count={request.Count} ");
        var codes = (await discountCodeController.GetUnusedDiscountCodes().ConfigureAwait(false)).ToList();
        if (request.Count > codes.Count)
            throw new RpcException(new Status(StatusCode.Unavailable, "There is no enough unused code, try generate new codes!"));
        var reply = new GetUnusedCodesReply();
        reply.Codes.AddRange(codes.Select( c => c.Code));
        return await Task.FromResult(reply);
    }
}
