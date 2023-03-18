using XReports.Table;

namespace XReports.Schema
{
    internal class ReportColumn<TSourceEntity> : IReportColumn<TSourceEntity>
    {
        private readonly string title;
        private readonly IReportCellsProvider<TSourceEntity> provider;
        private readonly ReportCellProperty[] cellProperties;
        private readonly ReportCellProperty[] headerProperties;
        private readonly IReportCellProcessor<TSourceEntity>[] cellProcessors;
        private readonly IReportCellProcessor<TSourceEntity>[] headerProcessors;

        private readonly ReportCell headerCell = new ReportCell();

        public ReportColumn(
            string title,
            IReportCellsProvider<TSourceEntity> provider,
            ReportCellProperty[] cellProperties,
            ReportCellProperty[] headerProperties,
            IReportCellProcessor<TSourceEntity>[] cellProcessors,
            IReportCellProcessor<TSourceEntity>[] headerProcessors)
        {
            this.title = title;
            this.provider = provider;
            this.cellProperties = cellProperties;
            this.headerProperties = headerProperties;
            this.cellProcessors = cellProcessors;
            this.headerProcessors = headerProcessors;
        }

        public ReportCell CreateCell(TSourceEntity entity)
        {
            ReportCell cell = this.provider.GetCell(entity);

            this.AddProperties(cell);
            this.RunProcessors(cell, entity);

            return cell;
        }

        public ReportCell CreateHeaderCell()
        {
            this.headerCell.Clear();
            this.headerCell.SetValue(this.title);

            this.AddHeaderProperties(this.headerCell);
            this.RunHeaderProcessors(this.headerCell);

            return this.headerCell;
        }

        private void AddProperties(ReportCell cell)
        {
            if (this.cellProperties.Length > 0)
            {
                cell.AddProperties(this.cellProperties);
            }
        }

        private void RunProcessors(ReportCell cell, TSourceEntity entity)
        {
            for (int i = 0; i < this.cellProcessors.Length; i++)
            {
                this.cellProcessors[i].Process(cell, entity);
            }
        }

        private void AddHeaderProperties(ReportCell cell)
        {
            if (this.headerProperties.Length > 0)
            {
                cell.AddProperties(this.headerProperties);
            }
        }

        private void RunHeaderProcessors(ReportCell cell)
        {
            for (int i = 0; i < this.headerProcessors.Length; i++)
            {
                this.headerProcessors[i].Process(cell, default);
            }
        }
    }
}