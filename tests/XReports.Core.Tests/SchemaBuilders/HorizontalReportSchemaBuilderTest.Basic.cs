using System.Collections.Generic;
using System.Linq;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders
{
    public partial class HorizontalReportSchemaBuilderTest
    {
        [Fact]
        public void BuildShouldSupportMultipleRows()
        {
            HorizontalReportSchemaBuilder<(string FirstName, string LastName)> reportBuilder =
                new HorizontalReportSchemaBuilder<(string FirstName, string LastName)>();
            reportBuilder.AddRow("First name", x => x.FirstName);
            reportBuilder.AddRow("Last name", x => x.LastName);

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                ("John", "Doe"),
                ("Jane", "Do"),
            });

            table.HeaderRows.Should().BeEquivalentTo(Enumerable.Empty<IEnumerable<object>>());
            table.Rows.Should().BeEquivalentTo(new[]
            {
                new[] { "First name", "John", "Jane" },
                new[] { "Last name", "Doe", "Do" },
            });
        }
    }
}