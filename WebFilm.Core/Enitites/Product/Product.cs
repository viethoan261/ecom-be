using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFilm.Core.Enitites.Product
{
    public class Product : BaseEntity
    {
        public string name { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
        public float price { get; set; }
        public string status { get; set; }
        public int categoryID { get; set; }
    }
}
