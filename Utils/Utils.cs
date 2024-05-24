using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntiqueShop.Utils
{
    internal class Utils
    {
        public static bool AnyIsNullOrEmpty(string[] strings)
        {
            if (strings == null)
            {
                return true;
            }

            foreach (string str in strings)
            {
                if (string.IsNullOrEmpty(str))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
