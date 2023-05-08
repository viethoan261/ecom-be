using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFilm.Core.Enitites.Rating
{
    public class RatingDTO
    {
        public int productDetailID { get; set; }
        public string author { get; set; }
        public float score { get; set; }
        public string review { get; set; }
    }
}
