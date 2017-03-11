using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Providers.RssFeed.Plugins;
using Sitecore.DataExchange.Providers.RssFeed.Processors.PipelineSteps;
using Xunit;

namespace Sitecore.DataExchange.Providers.RssFeed.Tests.Processors.PipelineSteps
{
    public class ReadRssFeeedStepProcessorTests
    {
        [Fact]
        public void NoEndpointSetOnPipelineStep()
        {
            var step = new PipelineStep { Enabled = true };
            var context = new PipelineContext(new PipelineBatchContext());
            var processor = new ReadRssFeedStepProcessor();
            processor.Process(step, context);
            Assert.False(context.HasIterableDataSettings());
        }
        [Fact]
        public void PathNotSetOnEndpoint()
        {
            var context = DoProcess(new RssFeedSettings());
            Assert.False(context.HasIterableDataSettings());
        }
        [Fact]
        public void PathSetOnPipelineStep()
        {
            var context = DoProcess(new RssFeedSettings { RssFeedUrl = "http://rss.weatherzone.com.au/?u=12994-1285&lt=aploc&lc=12495&obs=1&fc=1&warn=1" });
            Assert.False(context.HasIterableDataSettings());
            Assert.False(context.HasIterableDataSettings());
        }
       [Fact]
        public void RssFeedWithEndpointSet()
        {
            var context = DoProcess(new RssFeedSettings { RssFeedUrl = "http://rss.weatherzone.com.au/?u=12994-1285&lt=aploc&lc=12495&obs=1&fc=1&warn=1", FeedType = "Weather Feed"});
            Assert.False(context.HasIterableDataSettings());
            Assert.True(context.HasIterableDataSettings());
            var data =context.GetIterableDataSettings().Data;
   
        }
        private PipelineContext DoProcess(RssFeedSettings settings)
        {
            var endpoint = new Endpoint();
            endpoint.Plugins.Add(settings);
            var step = new PipelineStep { Enabled = true };
            step.Plugins.Add(new EndpointSettings { EndpointFrom = endpoint });
            //
            var processor = new ReadRssFeedStepProcessor();
            var context = new PipelineContext(new PipelineBatchContext());
            processor.Process(step, context);
            return context;
        }
      
    }
}
