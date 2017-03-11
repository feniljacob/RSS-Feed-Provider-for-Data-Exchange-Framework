using System;
using Sitecore.DataExchange.Converters.Endpoints;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.RssFeed.Models.ItemModels.Endpoints;
using Sitecore.DataExchange.Providers.RssFeed.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.RssFeed.Converters.Endpoints
{
    public class RssFeedEndpointConverter : BaseEndpointConverter<ItemModel>
    {
        private static readonly Guid TemplateId = Guid.Parse("{04C8406C-DFF7-4131-A0B4-919CE0C458BB}");
        public RssFeedEndpointConverter(IItemModelRepository repository) : base(repository)
        {
            this.SupportedTemplateIds.Add(TemplateId);
        }
        protected override void AddPlugins(ItemModel source, Endpoint endpoint)
        {
            var settings = new RssFeedSettings
            {
                RssFeedUrl = base.GetStringValue(source, RssFeedEndpointItemModel.RssFeedUrl),
                FeedType = base.GetStringValue(source, RssFeedEndpointItemModel.RssfeedName)
            };
            endpoint.Plugins.Add(settings);
        }
    }
}
