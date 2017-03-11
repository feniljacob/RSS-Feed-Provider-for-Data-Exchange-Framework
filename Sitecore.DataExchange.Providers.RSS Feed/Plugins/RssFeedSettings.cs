namespace Sitecore.DataExchange.Providers.RssFeed.Plugins
{
    public class RssFeedSettings : Sitecore.DataExchange.IPlugin
    {
        public RssFeedSettings()
        {
        }
        public string RssFeedUrl { get; set; }
        public string FeedType { get; set; }
    }
}
