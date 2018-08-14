using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPower.QnA.DTO.QnADTO
{
    public class UpdatedQuestionDto
    {
        public List<string> delete { get; set; }
        public List<string> add { get; set; }
    }
}
