using WebFilm.Core.Enitites;
using WebFilm.Core.Enitites.User;
namespace WebFilm.Core.Interfaces.Services
{
    public interface IUserService : IBaseService<int, User>
    {
        User GetUserByID(Guid userID);

        User Signup(UserDto user);

        Dictionary<string, object> Login(string username, string password);

        bool ActiveUser(string email);

        bool ChangePassword(string email, string oldPass, string newPass);

        bool ForgotPassword(string email);

        Task<bool> ResetPassword(string token, string pass, string confirmPass);

        List<User> GetUsers();

        User GetProfile();

        User ChangeProfile(UserProfileDTO dto);

        User editUser(int id, UserProfileDTO dto);
    }
}
