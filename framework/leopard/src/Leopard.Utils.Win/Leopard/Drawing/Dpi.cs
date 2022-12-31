using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leopard.Drawing
{
    /// <summary>
    /// DPI dots per inch ， 直接来说就是一英寸多少个像素点。我一般称作像素密度，简称密度
    /// </summary>
    public class Dpi
    {
        public Dpi(float x, float y)
        {
            X = x;
            Y = y;
        }

        public float X { get; private set; }
        public float Y { get; private set; }
    }
}
