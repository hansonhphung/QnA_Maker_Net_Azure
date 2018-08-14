using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPower.QnA.DTO.QnADTO
{
    public class QnAQuestionDto
    {
        public int id { get; set; }
        public string answer { get; set; }
        public string source { get; set; }
        public string[] questions { get; set; }
        public QnAQuestionMetaData[] metadata { get; set; }
    }
}
