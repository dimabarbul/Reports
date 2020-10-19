using System;
using System.Collections.Generic;
using FluentAssertions;
using Reports.Builders;
using Reports.Html.Interfaces;
using Reports.Html.Models;
using Reports.Html.PropertyHandlers.StandardHtml;
using Reports.Html.PropertyProcessors;
using Reports.Interfaces;
using Reports.Models;
using Reports.Models.Cells;
using Reports.Models.Properties;
using Xunit;

namespace Reports.Html.Tests
{
    public partial class HtmlReportBuilderTest
    {
        [Fact]
        public void Build_BoldProperty_StrongTag()
        {
            ReportTable table = new ReportTable
            {
                HeaderCells = new List<IEnumerable<IReportCell>>()
                {
                    new IReportCell[] { new ReportCell<string>("Value"), },
                },
                Cells = new List<IEnumerable<IReportCell>>()
                {
                    new IReportCell[] {
                        new ReportCell<string>("Test", new [] { new BoldProperty() }),
                    }
                },
            };

            HtmlReportBuilder builder = new HtmlReportBuilder();
            builder.SetPropertyProcessor(new StandardHtmlPropertyProcessor(GetPropertyHandlerFactory()));
            HtmlReportTable htmlReportTable = builder.Build(table);

            HtmlReportTableBodyCell[][] cells = this.GetBodyCellsAsArray(htmlReportTable);
            cells.Should().HaveCount(1);
            cells[0][0].Html.Should().Be("<strong>Test</strong>");
        }

        private static Func<Type, IHtmlPropertyHandler> GetPropertyHandlerFactory()
        {
            return propertyType =>
            {
                if (typeof(BoldProperty).IsAssignableFrom(propertyType))
                {
                    return new StandardHtmlBoldPropertyHandler();
                }

                return null;
            };
        }
    }
}
