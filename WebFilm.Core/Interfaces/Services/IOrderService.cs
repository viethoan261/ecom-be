using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Cart;
using WebFilm.Core.Enitites.Order;

namespace WebFilm.Core.Interfaces.Services
{
    public interface IOrderService : IBaseService<int, Order>
    {
        Order newOrder(OrderCreateDTO dto);

        List<OrderResponse> getAll();

        bool changeStatusOrder(int orderID, string status);
    }
}
