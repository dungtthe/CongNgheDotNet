using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GUI
{
    public static class Util
    {
        public static string ConvertIdProductToString(int id, List<ProductDTO> products)
        {
            int maxId = products.Max(p => p.Id);
            int digits = maxId.ToString().Length;
            string prefix = "SP";
            string idFormatted = id.ToString().PadLeft(digits + 1, '0');
            return prefix + idFormatted;
        }

        public static int ConvertStringToProductId(string productId)
        {
            if (string.IsNullOrEmpty(productId) || !productId.StartsWith("SP"))
            {
                return -1; 
            }
            string numericPart = productId.Substring(2);

            if (int.TryParse(numericPart.TrimStart('0'), out int id))
            {
                return id;
            }
            return -1; 
        }

    }
}
