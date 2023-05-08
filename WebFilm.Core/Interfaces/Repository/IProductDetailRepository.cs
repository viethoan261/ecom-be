using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Cart;
using WebFilm.Core.Enitites.Product;

namespace WebFilm.Core.Interfaces.Repository
{
    public interface IProductDetailRepository : IBaseRepository<int, ProductDetail>
    {
        ProductDetail getProductDetail(int productID, string size, string color);
    }
}
