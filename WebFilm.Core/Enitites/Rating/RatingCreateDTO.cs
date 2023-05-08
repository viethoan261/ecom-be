using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFilm.Core.Enitites.Rating
{
    public class RatingCreateDTO
    {
        public int orderID { get; set; }
        public List<RatingDTO> rating { get; set; }
    }
}
