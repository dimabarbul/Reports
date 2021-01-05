using System.Linq;
using XReports.Models;
using XReports.Properties;

namespace XReports.PropertyHandlers.Excel
{
    public class PercentFormatPropertyExcelHandler : PropertyHandler<PercentFormatProperty, ExcelReportCell>
    {
        protected override void HandleProperty(PercentFormatProperty property, ExcelReportCell cell)
        {
            if (!(property.PostfixText ?? string.Empty).Contains('%'))
            {
                cell.Value = cell.GetNullableValue<decimal>() * 100;
            }

            cell.NumberFormat = $"0.{string.Concat(Enumerable.Repeat('0', property.Precision))}{property.PostfixText}";
        }
    }
}
