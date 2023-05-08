using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFilm.Core.Enitites.Rating
{
    public class Rating : BaseEntity
    {
        public int userID { get; set; }
        public int productID { get; set; }
        public float score { get; set; }
        public string review { get; set; }
        public int orderID { get; set; }
    }
}
