using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFilm.Core.Enitites
{
    public class BaseEntity
    {
        #region Prop
        [Key]
        public int id { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? createdDate { get; set; }

        /// <summary>
        /// Ngày sửa
        /// </summary>
        public DateTime? modifiedDate { get; set; }

        #endregion
    }
}
