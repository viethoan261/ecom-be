using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Cart;

namespace WebFilm.Core.Interfaces.Services
{
    public interface ICartService : IBaseService<int, Cart>
    {
        bool addProductToCart(CartProductDTO dto);

        CartInfo getCartInfo();

        bool removeProduct(int cartDetailID);

        bool updateCartDetail(int cartDetailID, CartProductDTO dto);
    }
}
