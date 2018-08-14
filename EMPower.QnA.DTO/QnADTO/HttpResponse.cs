using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace EMPower.QnA.DTO.QnADTO
{
    public class HttpResponse
    {
        public HttpResponseHeaders Headers { get; set; }

        public string Message { get; set; }
    }
}
