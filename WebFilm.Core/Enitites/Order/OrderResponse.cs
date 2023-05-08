using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Product;
using WebFilm.Core.Enitites.Rating;

namespace WebFilm.Core.Enitites.Order
{
    public class OrderResponse
    {
        public int orderID { get; set; }
        public int customerID { get; set; }
        public string? name { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public string? address { get; set; }

        public string? status { get; set; }
        public float price { get; set; }
        public List<ProductCartDetail> products { get; set; }
        public List<RatingDTO> reviews { get; set; }
    }
}
