using System.ComponentModel.DataAnnotations;

namespace WebFilm.Core.Enitites.User
{
    public class User : BaseEntity
    {
        public string userName { get; set; }
        public string? fullName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string ward { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public int status { get; set; }
        public string? passwordResetToken { get; set; }
        public DateTime? resetTokenExpires { get; set; }

    }
}
