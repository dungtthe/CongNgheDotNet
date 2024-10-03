using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public interface IProductDAO
    {
        List<ProductDTO> GetAllProduct();
        ProductDTO FindProductById(int id);
        bool DeleteProductById(int id);
        ProductDTO UpdateProduct(ProductDTO product);
        ProductDTO AddProduct(ProductDTO product);
    }
}
