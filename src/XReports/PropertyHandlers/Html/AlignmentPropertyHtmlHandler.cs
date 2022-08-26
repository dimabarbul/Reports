using System.Collections.Generic;
using XReports.Enums;
using XReports.Models;
using XReports.Properties;

namespace XReports.PropertyHandlers.Html
{
    public class AlignmentPropertyHtmlHandler : PropertyHandler<AlignmentProperty, HtmlReportCell>
    {
        private static readonly Dictionary<Alignment, string> Alignments = new Dictionary<Alignment, string>()
        {
            [Alignment.Center] = "center",
            [Alignment.Left] = "left",
            [Alignment.Right] = "right",
        };

        protected override void HandleProperty(AlignmentProperty property, HtmlReportCell cell)
        {
            cell.Styles.Add("text-align", this.GetAlignmentString(property.Alignment));
        }

        private string GetAlignmentString(Alignment alignment)
        {
            return Alignments[alignment];
        }
    }
}
