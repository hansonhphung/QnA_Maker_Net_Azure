using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EMPower.QnA.Common.Constants;
using EMPower.QnA.Data.Abstractions;
using EMPower.QnA.Data.Context;
using EMPower.QnA.DTO.Models;
using EMPower.QnA.DTO.QnADTO;

namespace EMPower.QnA.Data.Implementations
{
    public class QnAServices : BaseService, IQnAServices
    {
        public QnAServices(QnAEntities context) : base(context)
        {

        }

        public IList<QuestionsAndAnswers> GetQnAByDate(DateTime date)
        {
            var query = BuildGetQnAQuery(date);

            return query.Select(x => new QuestionsAndAnswers
            {
                HeatQuestionId = x.RecId,
                HeatQuestion = x.Title,
                Answer = x.Details,
                UpdatedDate = x.LastModDateTime,
                Status = x.Status
            }).ToList();
        }

        private IQueryable<FRS_Knowledge> BuildGetQnAQuery(DateTime date)
        {
            return Context.FRS_Knowledge.Where(q => q.LastModDateTime >= date && q.FRS_KnowledgeType == QuestionType.QnA && (q.Status == QuestionStatus.PUBLISHED || q.Status == QuestionStatus.ARCHIVED));
        }
    }
}
