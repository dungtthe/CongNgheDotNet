using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class FakeData
    {
        private object lockFakeData = new object();
        private static FakeData ins;
        public static FakeData GI()
        {
            if (ins == null)
            {
                ins = new FakeData();
            }
            return ins;
        }


        //dữ liệu fake 
        private Random rnd = new Random();
        private List<ProductDTO> products;
        public List<ProductDTO> Products()
        {
            lock (lockFakeData)
            {
                if (products == null)
                {
                    int row = 100;
                    products = new List<ProductDTO>(row);

                    for (int i = 1; i <= row; i++)
                    {
                        products.Add(new ProductDTO()
                        {
                            Id = i,
                            Name = $"Product {i}",
                            UnitPrice = rnd.Next(10000, 100001),
                            Quantity = rnd.Next(1, 21),
                            Origin = RandomOrigin(),
                            ExpireDate = DateTime.Now.AddDays(-i)
                        });
                    }

                }
                return products;
            }
        }
        private string RandomOrigin()
        {
            int valueRandom = rnd.Next(1, 91);
            if (valueRandom < 30)
            {
                return "Việt Nam";
            }
            if (valueRandom > 60)
            {
                return "Trung Quốc";
            }
            return "Thái Lan";
        }
    }
}
