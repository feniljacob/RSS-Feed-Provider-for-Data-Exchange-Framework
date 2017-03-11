using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.RssFeed.Extensions;
using Sitecore.DataExchange.Providers.RssFeed.Plugins;
using Xunit;

namespace Sitecore.DataExchange.Providers.RssFeed.Tests.Extensions
{
    public class EndpointExtensionsTests
    {
        [Fact]
        public void RssFeedSettingsIsNotSet()
        {
            var endpoint = new Endpoint();
            Assert.Null(endpoint.GetPlugin<RssFeedSettings>());
            Assert.Null(endpoint.GetTextFileSettings());
            Assert.False(endpoint.HasTextFileSettings());
        }
        [Fact]
        public void RssFeedSettingsIsSet()
        {
            var endpoint = new Endpoint();
            var plugin = new RssFeedSettings();
            endpoint.Plugins.Add(plugin);
            Assert.NotNull(endpoint.GetPlugin<RssFeedSettings>());
            Assert.NotNull(endpoint.GetTextFileSettings());
            Assert.Same(plugin, endpoint.GetTextFileSettings());
            Assert.True(endpoint.HasTextFileSettings());
        }
    }
}
