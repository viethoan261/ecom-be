using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Cart;
using WebFilm.Core.Enitites.User;

namespace WebFilm.Core.Interfaces.Repository
{
    public interface ICartRepository : IBaseRepository<int, Cart>
    {
        Cart newCart(int customerID);

        Cart getCart(int customerID);
    }
}
