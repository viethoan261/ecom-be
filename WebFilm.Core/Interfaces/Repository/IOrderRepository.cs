﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Order;
using WebFilm.Core.Enitites.User;

namespace WebFilm.Core.Interfaces.Repository
{
    public interface IOrderRepository : IBaseRepository<int, Order>
    {
        Order newOrder(int customerID, float price, string address, string method);
    }
}
