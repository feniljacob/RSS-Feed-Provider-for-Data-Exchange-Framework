using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.RssFeed.Plugins;

namespace Sitecore.DataExchange.Providers.RssFeed.Extensions
{
    public static class EndpointExtensions
    {
        public static string secretkey = "secretkey";
        public static RssFeedSettings GetTextFileSettings(this Endpoint endpoint)
        {
            return endpoint.GetPlugin<RssFeedSettings>();
        }
        public static bool HasTextFileSettings(this Endpoint endpoint)
        {
            return (GetTextFileSettings(endpoint) != null);
        }
    }
}
