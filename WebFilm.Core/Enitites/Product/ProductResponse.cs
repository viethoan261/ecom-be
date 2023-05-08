using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Rating;

namespace WebFilm.Core.Enitites.Product
{
    public class ProductResponse
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
        public float price { get; set; }
        public string status { get; set; }
        public int categoryID { get; set; }
        public string categoryStatus { get; set; }
        public List<ProductDetailDTO> properties { get; set; }
        public List<RatingDTO> reviews { get; set; }
    }
}
