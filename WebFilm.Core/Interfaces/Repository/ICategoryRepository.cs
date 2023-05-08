using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Category;
using WebFilm.Core.Enitites.User;

namespace WebFilm.Core.Interfaces.Repository
{
    public interface ICategoryRepository : IBaseRepository<int, Category>
    {
        Category newCategory(CategoryDTO dto);
        Category updateCategory(int id, CategoryDTO dto);
        Category inActive(int id);
    }
}
