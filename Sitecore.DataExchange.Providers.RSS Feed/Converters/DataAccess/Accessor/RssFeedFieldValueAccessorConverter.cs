using System;
using Sitecore.DataExchange.Converters.DataAccess.ValueAccessors;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.DataAccess.Readers;
using Sitecore.DataExchange.DataAccess.Writers;
using Sitecore.DataExchange.Providers.RssFeed.Converters.DataAccess.Reader.Sitecore.DataExchange.Providers.DynamicsCrm.DataAccess.Readers;
using Sitecore.DataExchange.Providers.RssFeed.Models.ItemModels.DataAccess;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.RssFeed.Converters.DataAccess
{
    public class RssFeedFieldValueAccessorConverter : ValueAccessorConverter
    {
        private static readonly Guid TemplateId = Guid.Parse("{EEC71046-8139-4F1F-AAD2-2CD6923E895C}");
        public RssFeedFieldValueAccessorConverter(IItemModelRepository repository) : base(repository)
        {
            this.SupportedTemplateIds.Add(TemplateId);
        }
        public override IValueAccessor Convert(ItemModel source)
        {
            var accessor = base.Convert(source);
            if (accessor == null)
            {
                return null;
            }
            var fieldName = base.GetStringValue(source, RssFeedFieldValueValueAccessorItemModel.RssFeedFieldName);
            if (String.IsNullOrEmpty(fieldName))
            {
                return null;
            }
            if (accessor.ValueReader == null)
            {
                accessor.ValueReader = new RssFeedFieldValueReader(fieldName);
            }
            if (accessor.ValueWriter == null)
            {
                accessor.ValueWriter = new PropertyValueWriter(fieldName);
            }
            return accessor;
        }

    }
}
