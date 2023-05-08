using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFilm.Core.Enitites.User
{
    public class UserDto
    {
        public string userName { get; set; }
        public string password { get; set; }

        public string? fullName { get; set; }
        public string? role { get; set; }
        public string email { get; set; }
        public string? phone { get; set; }
    }
}
