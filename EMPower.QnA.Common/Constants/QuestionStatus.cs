using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPower.QnA.Common.Constants
{
    public static class QuestionStatus
    {
        //In Review process QnAs
        public const string IN_REVIEW = "In Review";
        //Approved QnAs
        public const string PUBLISHED = "Published";
        //Review to delete QnAs
        public const string EXPIRED = "Expired";
        //Deleted QnAs
        public const string ARCHIVED = "Archived";
        //Newly created QnAs, need to be review and approved
        public const string DRAFT = "Draft";
    }
}
