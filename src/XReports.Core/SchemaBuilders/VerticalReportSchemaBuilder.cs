using System.Linq;
using XReports.Interfaces;
using XReports.Models;

namespace XReports.SchemaBuilders
{
    public class VerticalReportSchemaBuilder<TSourceEntity> : ReportSchemaBuilder<TSourceEntity>
    {
        public VerticalReportSchemaBuilder<TSourceEntity> AddColumn(IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertColumn(this.CellsProviders.Count, provider);
        }

        public VerticalReportSchemaBuilder<TSourceEntity> InsertColumn(int index, IReportCellsProvider<TSourceEntity> provider)
        {
            this.InsertCellsProvider(index, provider);

            return this;
        }

        public VerticalReportSchemaBuilder<TSourceEntity> InsertColumnBefore(string title, IReportCellsProvider<TSourceEntity> provider)
        {
            return this.InsertColumn(this.GetCellsProviderIndex(title), provider);
        }

        public VerticalReportSchemaBuilder<TSourceEntity> ForColumn(string title)
        {
            this.SelectProvider(title);

            return this;
        }

        public VerticalReportSchema<TSourceEntity> BuildSchema()
        {
            return ReportSchema<TSourceEntity>.CreateVertical(
                this.CellsProviders
                    .Select(
                        c => new ReportSchemaCellsProvider<TSourceEntity>(
                            c.Provider,
                            c.CellProperties.ToArray(),
                            c.HeaderProperties.ToArray(),
                            c.CellProcessors.ToArray(),
                            c.HeaderProcessors.ToArray()))
                    .ToArray(),
                this.TableProperties.ToArray(),
                this.ComplexHeaders.ToArray(),
                this.ComplexHeadersProperties
                    .ToDictionary(x => x.Key, x => x.Value.ToArray()),
                this.CommonComplexHeadersProperties.ToArray());
        }
    }
}
