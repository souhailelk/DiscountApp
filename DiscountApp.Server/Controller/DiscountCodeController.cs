using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscountApp.Servrer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DiscountApp.Server.Controller;

public class DiscountCodeController
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
        return (context.DiscountCodes?.Where(e => e.Equals(dcode))).FirstOrDefault();
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
            if (!DiscountCodeExists(dcode))
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
    public bool DiscountCodeExists(DiscountCode dcode)
    {
        return (context.DiscountCodes?.Any(e => e.Equals(dcode))).GetValueOrDefault();
    }
}