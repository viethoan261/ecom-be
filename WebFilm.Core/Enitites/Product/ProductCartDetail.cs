using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFilm.Core.Enitites.Product
{
    public class ProductCartDetail : ProductDetailDTO
    {
        public int productID { get; set; }
        public int cartDetailID { get; set; }
        public int productDetailID { get; set; }
        public string? name { get; set; }
        public float price { get; set; }
        public List<ProductDetailDTO> properties { get; set; }
    }
}
