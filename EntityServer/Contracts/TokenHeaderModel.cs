using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityServer.Contracts
{
    public class TokenHeaderModel
    {
        public string Alg { get; set; }
        public string Kid { get; set; }
    }
}
