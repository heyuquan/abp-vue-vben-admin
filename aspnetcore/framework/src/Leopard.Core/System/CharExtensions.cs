using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class CharExtensions
    {
        public static int ToInt(this char ch)
        {
            if (ch >= '0' && ch <= '9')
            {
                return ch - '0';
            }
            else if (ch >= 'a' && ch <= 'f')
            {
                return (ch - 'a') + 10;
            }
            else if (ch >= 'A' && ch <= 'F')
            {
                return (ch - 'A') + 10;
            }

            return -1;
        }
    }
}
