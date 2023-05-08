using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebFilm.Core.Enitites.Category;
using WebFilm.Core.Enitites.Product;
using WebFilm.Core.Enitites.Rating;
using WebFilm.Core.Interfaces.Services;
using WebFilm.Core.Services;

namespace WebFilm.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseController<int, Product>
    {
        #region Field
        IUserContext _userContext;
        IProductService _productService;

        #endregion

        #region Contructor
        public ProductsController(IProductService productService, IUserContext userContext) : base(productService)
        {
            _productService = productService;
            _userContext = userContext;
        }
        #endregion

        [HttpPost("")]
        public IActionResult create([FromBody] ProductCreateDTO dto)
        {
            try
            {
                var res = _productService.create(dto);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("search")]
        [AllowAnonymous]
        public IActionResult getAll([FromBody] ProductFilter filter)
        {
            try
            {
                var res = _productService.getAll(filter);
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
                var res = _productService.Action(id, type);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("{id}/detail")]
        [AllowAnonymous]
        public IActionResult detail(int id)
        {
            try
            {
                var res = _productService.detailProduuct(id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("test")]
        [AllowAnonymous]
        public IActionResult socket()
        {
            try
            {
                _productService.testSocket();
                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{id}")]
        public IActionResult update(int id, [FromBody] ProductCreateDTO dto)
        {
            try
            {
                var res = _productService.update(id, dto);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("{propertyID}/property")]
        public IActionResult removeProperty(int propertyID)
        {
            try
            {
                var res = _productService.deleteProperty(propertyID);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("rating")]
        public IActionResult rating([FromBody] RatingCreateDTO dto)
        {
            try
            {
                var res = _productService.ratingProduct(dto);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
