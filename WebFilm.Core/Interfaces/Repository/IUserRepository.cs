using WebFilm.Core.Enitites;
using WebFilm.Core.Enitites.Statistic;
using WebFilm.Core.Enitites.User;

namespace WebFilm.Core.Interfaces.Repository
{
    public interface IUserRepository : IBaseRepository<int, User>
    {
        User GetUserByID(Guid userID);

        User Signup(UserDto user);

        User Login(string username);

        bool CheckDuplicateEmail(string email);

        bool CheckDuplicateUsername (string username);

        bool ActiveUser(string email);

        bool ChangePassword(string email, string newPass);

        bool AddTokenReset(User user);

        Task<User> GetUserByTokenReset(string token);

        User getUserByUsername(string username);

        User GetUserByEmail(string email);

        User ChangeProfile(int id, UserProfileDTO dto);

        StatisticUser userStat();

        StatisticProduct productStat();

        List<StatisticOrder> orderStat();

        List<StatisticTurnover> statisticTurnovers();
    }
}
