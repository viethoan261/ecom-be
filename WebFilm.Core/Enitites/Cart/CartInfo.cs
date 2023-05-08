using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Product;

namespace WebFilm.Core.Enitites.Cart
{
    public class CartInfo
    {
        public int id { get; set; }
        public float totalAmount { get; set; }
        public List<ProductCartDetail> products { get; set; }
    }
}
