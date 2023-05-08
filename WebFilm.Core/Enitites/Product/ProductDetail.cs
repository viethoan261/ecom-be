using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFilm.Core.Enitites.Product
{
    public class ProductDetail : BaseEntity
    {
        public int productID { get; set; }
        public string color { get; set; }
        public string size { get; set; }
        public int quantity { get; set; }
        public string imagePath { get; set; }
        public string status { get; set; }
    }
}
