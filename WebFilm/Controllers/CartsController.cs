using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebFilm.Core.Enitites.Cart;
using WebFilm.Core.Interfaces.Services;

namespace WebFilm.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : BaseController<int, Cart>
    {
        #region Field
        IUserContext _userContext;
        ICartService _cartService;

        #endregion

        #region Contructor
        public CartsController(ICartService cartService, IUserContext userContext) : base(cartService)
        {
            _cartService = cartService;
            _userContext = userContext;
        }
        #endregion

        [HttpPost("")]
        public IActionResult create(CartProductDTO dto)
        {
            try
            {
                var res = _cartService.addProductToCart(dto);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("")]
        public IActionResult getCart()
        {
            try
            {
                var res = _cartService.getCartInfo();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("{cartDetailID}")]
        public IActionResult removeProduct(int cartDetailID)
        {
            try
            {
                var res = _cartService.removeProduct(cartDetailID);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{cartDetailID}")]
        public IActionResult updateCart(int cartDetailID, CartProductDTO dto)
        {
            try
            {
                var res = _cartService.updateCartDetail(cartDetailID, dto);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
