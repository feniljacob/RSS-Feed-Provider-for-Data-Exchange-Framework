﻿using System;
using System.Linq;
using NSubstitute;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Providers.RssFeed.Converters.DataAccess;
using Sitecore.DataExchange.Providers.RssFeed.Models.ItemModels.DataAccess;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;
using Xunit;

namespace Sitecore.DataExchange.Providers.RssFeed.Tests.Converters.DataAccess
{
    public class ArrayValueAccessorConverterTests
    {
        [Fact]
        public void CannotConvertNullItemModel()
        {
            var itemModelRepo = Substitute.For<IItemModelRepository>();
            var converter = new RssFeedFieldValueAccessorConverter(itemModelRepo);
            Assert.False(converter.CanConvert(null));
            Assert.Null(converter.Convert(null));
        }
        [Fact]
        public void CannotConvertWhenTemplateIsNotSupported()
        {
            var itemModelRepo = Substitute.For<IItemModelRepository>();
            var converter = new RssFeedFieldValueAccessorConverter(itemModelRepo);
            var itemModel = new ItemModel();
            itemModel[ItemModel.TemplateID] = Guid.Empty;
            Assert.False(converter.CanConvert(itemModel));
            Assert.Null(converter.Convert(itemModel));
        }
        [Fact]
        public void CannotConvertWhenNoPositionValueIsSet()
        {
            var itemModelRepo = Substitute.For<IItemModelRepository>();
            var converter = new RssFeedFieldValueAccessorConverter(itemModelRepo);
            var itemModel = new ItemModel();
            itemModel[ItemModel.TemplateID] = converter.SupportedTemplateIds.FirstOrDefault();
            Assert.True(converter.CanConvert(itemModel));
            Assert.Null(converter.Convert(itemModel));
        }
        [Fact]
        public void CannotConvertWhenPositionValueIsLessThanZero()
        {
            var itemModelRepo = Substitute.For<IItemModelRepository>();
            var converter = new RssFeedFieldValueAccessorConverter(itemModelRepo);
            var itemModel = new ItemModel();
            itemModel[ItemModel.TemplateID] = converter.SupportedTemplateIds.FirstOrDefault();
            itemModel[RssFeedFieldValueValueAccessorItemModel.RssFeedFieldName] = "1";
            Assert.True(converter.CanConvert(itemModel));
            Assert.Null(converter.Convert(itemModel));
        }
        [Fact]
        public void CannotConvertWhenPositionValueIsZero()
        {
            var itemModelRepo = Substitute.For<IItemModelRepository>();
            var converter = new RssFeedFieldValueAccessorConverter(itemModelRepo);
            var itemModel = new ItemModel();
            itemModel[ItemModel.TemplateID] = converter.SupportedTemplateIds.FirstOrDefault();
            itemModel[RssFeedFieldValueValueAccessorItemModel.RssFeedFieldName] = "2";
            Assert.True(converter.CanConvert(itemModel));
            Assert.Null(converter.Convert(itemModel));
        }
        [Fact]
        public void CanConvert()
        {
            var itemModelRepo = Substitute.For<IItemModelRepository>();
            var converter = new RssFeedFieldValueAccessorConverter(itemModelRepo);
            var itemModel = new ItemModel();
            itemModel[ItemModel.TemplateID] = converter.SupportedTemplateIds.FirstOrDefault();
            itemModel[RssFeedFieldValueValueAccessorItemModel.RssFeedFieldName] = 1;
            Assert.True(converter.CanConvert(itemModel));
            var accessor = converter.Convert(itemModel);
            Assert.NotNull(accessor);
            //
            //test reader
            var reader = accessor.ValueReader;
            Assert.NotNull(reader);
            var names = new string[] { "VALUE 1", "VALUE 2", "VALUE 3" };
            var valueFromReader = reader.Read(names, new DataAccessContext()).ReadValue;
            Assert.Same(names[0], valueFromReader);
            //
            //test writer
            var writer = accessor.ValueWriter;
            Assert.NotNull(writer);
            writer.Write(names, "NEW VALUE", new DataAccessContext());
            Assert.Same("NEW VALUE", names[0]);
        }
    }
}
