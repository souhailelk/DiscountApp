using System;
using System.Collections.Generic;
using DiscountApp.Servrer.Models;

namespace DiscountApp.Server;

public class DiscountCodeGenertor
{
    public HashSet<DiscountCode> Generate(int count, int length)
    {
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
