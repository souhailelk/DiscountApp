using System.Collections.Generic;
using System.Threading.Tasks;
using DiscountApp.Servrer.Models;

namespace DiscountApp.Server.Controller;

public interface IDiscountCodeController
{
    Task<IEnumerable<DiscountCode>> GetDiscountCodes();
    Task<DiscountCode> GetDiscountCode(DiscountCode dcode);
    Task UseDiscountCode(DiscountCode dcode);
    Task<DiscountCode> SaveDiscountCode(DiscountCode dcode);
    Task<bool> DiscountCodeExists(DiscountCode dcode);
    Task<bool> SaveDiscountCodeIfItDontExist(DiscountCode dcode);
    Task<IEnumerable<DiscountCode>> GetUnusedDiscountCodes();
}
