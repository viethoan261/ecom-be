using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Cart;
using WebFilm.Core.Enitites.Image;
using WebFilm.Core.Interfaces.Repository;

namespace WebFilm.Infrastructure.Repository
{
    public class ImageRepository : BaseRepository<int, Image>, IImageRepository
    {
        public ImageRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
