using System;
using System.Collections.Generic;
using DiscountApp.Servrer.Models;
using Grpc.Core;

namespace DiscountApp.Server;

public class DiscountCodeGenertor : IDiscountCodeGeneretor
{
    public HashSet<DiscountCode> Generate(int count, int length)
    {
        if (count > 2000 || count < 1000)
            throw new RpcException(new Status(StatusCode.OutOfRange, "Codes count must be between 1000 and 2000!"));
        if (length > 8 || length < 7)
            throw new RpcException(new Status(StatusCode.OutOfRange, "Codes length must be either 7 and 8!"));
        var codes = new HashSet<DiscountCode>();

        while (codes.Count < count)
        {
            var guid = Guid.NewGuid().ToString("N"); // N format removes hyphens
            var code = guid[..length];
            codes.Add(new DiscountCode(code, false));
        }
        return codes;
    }
}
