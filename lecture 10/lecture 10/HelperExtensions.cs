using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lecture_10
{
    public static class HelperExtensions
    {
        public static decimal toDecimal(this string val)
        {
            decimal result = 0;
            decimal.TryParse(val, out result);
            return result;
        }

        public static int toInt(this string val)
        {
            int result = 0;
            int.TryParse(val, out result);
            return result;
        }
    }
}
