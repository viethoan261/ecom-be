using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFilm.Core.Enitites.Image
{
    public class Image : BaseEntity
    {
        public int productID { get; set; }
        public string? imagePath { get; set; }
    }
}
