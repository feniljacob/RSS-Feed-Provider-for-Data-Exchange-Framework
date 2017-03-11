using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading;
using System.Xml;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.DataExchange.Providers.RssFeed.Extensions;
using Sitecore.DataExchange.Providers.RssFeed.Models.RSSFeed;
using Sitecore.DataExchange.Providers.RssFeed.Plugins;

namespace Sitecore.DataExchange.Providers.RssFeed.Processors.PipelineSteps
{
    [RequiredEndpointPlugins(typeof(RssFeedSettings))]
    public class ReadRssFeedStepProcessor : BaseReadDataStepProcessor
    {
        public ReadRssFeedStepProcessor()
        {
        }
        public override bool CanProcess(PipelineStep pipelineStep, PipelineContext pipelineContext)
        {
            return base.CanProcess(pipelineStep, pipelineContext);
        }
        public override void Process(PipelineStep pipelineStep, PipelineContext pipelineContext)
        {
            base.Process(pipelineStep, pipelineContext);
        }
        protected override void ReadData(Endpoint endpoint, PipelineStep pipelineStep, PipelineContext pipelineContext)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException(nameof(endpoint));
            }
            if (pipelineStep == null)
            {
                throw new ArgumentNullException(nameof(pipelineStep));
            }
            if (pipelineContext == null)
            {
                throw new ArgumentNullException(nameof(pipelineContext));
            }
            var logger = pipelineContext.PipelineBatchContext.Logger;
            //
            //get the file path from the plugin on the endpoint
            var settings = endpoint.GetTextFileSettings();
            if (settings == null)
            {
                logger.Error("No Rss Feed  settings are specified on the endpoint. (pipeline step: {0}, endpoint: {1})", pipelineStep.Name, endpoint.Name);
                return;
            }
            if (string.IsNullOrWhiteSpace(settings.RssFeedUrl))
            {
                logger.Error("No Rss Feed is specified on the endpoint. (pipeline step: {0}, endpoint: {1})", pipelineStep.Name, endpoint.Name);
                return;
            }
            //
            var rssFeedUrl = settings.RssFeedUrl;
            Uri uriResult;
            if (!(Uri.TryCreate(rssFeedUrl, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)))
            {
                logger.Error("Please specify a valid feed URL. (pipeline step: {0}, endpoint: {1}, path: {2})", pipelineStep.Name, endpoint.Name, rssFeedUrl);
                return;
            }
            try
            {
                FeedData feedData = new FeedData();
                var feedlines = new List<string[]>();
                using (var reader = XmlReader.Create(rssFeedUrl))
                {
                    var rssFeed = SyndicationFeed.Load(reader);
                    if (rssFeed != null)
                    {
                       
                        if (rssFeed.Items.Any())
                        {
                            var dataSettings = new IterableDataSettings(rssFeed.Items);
                            logger.Info(
                                "{0} rows were read from RSS Feed{1}. (pipeline step: {2}, endpoint: {3})",
                                rssFeed.Items.Count(), rssFeedUrl,
                                pipelineStep.Name, endpoint.Name);
                            //add the plugin to the pipeline context
                            pipelineContext.Plugins.Add(dataSettings);
                        }
                       
                    }
                  
                }
            }
            catch (Exception ex)
            {
                logger.Error("Can't read the feed. (pipeline step: {0}, endpoint: {1}, path: {2}), exception:{3}", pipelineStep.Name,
                    endpoint.Name, rssFeedUrl, ex.ToString());
            }
        }
    }
}
