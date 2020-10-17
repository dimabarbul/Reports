namespace Reports.Models.Cells
{
    public class HeaderReportCell : ReportCell<string>
    {
        public HeaderReportCell(string text)
        {
            this.SetValue(text);
        }
    }
}
