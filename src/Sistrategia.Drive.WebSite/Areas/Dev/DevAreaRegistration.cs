using System.Web.Mvc;
using Sistrategia.Drive.WebSite.Utils;

namespace Sistrategia.Drive.WebSite.Areas.Dev
{
    public class DevAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Dev";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapLocalizeRoute(
                "Dev_default",
                "{culture}/Dev/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                , constraints: new { culture = "[a-zA-Z]{2}-[a-zA-Z]{2}" }
            );
        }
    }
}