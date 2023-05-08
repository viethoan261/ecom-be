using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebFilm.Core.Interfaces.Services;
using WebFilm.Core.Services;

namespace WebFilm.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        #region Field
        IUserContext _userContext;
        IStatisticService _statisticService;

        #endregion

        #region Contructor
        public StatisticsController(IStatisticService statisticService, IUserContext userContext)
        {
            _statisticService = statisticService;
            _userContext = userContext;
        }
        #endregion

        [HttpGet("users")]
        public IActionResult statUser()
        {
            try
            {
                var res = _statisticService.getStatUser();
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    devMsg = ex.Message,
                    userMsg = Core.Resources.Resource.Error_Exception,
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("products")]
        public IActionResult statProduct()
        {
            try
            {
                var res = _statisticService.getStatProduct();
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    devMsg = ex.Message,
                    userMsg = Core.Resources.Resource.Error_Exception,
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("orders")]
        public IActionResult statOrder()
        {
            try
            {
                var res = _statisticService.getStatisticOrder();
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    devMsg = ex.Message,
                    userMsg = Core.Resources.Resource.Error_Exception,
                };
                return StatusCode(500, response);
            }
        }

        [HttpGet("turnover")]
        public IActionResult turnoverStat()
        {
            try
            {
                var res = _statisticService.getStatisticTurnover();
                return Ok(res);
            }
            catch (Exception ex)
            {
                var response = new
                {
                    devMsg = ex.Message,
                    userMsg = Core.Resources.Resource.Error_Exception,
                };
                return StatusCode(500, response);
            }
        }
    }
}
