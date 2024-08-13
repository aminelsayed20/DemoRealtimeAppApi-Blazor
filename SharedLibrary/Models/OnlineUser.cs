﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Models
{
    public class OnlineUser
    {
        public string? UserName  { get; set; }
        public string? Status { get; set; }
        public DateTime? StartConnection { get; set; }
    }
}
