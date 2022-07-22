using System.IO;
using System.Threading.Tasks;
using XReports.Models;

namespace XReports.Interfaces
{
    public interface IHtmlStreamCellWriter
    {
        Task WriteHeaderCellAsync(StreamWriter streamWriter, HtmlReportCell cell);

        Task WriteBodyCellAsync(StreamWriter streamWriter, HtmlReportCell cell);
    }
}