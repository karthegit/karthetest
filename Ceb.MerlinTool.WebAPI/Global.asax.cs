using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Ceb.MerlinTool.WebAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.Culture = new CultureInfo(string.Empty)
            {
                NumberFormat = new NumberFormatInfo
                {
                    CurrencyDecimalDigits = 5
                }
            };
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
