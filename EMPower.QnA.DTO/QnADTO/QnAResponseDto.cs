using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPower.QnA.DTO.QnADTO
{
    public class QnAResponseDto : HttpResponse
    {
        public QnAErrorResponseDto error { get; set; }
        public string operationState { get; set; }
    }
}
