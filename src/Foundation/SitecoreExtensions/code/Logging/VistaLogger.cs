using log4net;
using Sitecore.Diagnostics;

namespace WageWorks.Foundation.SitecoreExtensions.Logging
{
    public class WageWorksLogger
    {
        public static ILog Log => LogManager.GetLogger("WageWorksLogger") ?? LoggerFactory.GetLogger(typeof(WageWorksLogger));
    }
}