﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityServer.Contracts
{
    public class UserCredsModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
    }
}
