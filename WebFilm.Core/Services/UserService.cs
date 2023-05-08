using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebFilm.Core.Enitites;
using WebFilm.Core.Enitites.Mail;
using WebFilm.Core.Enitites.User;
using WebFilm.Core.Exceptions;
using WebFilm.Core.Interfaces.Repository;
using WebFilm.Core.Interfaces.Services;

namespace WebFilm.Core.Services
{
    public class UserService : BaseService<int, User>, IUserService
    {
        IUserRepository _userRepository;
        IUserContext _userContext;
        private readonly IMailService _mail;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository,
            IMailService mail,
            IConfiguration configuration,
            IUserContext userContext) : base(userRepository)
        {
            _mail = mail;
            _configuration = configuration;
            _userContext = userContext;
            _userRepository = userRepository;
        }

        #region Method
        public User GetUserByID(Guid userID)
        {
            var user = _userRepository.GetUserByID(userID);
            return user;
        }

        public User Signup(UserDto user)
        {
            //Email phải đúng định dạng
            if (user.email != null && user.email.Length > 0)
            {
                if (!IsValidEmail(email: user.email))
                {
                    throw new ServiceException(Resources.Resource.Error_EmailFormat);
                }
            }

            //Email không được phép trùng
            var isDuplicateEmail = _userRepository.CheckDuplicateEmail(user.email);
            if (isDuplicateEmail)
            {
                throw new ServiceException(Resources.Resource.Error_Duplicate_Email);
            }

            var isDuplicateUsername = _userRepository.CheckDuplicateUsername(user.userName);
            //Username không được phép trùng
            if (isDuplicateUsername)
            {
                throw new ServiceException("Username không được phép trùng");
            }

            user.password = BCrypt.Net.BCrypt.HashPassword(user.password);
            var res = _userRepository.Signup(user);

            // Gửi mail
            MailTemplate welcomeMail = new MailTemplate()
            {
                Email = user.email,
                Name = user.userName,
                Role = user.role,
            };
            MailData mailData = new MailData()
            {
                To = new List<string>() { user.email },
                Subject = "Thank you for signing up",
                Body = _mail.GetEmailTemplate("welcome", welcomeMail)
            };
            _mail.SendAsync(mailData, new CancellationToken());

            return res;
        }

        public Dictionary<string, object> Login(string username, string password)
        {
            var user = _userRepository.Login(username);
            if (user != null)
            {
                if (user.status == 1)
                {
                    throw new ServiceException("Tài khoản chưa xác nhận email kích hoạt");
                }
                var isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, user.password);
                if (isPasswordCorrect)
                {
                    var token = GenarateToken(user);
                    //User userToken = new User();
                    //user.UserID = userDto.UserID;
                    //user.UserName = userDto.UserName;
                    //user.FullName = userDto.FullName;
                    //user.Email = userDto.Email;
                    //user.DateOfBirth = userDto.DateOfBirth;
                    //user.Status = userDto.Status;
                    //user.RoleType = userDto.RoleType;
                    //user.FavouriteFilmList = userDto.FavouriteFilmList;
                    return new Dictionary<string, object>()
                    {
                        { "Token", token }
                    };
                }
            }
            throw new ServiceException("Thông tin tài khoản hoặc mật khẩu không chính xác");
        }

        private string GenarateToken(User user)
        {
            // Authenticate user credentials and get the user's claims
            var claims = new List<Claim>
            {
                new Claim("ID", user.id.ToString()),
                new Claim("Username", user.userName),
                new Claim("Email", user.email),
                new Claim("Role", user.role),
                new Claim("Fullname", user.fullName ?? "")

                // Add any other user claims as needed
            };

            // Generate a symmetric security key using your secret key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            // Create a signing credentials object using the key
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Set token expiration time
            var expires = DateTime.UtcNow.AddDays(30);

            // Create a JWT token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            // Serialize the token to a string
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return "Bearer " + tokenString;
        }

        private bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        public bool ActiveUser(string email)
        {
            return _userRepository.ActiveUser(email);
        }

        public bool ChangePassword(string email, string oldPass, string newPass)
        {
            var user = _userRepository.Login(email);
            if (user != null)
            {
                //if (user.Status == 1)
                //{
                //    throw new ServiceException("Tài khoản chưa xác nhận email kích hoạt");
                //}
                var isPasswordCorrect = BCrypt.Net.BCrypt.Verify(oldPass, user.password);
                if (isPasswordCorrect)
                {
                    newPass = BCrypt.Net.BCrypt.HashPassword(newPass);
                    return _userRepository.ChangePassword(email, newPass);
                }
            }
            throw new ServiceException("Mật khẩu không chính xác");
        }

        public bool ForgotPassword(string email)
        {
            var userDto = _userRepository.GetUserByEmail(email);
            if (userDto == null)
            {
                throw new ServiceException("Email không tồn tại");
            }

            userDto.passwordResetToken = CreateRandomToken();
            userDto.resetTokenExpires = DateTime.Now.AddDays(1);

            var res = _userRepository.AddTokenReset(userDto);

             //Gửi mail
            if (res)
            {
                MailTemplate welcomeMail = new MailTemplate()
                {
                    Email = email,
                    Token = userDto.passwordResetToken,
                    Role = userDto.role
                };
                MailData mailData = new MailData()
                {
                    To = new List<string>() { email },
                    Subject = "Reset your password",
                    Body = _mail.GetEmailTemplate("resetPassword", welcomeMail)
                };
                _mail.SendAsync(mailData, new CancellationToken());
                return true;
            }
            return false;
        }

        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

        public async Task<bool> ResetPassword(string token, string pass, string confirmPass)
        {
            if (pass != confirmPass)
            {
                throw new ServiceException("The passwords you entered were not identical. Please try again.");
            }
            var user = await _userRepository.GetUserByTokenReset(token);
            if (user == null || user.resetTokenExpires < DateTime.Now)
            {
                throw new ServiceException("Invalid Token");
            }
            pass = BCrypt.Net.BCrypt.HashPassword(pass);
            return _userRepository.ChangePassword(user.email, pass);
        }

        public List<User> GetUsers()
        {
            var users = _userRepository.GetAll().Where(p => p.status == 2).ToList();
            foreach (var user in users)
            {
                user.password = "";
            }
            return users;
        }

        public User GetProfile()
        {
            int id = _userContext.UserId;
            var user = _userRepository.GetByID(id);

            user.password = "";
            return user;
        }

        public User ChangeProfile(UserProfileDTO dto)
        {
            int id = _userContext.UserId;
            
            var res = _userRepository.ChangeProfile(id, dto);
            return res;
        }

        public User editUser(int id, UserProfileDTO dto)
        {
            var res = _userRepository.ChangeProfile(id, dto);
            return res;
        }
        #endregion
    }
}
