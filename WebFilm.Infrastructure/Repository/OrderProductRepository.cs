using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Cart;
using WebFilm.Core.Enitites.Order;
using WebFilm.Core.Interfaces.Repository;

namespace WebFilm.Infrastructure.Repository
{
    public class OrderProductRepository : BaseRepository<int, OrderProduct>, IOrderProductRepository
    {
        public OrderProductRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
