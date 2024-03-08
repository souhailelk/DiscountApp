using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscountApp.Servrer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscountApp.Server.Controller;
public class DiscountCodeController : IDiscountCodeController
{
    private readonly DiscountCodeContext context;
    public DiscountCodeController(DiscountCodeContext context)
    {
        this.context = context;
    }
    public async Task<IEnumerable<DiscountCode>> GetDiscountCodes()
    {
        if (context.DiscountCodes == null)
        {
            throw new Exception("Could not find DiscountCodes table in db");
        }
        return await context.DiscountCodes.ToListAsync();
    }
    public async Task<DiscountCode> GetDiscountCode(DiscountCode dcode)
    {
        if (context.DiscountCodes == null)
        {
            throw new Exception("Could not find DiscountCodes table in db");
        }
        return await context.DiscountCodes.Where(e => e.Equals(dcode)).FirstAsync().ConfigureAwait(false);
    }
    public async Task UseDiscountCode(DiscountCode dcode)
    {

        context.Entry(dcode).State = EntityState.Modified;
        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await DiscountCodeExists(dcode).ConfigureAwait(false))
            {
                throw new Exception($"Could not find the code {dcode.Code} in DB");
            }
            else
            {
                throw;
            }
        };
    }
    public async Task<DiscountCode> SaveDiscountCode(DiscountCode dcode)
    {
        if (context.DiscountCodes == null)
        {
            throw new Exception("Entity set 'AppDbContext.DiscountCodes'  is null.");
        }
        context.DiscountCodes.Add(dcode);
        await context.SaveChangesAsync().ConfigureAwait(false);
        return dcode;
    }
    public async Task<bool> DiscountCodeExists(DiscountCode dcode)
    {
         if (context.DiscountCodes == null)
        {
            throw new Exception("Could not find DiscountCodes table in db");
        }
        return await context.DiscountCodes.Where(e => e.Equals(dcode)).AnyAsync().ConfigureAwait(false);
    }
    public async Task<bool> SaveDiscountCodeIfItDontExist(DiscountCode dcode)
    {
        if(await DiscountCodeExists(dcode).ConfigureAwait(false))
            return false;
        await SaveDiscountCode(dcode).ConfigureAwait(false);
        return true;
    }
}