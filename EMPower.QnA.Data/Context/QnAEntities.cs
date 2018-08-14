using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPower.QnA.Data.Context
{
    public partial class QnAEntities : DbContext
    {
        public QnAEntities(string connectionString)
            : base("name=" + connectionString)
        {
        }
    }
}
