using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using log4net;

namespace EMPower.QnA.WebApi.StandAlone.Controllers
{
    public class BaseController : ApiController
    {
        protected static readonly ILog logger = LogManager.GetLogger("QnA.WebApi.StandAlone");
    }
}
