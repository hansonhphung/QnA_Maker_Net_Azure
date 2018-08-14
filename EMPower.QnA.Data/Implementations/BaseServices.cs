using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EMPower.QnA.Data.Context;

namespace EMPower.QnA.Data.Implementations
{
    public abstract class BaseService
    {
        protected QnAEntities Context { get; private set; }
        public BaseService(QnAEntities context)
        {
            Context = context;
            Context.Database.CommandTimeout = SqlCommandTimeout();
        }

        private static int SqlCommandTimeout()
        {
            int value;
            return int.TryParse(ConfigurationManager.AppSettings["SqlCommandTimeout"], out value) ? value : 0;
        }

        protected virtual List<TEntity> MakeDatabaseQueryWithoutPagging<TEntity>(IQueryable<TEntity> query)
        {
            return query
                .ToList();
        }

        protected virtual List<TEntity> MakeDatabaseQuery<TEntity>(IQueryable<TEntity> query)
        {
            return query.ToList();
        }
    }
}
