
using System;
using Microsoft.EntityFrameworkCore;


namespace DiscountApp.Servrer.Models;
public class DiscountCodeContext : DbContext
{
    public DbSet<DiscountCode> DiscountCodes { get; set; }

    public string DbPath { get; }

    public DiscountCodeContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "discountCode.db");
    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}
