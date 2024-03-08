using Grpc.Core;

namespace DiscountApp.Server.UnitTests;

public class DiscountCodeGeneretorTest
{
    [TestCase(0),
    TestCase(100),
    TestCase(999),
    TestCase(2001)]
    public void TestGenerate_When_Count_IsNot_In_Range(int count)
    {
        var discountCodeGeneretor = new DiscountCodeGenertor();
        var exception = Assert.Throws<RpcException>(() => discountCodeGeneretor.Generate(count, 7));
        Assert.Multiple(() =>
        {
            Assert.That(exception.Message, Does.Contain("Codes count must be between 1000 and 2000!"));
            Assert.That(exception.StatusCode, Is.EqualTo(StatusCode.OutOfRange));
        });

    }
    [TestCase(0),
    TestCase(6),
    TestCase(9)]
    public void TestGenerate_When_Length_IsNot_In_Range(int length)
    {
        var discountCodeGeneretor = new DiscountCodeGenertor();
        var exception = Assert.Throws<RpcException>(() => discountCodeGeneretor.Generate(2000, length));
        Assert.Multiple(() =>
        {
            Assert.That(exception.Message, Does.Contain("Codes length must be either 7 and 8!"));
            Assert.That(exception.StatusCode, Is.EqualTo(StatusCode.OutOfRange));
        });

    }
    [TestCase(1500, 7),
    TestCase(2000, 8)]
    public void TestGenerate_When_Ok(int count, int length)
    {
        var discountCodeGeneretor = new DiscountCodeGenertor();
        var codes = discountCodeGeneretor.Generate(count, length);
        Assert.That(codes, Has.Count.EqualTo(count));
        CollectionAssert.AllItemsAreUnique(codes);
        foreach (var code in codes)
            Assert.That(code.Code, Has.Length.EqualTo(length));
    }
}