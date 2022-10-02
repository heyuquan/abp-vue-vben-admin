using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Numeric
{
    public static class TaxPercentageExtenstions
    {
        ///// <summary>
        ///// Calculates the tax (percentage) from a gross and a net value.
        ///// </summary>
        ///// <param name="inclTax">Gross value</param>
        ///// <param name="exclTax">Net value</param>
        ///// <param name="decimals">Rounding decimal number</param>
        ///// <returns>Tax percentage</returns>
        //public static decimal ToTaxPercentage(this decimal inclTax, decimal exclTax, int? decimals = null)
        //{
        //    if (exclTax == decimal.Zero)
        //    {
        //        return decimal.Zero;
        //    }

        //    var result = ((inclTax / exclTax) - 1.0M) * 100.0M;

        //    return (decimals.HasValue ? Math.Round(result, decimals.Value) : result);
        //}
    }
}
