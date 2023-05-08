using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebFilm.Core.Enitites.Cart;
using WebFilm.Core.Enitites.Order;
using WebFilm.Core.Interfaces.Services;
using WebFilm.Core.Services;

namespace WebFilm.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : BaseController<int, Order>
    {
        #region Field
        IUserContext _userContext;
        IOrderService _orderService;

        #endregion

        #region Contructor
        public OrdersController(IOrderService orderService, IUserContext userContext) : base(orderService)
        {
            _orderService = orderService;
            _userContext = userContext;
        }
        #endregion

        [HttpPost("")]
        public IActionResult create([FromBody] OrderCreateDTO dto)
        {
            try
            {
                var res = _orderService.newOrder(dto);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("")]
        public IActionResult getAll()
        {
            try
            {
                var res = _orderService.getAll();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{id}/status")]
        public IActionResult changeStatus(int id, string status)
        {
            try
            {
                var res = _orderService.changeStatusOrder(id, status);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

    }
}
