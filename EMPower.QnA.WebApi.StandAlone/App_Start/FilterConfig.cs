using System.Web;
using System.Web.Mvc;

namespace EMPower.QnA.WebApi.StandAlone
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
