using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Category;
using WebFilm.Core.Enitites.Product;
using WebFilm.Core.Enitites.Rating;

namespace WebFilm.Core.Interfaces.Services
{
    public interface IProductService : IBaseService<int, Product>
    {
        ProductCreateDTO create(ProductCreateDTO dto);

        List<ProductResponse> getAll(ProductFilter filter);

        Product Action(int id, string type);

        ProductResponse detailProduuct(int id);

        void testSocket();

        bool update(int id, ProductCreateDTO dto);

        bool deleteProperty(int propertyID);

        bool ratingProduct(RatingCreateDTO dto);
    }
}
