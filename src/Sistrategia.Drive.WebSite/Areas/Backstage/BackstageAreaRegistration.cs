using System.Web.Mvc;
using Sistrategia.Drive.WebSite.Utils;

namespace Sistrategia.Drive.WebSite.Areas.Backstage
{
    public class BackstageAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Backstage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Backstage_default",
                "{culture}/Backstage/{controller}/{action}/{id}",
                new { controller = "Inbox", action = "Index", id = UrlParameter.Optional }
                , constraints: new { culture = "[a-zA-Z]{2}-[a-zA-Z]{2}" }
            );
        }
    }
}