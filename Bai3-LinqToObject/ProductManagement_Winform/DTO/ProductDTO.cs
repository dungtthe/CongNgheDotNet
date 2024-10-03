using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Origin { get; set; }
        public DateTime ExpireDate { get; set; }
        public ProductDTO()
        {

        }
        public ProductDTO(int id, string name, int quantity, decimal unitPrice, string origin, DateTime expireDate)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            UnitPrice = unitPrice;
            Origin = origin;
            ExpireDate = expireDate;
        }
    }
}
