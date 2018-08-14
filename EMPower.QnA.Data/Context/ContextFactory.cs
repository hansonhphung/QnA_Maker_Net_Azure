using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPower.QnA.Data.Context
{
    public static class QnAContextFactory
    {
        public static QnAEntities CreateContext(string connectionStringName = null)
        {
            if (string.IsNullOrWhiteSpace(connectionStringName))
                return new QnAEntities();
            else
                return new QnAEntities(connectionStringName);
        }
    }
}
