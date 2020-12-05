using Reports.Core.Models;
using Reports.Core.PropertyHandlers;

namespace Reports.Extensions.Properties.PropertyHandlers.Excel
{
    public class MaxLengthPropertyExcelHandler : PropertyHandler<MaxLengthProperty, ExcelReportCell>
    {
        protected override void HandleProperty(MaxLengthProperty property, ExcelReportCell cell)
        {
            string text = cell.GetValue<string>();

            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            if (text.Length <= property.MaxLength)
            {
                return;
            }

            cell.InternalValue = text.Substring(0, property.MaxLength - 1) + "…";
        }
    }
}
