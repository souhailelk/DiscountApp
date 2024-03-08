using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscountApp.Server.Controller;
using DiscountApp.Server.Services;
using DiscountApp.Servrer.Models;
using Grpc.Core.Testing;
using Microsoft.Extensions.Logging;
using Moq;

namespace DiscountApp.Server.UnitTests;
public class DiscountServiceTest
{
    [Test]
    public async Task TestGenerate_Should_Return_False_When_A_Newly_generated_Code_Was_Previously_Generated()
    {
        //Prepare
        var dummyServercallcontext = TestServerCallContext.Create("", "", DateTime.Now, [], new System.Threading.CancellationToken(), "", null, null, null, null, null);
        var logger = new Mock<ILogger<DiscountService>>(MockBehavior.Loose);
        var discountCodeGeneretor = new Mock<IDiscountCodeGeneretor>(MockBehavior.Strict);
        var discountCodeController = new Mock<IDiscountCodeController>(MockBehavior.Strict);
        var service = new DiscountService(logger.Object, discountCodeGeneretor.Object, discountCodeController.Object);
        //Setup
        var generateRequest = new GenerateRequest { Count = 1500, Length = 8 };
        discountCodeGeneretor.Setup(g => g.Generate(generateRequest.Count, generateRequest.Length)).Returns([new("abcdefgh", false)]);
        discountCodeController.Setup(contr => contr.SaveDiscountCodeIfItDontExist(It.IsAny<DiscountCode>())).Returns(Task.FromResult(false));
        var result = await service.Generate(generateRequest, dummyServercallcontext).ConfigureAwait(false);
        //Test
        Assert.IsFalse(result.Result);
        logger.VerifyAll();
        discountCodeGeneretor.VerifyAll();
        discountCodeController.VerifyAll();
    }
    [Test]
    public async Task TestUseCode_When_Code_DoNot_Exist()
    {
        //Prepare
        var dummyServercallcontext = TestServerCallContext.Create("", "", DateTime.Now, [], new System.Threading.CancellationToken(), "", null, null, null, null, null);
        var logger = new Mock<ILogger<DiscountService>>(MockBehavior.Loose);
        var discountCodeGeneretor = new Mock<IDiscountCodeGeneretor>(MockBehavior.Strict);
        var discountCodeController = new Mock<IDiscountCodeController>(MockBehavior.Strict);
        var service = new DiscountService(logger.Object, discountCodeGeneretor.Object, discountCodeController.Object);
        var discountCode = new DiscountCode("abcdefgh", false);
        //Setup
        var useCodeRequest = new UseCodeRequest { Code = discountCode.Code };
        discountCodeController.Setup(contr => contr.DiscountCodeExists(discountCode)).Returns(Task.FromResult(false));
        var result = await service.UseCode(useCodeRequest, dummyServercallcontext).ConfigureAwait(false);
        //Test
        Assert.That(result.Result, Is.EqualTo(UseCodeResponse.Doesnotexist));
        logger.VerifyAll();
        discountCodeGeneretor.VerifyAll();
        discountCodeController.VerifyAll();
    }
    [Test]
    public async Task TestUseCode_When_Code_Is_Used()
    {
        //Prepare
        var dummyServercallcontext = TestServerCallContext.Create("", "", DateTime.Now, [], new System.Threading.CancellationToken(), "", null, null, null, null, null);
        var logger = new Mock<ILogger<DiscountService>>(MockBehavior.Loose);
        var discountCodeGeneretor = new Mock<IDiscountCodeGeneretor>(MockBehavior.Strict);
        var discountCodeController = new Mock<IDiscountCodeController>(MockBehavior.Strict);
        var service = new DiscountService(logger.Object, discountCodeGeneretor.Object, discountCodeController.Object);
        var discountCode = new DiscountCode("abcdefgh", false);
        //Setup
        var useCodeRequest = new UseCodeRequest { Code = discountCode.Code };
        discountCodeController.Setup(contr => contr.DiscountCodeExists(discountCode)).Returns(Task.FromResult(true));
        discountCodeController.Setup(contr => contr.GetDiscountCode(discountCode)).Returns(Task.FromResult(new DiscountCode(discountCode.Code, true)));
        var result = await service.UseCode(useCodeRequest, dummyServercallcontext).ConfigureAwait(false);
        //Test
        Assert.That(result.Result, Is.EqualTo(UseCodeResponse.Used));
        logger.VerifyAll();
        discountCodeGeneretor.VerifyAll();
        discountCodeController.VerifyAll();
    }
    [Test]
    public async Task TestUseCode_When_OK()
    {
        //Prepare
        var dummyServercallcontext = TestServerCallContext.Create("", "", DateTime.Now, [], new System.Threading.CancellationToken(), "", null, null, null, null, null);
        var logger = new Mock<ILogger<DiscountService>>(MockBehavior.Loose);
        var discountCodeGeneretor = new Mock<IDiscountCodeGeneretor>(MockBehavior.Strict);
        var discountCodeController = new Mock<IDiscountCodeController>(MockBehavior.Strict);
        var service = new DiscountService(logger.Object, discountCodeGeneretor.Object, discountCodeController.Object);
        var discountCode = new DiscountCode("abcdefgh", false);
        //Setup
        var useCodeRequest = new UseCodeRequest { Code = discountCode.Code };
        discountCodeController.Setup(contr => contr.DiscountCodeExists(discountCode)).Returns(Task.FromResult(true));
        discountCodeController.Setup(contr => contr.GetDiscountCode(discountCode)).Returns(Task.FromResult(discountCode));
        discountCodeController.Setup(contr => contr.UseDiscountCode(new DiscountCode(discountCode.Code, true))).Returns(Task.CompletedTask);
        var result = await service.UseCode(useCodeRequest, dummyServercallcontext).ConfigureAwait(false);
        //Test
        Assert.That(result.Result, Is.EqualTo(UseCodeResponse.Success));
        logger.VerifyAll();
        discountCodeGeneretor.VerifyAll();
        discountCodeController.VerifyAll();
    }

    [Test]
    public async Task TestGetUnusedCodes_When_OK()
    {
        //Prepare
        var dummyServercallcontext = TestServerCallContext.Create("", "", DateTime.Now, [], new System.Threading.CancellationToken(), "", null, null, null, null, null);
        var logger = new Mock<ILogger<DiscountService>>(MockBehavior.Loose);
        var discountCodeGeneretor = new Mock<IDiscountCodeGeneretor>(MockBehavior.Strict);
        var discountCodeController = new Mock<IDiscountCodeController>(MockBehavior.Strict);
        var service = new DiscountService(logger.Object, discountCodeGeneretor.Object, discountCodeController.Object);
        var discountCode = new DiscountCode("abcdefgh", false);
        //Setup
        var useCodeRequest = new GetUnusedCodesRequest { Count =1 };
        discountCodeController.Setup(contr => contr.GetUnusedDiscountCodes()).Returns(Task.FromResult(new List<DiscountCode>{discountCode}.AsEnumerable()));
        var result = await service.GetUnusedCodes(useCodeRequest, dummyServercallcontext).ConfigureAwait(false);
        //Test
        Assert.That(result.Codes, Has.Count.EqualTo(1));
        Assert.That(result.Codes.First(), Is.EqualTo(discountCode.Code));
        logger.VerifyAll();
        discountCodeGeneretor.VerifyAll();
        discountCodeController.VerifyAll();
    }
}