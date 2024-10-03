using DAO;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class ProductService : IProductService

    {
        private static ProductService ins;
        public static ProductService GI()
        {
            if (ins == null)
            {
                ins = new ProductService();
            }
            return ins;
        }

        private IProductDAO productDAO = ProductDAO.GI();

        public bool DeleteProductById(int id)
        {
            return productDAO.DeleteProductById(id);
        }

        public ProductDTO FindProductById(int id)
        {
            return productDAO.FindProductById(id);
        }

        public List<ProductDTO> GetAllProduct()
        {
            return productDAO.GetAllProduct();
        }

        public ProductDTO UpdateProduct(ProductDTO product)
        {
            return productDAO.UpdateProduct(product);
        }
        public ProductDTO AddProduct(ProductDTO product)
        {
            return productDAO.AddProduct(product);
        }
    }
}
