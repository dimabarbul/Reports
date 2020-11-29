using System.Collections.Generic;
using Reports.Extensions.Builders.BuilderHelpers;
using Reports.Interfaces;
using Reports.Models;

namespace Reports.Demos.FromDb.Services
{
    public class ReportService
    {
        private readonly EntityAttributeBuilderHelper builderHelper;

        public ReportService(EntityAttributeBuilderHelper builderHelper)
        {
            this.builderHelper = builderHelper;
        }

        public IReportTable<ReportCell> GetReport<TEntity>(IEnumerable<TEntity> entities)
        {
            IReportSchema<TEntity> schema = this.builderHelper.BuildSchema<TEntity>();

            return schema.BuildReportTable(entities);
        }
    }
}
