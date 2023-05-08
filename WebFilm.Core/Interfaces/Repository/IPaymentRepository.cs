﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Payment;

namespace WebFilm.Core.Interfaces.Repository
{
    public interface IPaymentRepository : IBaseRepository<int, Payment>
    {
        int newPayment(int orderID, float amount);

        Payment getByOrderID(int orderID);
    }
}
