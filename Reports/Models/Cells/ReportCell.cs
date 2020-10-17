using System;
using Reports.Interfaces;

namespace Reports.Models.Cells
{
    public class ReportCell<TValue> : IReportCell
    {
        public Type ValueType => typeof(TValue);
        public int ColumnSpan { get; set; } = 1;
        public int RowSpan { get; set; } = 1;

        protected readonly TValue Value;
        protected IValueFormatter<TValue> Formatter;

        public ReportCell(TValue value)
        {
            this.Value = value;
        }

        public void SetFormatter(IValueFormatter<TValue> formatter)
        {
            this.Formatter = formatter;
        }

        public virtual string DisplayValue =>
            this.Formatter != null ?
                this.Formatter.Format(this.Value) :
                this.Value?.ToString() ?? string.Empty;
    }
}
