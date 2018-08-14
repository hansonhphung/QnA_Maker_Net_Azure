using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMPower.QnA.DTO.Models;
using EMPower.QnA.DTO.QnADTO;

namespace EMPower.QnA.Data.Abstractions
{
    public interface IQnAServices:IServices
    {
        IList<QuestionsAndAnswers> GetQnAByDate(DateTime date);
    }
}
