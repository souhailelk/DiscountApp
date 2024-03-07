using System;
using System.ComponentModel.DataAnnotations;

namespace DiscountApp.Servrer.Models;

public class DiscountCode(string code, bool isUsed)
{
    [Key]
    public string Code { get; set; } = code;
    public bool IsUsed { get; set; } = isUsed;
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != this.GetType())
        {
            return false;
        }

        var other = (DiscountCode)obj;
        return Code == other.Code;
    }

    public override int GetHashCode()
    {
        return Code.GetHashCode();
    }
}