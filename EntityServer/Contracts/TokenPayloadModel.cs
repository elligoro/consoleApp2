using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityServer.Contracts
{
    public class TokenPayloadModel
    {
        public string Tid { get; set; }
        public string Sub { get; set; }
        public DateTime Exp { get; set; }
        public string Aud { get; set; }
        public string Iss { get; set; }
        public Dictionary<string, string> Attr { get; set; }
    }
}
