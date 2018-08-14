using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPower.QnA.DTO.QnADTO
{
    public class UpdatedQuestionMetaData
    {
        public List<QnAQuestionMetaData> add { get; set; }
        public List<QnAQuestionMetaData> delete { get; set; }
    }
}
