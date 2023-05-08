using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFilm.Core.Enitites.Product
{
    public class ProductCreateDTO
    {
        public string name { get; set; }
        public string description { get; set; }
        public float price { get; set; }
        public int categoryID { get; set; }
        public List<ProductDetailDTO> properties { get; set; }
    }
}
