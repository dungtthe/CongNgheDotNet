using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class ProductDAO : IProductDAO
    {
        private FakeData Data = FakeData.GI();
        private static ProductDAO ins;
        public static ProductDAO GI()
        {
            if (ins == null)
            {
                ins = new ProductDAO();
            }
            return ins;
        }

        public bool DeleteProductById(int id)
        {
            var product = Data.Products().FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                Data.Products().Remove(product);
                return true;
            }

            return false;
        }

        public ProductDTO FindProductById(int id)
        {
            var product = Data.Products().FirstOrDefault(p => p.Id == id);
            return product;
        }

        public List<ProductDTO> GetAllProduct()
        {
            return Data.Products();
        }

        public ProductDTO UpdateProduct(ProductDTO product)
        {
            var p = FindProductById(product.Id);
            if (p != null)
            {
                p.Name = product.Name;
                p.Quantity = product.Quantity;
                p.UnitPrice = product.UnitPrice;
                p.Origin = product.Origin;
                p.ExpireDate = product.ExpireDate;
            }
            return p;
        }

        public ProductDTO AddProduct(ProductDTO product)
        {
            int newId = 1;
            if (Data.Products().Count > 0)
            {
                newId = Data.Products().Max(p => p.Id) + 1;
            }
            product.Id = newId;
            Data.Products().Add(product);
            return FindProductById(newId);
        }
    }
}
