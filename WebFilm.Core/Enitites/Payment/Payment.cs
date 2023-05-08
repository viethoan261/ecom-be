using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFilm.Core.Enitites.Payment
{
    public class Payment : BaseEntity
    {
        public int orderID { get; set; }
        public float amount { get; set; }
        public DateTime? paymentDate { get; set; }
    }
}
