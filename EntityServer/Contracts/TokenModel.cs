using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityServer.Contracts
{
    public class TokenModel
    {
        public Guid Tid { get; set; }
        public string Sub { get; set; }
        public string Exp { get; set; }
        public string Aud { get; set; }
    }
}
