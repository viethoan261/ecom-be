using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFilm.Core.Enitites.Category
{
    public class Category : BaseEntity
    {
        public string? name { get; set; }
        public string? description { get; set; }
        public string? status { get; set; }
        public int categoryParentID { get; set; }
    }
}
