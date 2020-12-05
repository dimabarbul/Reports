using FluentAssertions;
using Reports.Core.Extensions;
using Reports.Core.Interfaces;
using Reports.Core.Models;
using Reports.Core.SchemaBuilders;
using Reports.Core.ValueProviders;
using Xunit;

namespace Reports.Tests.SchemaBuilders
{
    public partial class VerticalReportTest
    {
        [Fact]
        public void Build_SequentialNumberValueProviderWithDefaultStartValue_CorrectValues()
        {
            VerticalReportSchemaBuilder<string> reportBuilder = new VerticalReportSchemaBuilder<string>();
            reportBuilder.AddColumn("#", new SequentialNumberValueProvider());

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                "John Doe",
                "Jane Doe",
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(1);
            headerCells[0][0].GetValue<string>().Should().Be("#");

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(2);
            cells[0][0].GetValue<int>().Should().Be(1);
            cells[1][0].GetValue<int>().Should().Be(2);
        }

        [Fact]
        public void Build_SequentialNumberValueProviderWithNonDefaultStartValue_CorrectValues()
        {
            VerticalReportSchemaBuilder<string> reportBuilder = new VerticalReportSchemaBuilder<string>();
            reportBuilder.AddColumn("#", new SequentialNumberValueProvider(15));

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new[]
            {
                "John Doe",
                "Jane Doe",
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(1);
            headerCells[0][0].GetValue<string>().Should().Be("#");

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(2);
            cells[0][0].GetValue<int>().Should().Be(15);
            cells[1][0].GetValue<int>().Should().Be(16);
        }
    }
}
