using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPower.QnA.Common.CommonDTO
{
    public class ProxyDto
    {
        public bool UseInternetProxy { get; set; }
        public string InternetProxy { get; set; }
        public string SharedAccount { get; set; }
        public string SharedPassword { get; set; }
        public string SharedDomain { get; set; }
    }
}
