﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFilm.Core.Enitites.Cart
{
    public class Cart : BaseEntity
    {
        public int customerID { get; set; }
        public string status { get; set; }
    }
}
