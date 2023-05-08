using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFilm.Core.Enitites.Category
{
    public class CategoryDTO
    {
        public string? name { get; set; }
        public string? description { get; set; }
        public int categoryParentID { get; set; }
    }
}
