using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Category;
using WebFilm.Core.Enitites.User;

namespace WebFilm.Core.Interfaces.Services
{
    public interface ICategoryService : IBaseService<int, Category>
    {
        Category create(CategoryDTO dto);

        Category updateCategory (int id, CategoryDTO dto);

        List<Category> getAll();

        Category Action(int id, string type);
    }
}
