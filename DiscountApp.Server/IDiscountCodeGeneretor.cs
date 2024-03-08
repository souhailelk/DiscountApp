using System.Collections.Generic;
using DiscountApp.Servrer.Models;

namespace DiscountApp.Server;

public interface IDiscountCodeGeneretor
{
    HashSet<DiscountCode> Generate(int count, int length);
}
