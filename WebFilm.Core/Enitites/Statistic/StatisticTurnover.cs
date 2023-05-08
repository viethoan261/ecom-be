using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFilm.Core.Enitites.Statistic
{
    public class StatisticTurnover
    {
        public DateTime? paymentDate { get; set; }
        public float totalAmount { get; set; }
    }
}
