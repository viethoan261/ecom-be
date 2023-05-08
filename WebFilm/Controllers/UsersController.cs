using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebFilm.Controllers;
using WebFilm.Core.Enitites;
using WebFilm.Core.Enitites.Product;
using WebFilm.Core.Enitites.User;
using WebFilm.Core.Interfaces.Services;
using WebFilm.Core.Services;

namespace WebFilm.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController<int, User>
    {
        #region Field
        IUserService _userService;
        public static IWebHostEnvironment _webHostEnvironment;
        IUserContext _userContext;

        #endregion

        #region Contructor
        public UsersController(IUserService userService, IWebHostEnvironment webHostEnvironment, IUserContext userContext) : base(userService)
        {
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
            _userContext = userContext;
        }
        #endregion

        #region Method
        [HttpPost("Signup")]
        [AllowAnonymous]
        public IActionResult Signup(UserDto user)
        {
            try
            {
                var res = _userService.Signup(user);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public IActionResult Login(LoginDTO dto)
        {
            try
            {
                var res = _userService.Login(dto.userName, dto.password);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("Active")]
        [AllowAnonymous]
        public IActionResult ActiveUser(string email)
        {
            try
            {
                var res = _userService.ActiveUser(email);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        //[HttpPost("ChangePassword")]
        //public IActionResult ChangePassword(string email, string oldPass, string newPass)
        //{
        //    try
        //    {
        //        var res = _userService.ChangePassword(email, oldPass, newPass);
        //        return Ok(res);
        //    }
        //    catch (Exception ex)
        //    {
        //        return HandleException(ex);
        //    }
        //}

        [HttpGet("ForgotPassword")]
        [AllowAnonymous]
        public IActionResult ForgotPassword(string email)
        {
            try
            {
                var res = _userService.ForgotPassword(email);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string token, string pass, string confirmPass)
        {
            try
            {
                var res = await _userService.ResetPassword(token, pass, confirmPass);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("")]
        public IActionResult getUsers()
        {
            try
            {
                var res =  _userService.GetUsers();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("Profile")]
        public IActionResult getProfile()
        {
            try
            {
                var res = _userService.GetProfile();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost("Profile")]
        public IActionResult changeProfile([FromBody] UserProfileDTO dto)
        {
            try
            {
                var res = _userService.ChangeProfile(dto);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{id}/edit")]
        public IActionResult editUser(int id, [FromBody] UserProfileDTO dto)
        {
            try
            {
                var res = _userService.editUser(id, dto);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        #endregion
    }
}
