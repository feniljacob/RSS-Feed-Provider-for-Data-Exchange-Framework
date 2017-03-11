using System;
using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Providers.RssFeed.Models.ItemModels.PipelineSteps;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace Sitecore.DataExchange.Providers.RssFeed.Converters.PipelineSteps
{
    public class RssFeedFileStepConverter : BasePipelineStepConverter<ItemModel>
    {
        private static readonly Guid TemplateId = Guid.Parse("{AB961AE0-8E5D-4B69-A3E9-982EB20A60B0}");
        public RssFeedFileStepConverter(IItemModelRepository repository) : base(repository)
        {
            this.SupportedTemplateIds.Add(TemplateId);
        }
        protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
        {
            AddEndpointSettings(source, pipelineStep);
        }
        private void AddEndpointSettings(ItemModel source, PipelineStep pipelineStep)
        {
            var settings = new EndpointSettings();
            var endpointFrom = base.ConvertReferenceToModel<Endpoint>(source, ReadRssFeedStepItemModel.EndpointFrom);
            if (endpointFrom != null)
            {
                settings.EndpointFrom = endpointFrom;
            }
            pipelineStep.Plugins.Add(settings);
        }
    }
}
