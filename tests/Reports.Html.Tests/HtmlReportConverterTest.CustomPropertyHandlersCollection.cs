using System.Collections.Generic;
using FluentAssertions;
using Reports.Core;
using Reports.Core.Enums;
using Reports.Core.Interfaces;
using Reports.Core.Models;
using Reports.Core.PropertyHandlers;
using Xunit;

namespace Reports.Html.Tests
{
    public partial class HtmlReportConverterTest
    {
        [Fact]
        public void Build_SeveralHandlers_BothApplied()
        {
            ReportTable<ReportCell> table = new ReportTable<ReportCell>()
            {
                HeaderRows = new List<IEnumerable<ReportCell>>()
                {
                    new ReportCell[] { new ReportCell<string>("Value"), },
                },
                Rows = new List<IEnumerable<ReportCell>>()
                {
                    new ReportCell[] {
                        this.CreateReportCell("Test", new BoldProperty()),
                    }
                }
            };

            ReportConverter<HtmlReportCell> converter = new ReportConverter<HtmlReportCell>(GetCustomPropertyHandlers());
            IReportTable<HtmlReportCell> htmlReportTable = converter.Convert(table);

            HtmlReportCell[][] cells = this.GetBodyCellsAsArray(htmlReportTable);
            cells.Should().HaveCount(1);
            cells[0][0].Html.Should().Be("bold: TEST");
        }

        private static IEnumerable<IPropertyHandler<HtmlReportCell>> GetCustomPropertyHandlers()
        {
            return new IPropertyHandler<HtmlReportCell>[]
            {
                new BoldToUpperHandler(),
                new BoldMarkedWithTextHandler(),
            };
        }

        private class BoldProperty : ReportCellProperty
        {
        }

        private class BoldToUpperHandler : PropertyHandler<BoldProperty, HtmlReportCell>
        {
            protected override void HandleProperty(BoldProperty property, HtmlReportCell cell)
            {
                cell.Html = cell.Html.ToUpperInvariant();
            }

            public override int Priority => (int) HtmlPropertyHandlerPriority.Text;
        }

        private class BoldMarkedWithTextHandler : PropertyHandler<BoldProperty, HtmlReportCell>
        {
            protected override void HandleProperty(BoldProperty property, HtmlReportCell cell)
            {
                cell.Html = "bold: " + cell.Html;
            }

            public override int Priority => (int) HtmlPropertyHandlerPriority.Text;
        }
    }
}
