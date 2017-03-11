using System;
using System.Linq;
using NSubstitute;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Providers.RssFeed.Converters.Endpoints;
using Sitecore.DataExchange.Providers.RssFeed.Extensions;
using Sitecore.DataExchange.Providers.RssFeed.Models.ItemModels.Endpoints;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using Xunit;

namespace Sitecore.DataExchange.Providers.RssFeed.Tests.Converters.Endpoints
{
    public class RssFeedEndpointConverterTests
    {
        [Fact]
        public void CannotConvertNullItemModel()
        {
            var itemModelRepo = Substitute.For<IItemModelRepository>();
            var converter = new RssFeedEndpointConverter(itemModelRepo);
            Assert.False(converter.CanConvert(null));
            Assert.Null(converter.Convert(null));
        }
        [Fact]
        public void CannotConvertWhenTemplateIsNotSupported()
        {
            var itemModelRepo = Substitute.For<IItemModelRepository>();
            var converter = new RssFeedEndpointConverter(itemModelRepo);
            var itemModel = new ItemModel();
            itemModel[ItemModel.TemplateID] = Guid.Empty;
            Assert.False(converter.CanConvert(itemModel));
            Assert.Null(converter.Convert(itemModel));
        }
        [Fact]
        public void CanConvert()
        {
            var itemModelRepo = Substitute.For<IItemModelRepository>();
            var converter = new RssFeedEndpointConverter(itemModelRepo);
            var itemModel = new ItemModel();
            itemModel[ItemModel.TemplateID] = converter.SupportedTemplateIds.FirstOrDefault();
            itemModel[RssFeedEndpointItemModel.RssfeedName] = "COLUMN-SEPARATOR";
            itemModel[RssFeedEndpointItemModel.RssFeedUrl] = "PATH-VALUE";
            Assert.True(converter.CanConvert(itemModel));
            var endpoint = converter.Convert(itemModel);
            Assert.NotNull(endpoint);
            Assert.IsAssignableFrom<Endpoint>(endpoint);
            var settings = endpoint.GetTextFileSettings();
            Assert.NotNull(settings);
            Assert.Same("COLUMN-SEPARATOR", settings.FeedType);
            Assert.Same("PATH-VALUE", settings.RssFeedUrl);
        }
    }
}
