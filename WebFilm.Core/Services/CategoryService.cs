using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Category;
using WebFilm.Core.Enitites.Product;
using WebFilm.Core.Enitites.User;
using WebFilm.Core.Exceptions;
using WebFilm.Core.Interfaces.Repository;
using WebFilm.Core.Interfaces.Services;

namespace WebFilm.Core.Services
{
    public class CategoryService : BaseService<int, Category>, ICategoryService
    {
        IUserContext _userContext;
        ICategoryRepository _categoryRepository;
        private readonly IConfiguration _configuration;

        public CategoryService(ICategoryRepository categoryRepository,
            IConfiguration configuration,
            IUserContext userContext) : base(categoryRepository)
        {
            _configuration = configuration;
            _userContext = userContext;
            _categoryRepository = categoryRepository;
        }

        public Category create(CategoryDTO dto)
        {
            if ("CUSTOMER".Equals(_userContext.Role))
            {
                throw new ServiceException(Resources.Resource.Not_Permission);
            }
            var category = _categoryRepository.newCategory(dto);
            return category;
        }

        public Category updateCategory(int id, CategoryDTO dto)
        {
            if ("CUSTOMER".Equals(_userContext.Role))
            {
                throw new ServiceException(Resources.Resource.Not_Permission);
            }
            var category = _categoryRepository.updateCategory(id, dto);
            if (category == null)
            {
                throw new ServiceException(Resources.Resource.Category_Not_Existed);
            }
            return category;
        }

        public List<Category> getAll()
        {
            var categories = _categoryRepository.GetAll().ToList();
            return categories;
        }

        public Category Action(int id, string type)
        {
            if ("CUSTOMER".Equals(_userContext.Role))
            {
                throw new ServiceException(Resources.Resource.Not_Permission);
            }
            var category = _categoryRepository.GetByID(id);
            if (category == null)
            {
                throw new ServiceException(Resources.Resource.Category_Not_Existed);
            }

            if ("INACTIVE".Equals(type))
            {
                if (!"ACTIVE".Equals(category.status))
                {
                    throw new ServiceException(Resources.Resource.Category_Not_Existed);
                }
                this.ActionSubCategory(category, "INACTIVE");
            }

            if ("ACTIVE".Equals(type))
            {
                if (!"INACTIVE".Equals(category.status))
                {
                    throw new ServiceException(Resources.Resource.Product_Not_Used);
                }
                this.ActionSubCategory(category, "ACTIVE");
            }

            return category;
        }

        private void ActionSubCategory(Category category, string type)
        {
            
            if ("ACTIVE".Equals(type))
            {
                if (!CheckParrentStatus(category))
                {
                    throw new ServiceException(Resources.Resource.Action_Fail);
                }
                List<Category> categories = _categoryRepository.GetAll().Where(p => p.categoryParentID == category.id && "INACTIVE".Equals(p.status)).ToList();
                category.status = "ACTIVE";
                _categoryRepository.Edit(category.id, category);
                foreach (Category cate in categories)
                {
                    this.ActionSubCategory(cate, "ACTIVE");
                }
            }

            if ("INACTIVE".Equals(type))
            {
                List<Category> categories = _categoryRepository.GetAll().Where(p => p.categoryParentID == category.id && "ACTIVE".Equals(p.status)).ToList();
                category.status = "INACTIVE";
                _categoryRepository.Edit(category.id, category);
                foreach (Category cate in categories)
                {
                    this.ActionSubCategory(cate, "INACTIVE");
                }
            }
        }

        private bool CheckParrentStatus(Category category)
        {
                List<Category> parent = _categoryRepository.GetAll().Where(p => p.id == category.categoryParentID).ToList();
                if (parent.Count > 0)
                {
                    if ("INACTIVE".Equals(parent[0].status))
                    {
                        return false;
                    }
                    this.CheckParrentStatus(parent[0]);
                }
            return true;
        }
    }
}
