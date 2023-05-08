using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Cart;

namespace WebFilm.Core.Interfaces.Repository
{
    public interface ICartDetailRepository : IBaseRepository<int, CartDetail>
    {
        CartDetail getCartDetail(int productID, string size, string color, int cartID);
    }
}
