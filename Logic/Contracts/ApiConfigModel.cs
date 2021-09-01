using System;
using System.Collections.Generic;
using System.Text;

namespace Logic.Contracts
{
    public class ApiConfigModel
    {
        public string ApiId { get; set; }
        public string ApiUrl { get; set; }
        public string ApiName { get; set; }
        public string Secret { get; set; }
        public string Salt { get; set; }
    }
}
