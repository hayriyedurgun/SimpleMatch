using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets._Scripts.Utils
{
    public static class LayerHelper
    {
        public static int Or(params Layer[] layers)
        {
            var val = 0;
            for (int i = 0; i < layers.Length; i++)
            {
                val |= 1 << (int)layers[i];
            }

            return val;
        }
    }
}
