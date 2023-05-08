using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFilm.Core.Enitites.Order
{
    public class OrderCreateDTO
    {
        public string? address { get; set; }
        public string? method { get; set; }
        public List<OrderDTO> products { get; set; }
    }
}
