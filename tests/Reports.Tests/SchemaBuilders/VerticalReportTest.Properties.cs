using FluentAssertions;
using Reports.Core.Extensions;
using Reports.Core.Interfaces;
using Reports.Core.Models;
using Reports.Core.SchemaBuilders;
using Xunit;

namespace Reports.Tests.SchemaBuilders
{
    public partial class VerticalReportTest
    {
        [Fact]
        public void Build_CustomPropertyUsingProcessor_CorrectProperties()
        {
            VerticalReportSchemaBuilder<string> reportBuilder = new VerticalReportSchemaBuilder<string>();
            reportBuilder.AddColumn("Value", s => s)
                .AddProcessors(new CustomPropertyProcessor());

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new []
            {
                "Test",
            });

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(1);
            cells[0][0].Properties.Should()
                .HaveCount(1).And
                .ContainSingle(p => p is CustomProperty && ((CustomProperty) p).Assigned);
        }

        [Fact]
        public void Build_CustomPropertyUsingAddProperty_CorrectProperties()
        {
            VerticalReportSchemaBuilder<string> reportBuilder = new VerticalReportSchemaBuilder<string>();
            reportBuilder.AddColumn("Value", s => s)
                .AddProperties(new CustomProperty());

            IReportTable<ReportCell> table = reportBuilder.BuildSchema().BuildReportTable(new []
            {
                "Test",
            });

            ReportCell[][] headerCells = this.GetCellsAsArray(table.HeaderRows);
            headerCells.Should().HaveCount(1);
            headerCells[0][0].Properties.Should().BeEmpty();

            ReportCell[][] cells = this.GetCellsAsArray(table.Rows);
            cells.Should().HaveCount(1);
            cells[0][0].Properties.Should()
                .HaveCount(1).And
                .AllBeOfType<CustomProperty>();
        }

        private class CustomPropertyProcessor : IReportCellProcessor<string>
        {
            public void Process(ReportCell cell, string data)
            {
                cell.AddProperty(new CustomProperty(true));
            }
        }

        private class CustomProperty : ReportCellProperty
        {
            public bool Assigned { get; }

            public CustomProperty(bool assigned = false)
            {
                this.Assigned = assigned;
            }
        }
    }
}
