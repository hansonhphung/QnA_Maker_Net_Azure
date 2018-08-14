using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPower.QnA.DTO.QnADTO
{
    public class UpdateQnAQuestionDto
    {
        public int id { get; set; }
        public string answer { get; set; }
        public string source { get; set; }
        public UpdatedQuestionDto questions { get; set; }
        public List<QnAQuestionMetaData> metadata { get; set; }
    }
}
