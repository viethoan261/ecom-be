using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Statistic;
using WebFilm.Core.Interfaces.Repository;
using WebFilm.Core.Interfaces.Services;

namespace WebFilm.Core.Services
{
    public class StatisticService : IStatisticService
    {
        IUserRepository _userRepository;
        IUserContext _userContext;

        public StatisticService(IUserRepository userRepository,
            IUserContext userContext)
        {
            _userContext = userContext;
            _userRepository = userRepository;
        }

        public List<StatisticOrder> getStatisticOrder()
        {
            return _userRepository.orderStat();
        }

        public List<StatisticTurnover> getStatisticTurnover()
        {
            return _userRepository.statisticTurnovers();
        }

        public StatisticProduct getStatProduct()
        {
            return _userRepository.productStat();
        }

        public StatisticUser getStatUser()
        {
            return _userRepository.userStat();
        }
    }
}
