using System.Collections.Generic;
using Reports.Interfaces;
using Reports.Models;
using Reports.SchemaBuilders;

namespace Reports.Demos.FromDb.Services
{
    public class ReportService
    {
        private readonly AttributeBasedBuilder builderHelper;

        public ReportService(AttributeBasedBuilder builderHelper)
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
