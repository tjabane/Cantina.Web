﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cantina.Core.Dto
{
    public class Review
    {
        public int UserId { get; set; }
        public int MenuId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
    }
}
