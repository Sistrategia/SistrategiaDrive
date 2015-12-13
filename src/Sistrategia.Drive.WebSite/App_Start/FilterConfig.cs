using System.Web;
using System.Web.Mvc;

namespace Sistrategia.Drive.WebSite
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
            //var e = new HandleErrorAttribute();
            //e.View = "Error";
            //filters.Add(e);            
        }
    }
}