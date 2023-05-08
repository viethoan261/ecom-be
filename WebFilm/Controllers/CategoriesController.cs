using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebFilm.Core.Enitites.Category;
using WebFilm.Core.Enitites.User;
using WebFilm.Core.Exceptions;
using WebFilm.Core.Interfaces.Services;
using WebFilm.Core.Services;

namespace WebFilm.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : BaseController<int, Category>
    {
        #region Field
        IUserContext _userContext;
        ICategoryService _categoryService;

        #endregion

        #region Contructor
        public CategoriesController(ICategoryService categoryService, IUserContext userContext) : base(categoryService)
        {
            _categoryService = categoryService;
            _userContext = userContext;
        }
        #endregion

        [HttpPost("")]
        public IActionResult create(CategoryDTO dto)
        {
            try
            {
                var res = _categoryService.create(dto);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{id}")]
        public IActionResult update(int id, CategoryDTO dto)
        {
            try
            {
                var res = _categoryService.updateCategory(id, dto);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("{id}")]
        public IActionResult Action(int id, string type)
        {
            try
            {
                var res = _categoryService.Action(id, type);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("")]
        [AllowAnonymous]
        public IActionResult getAll()
        {
            try
            {
                var res = _categoryService.getAll();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
