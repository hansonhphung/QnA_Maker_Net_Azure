using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPower.QnA.DTO.Models
{
    public class QuestionsAndAnswers
    {
        public string HeatQuestionId { get; set; }
        public int QnAQuestionId { get; set; }
        public string HeatQuestion { get; set; }
        public List<string> QnAQuestion { get; set; }
        public string Answer { get; set; }
        public string Status { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
