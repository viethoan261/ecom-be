using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFilm.Core.Enitites.Order
{
    public class Order : BaseEntity
    {
        public int customerID { get; set; }
        public int staffID { get; set; }
        public DateTime? orderDate { get; set; }
        public float price { get; set; }
        public string status { get; set; }
        public string? address { get; set; }
        public string? method { get; set; }
    }
}
