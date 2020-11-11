using FluentAssertions;
using Reports.Builders;
using Reports.Extensions;
using Reports.Interfaces;
using Reports.Models;
using Reports.Models.Properties;
using Xunit;

namespace Reports.Tests.Builders
{
    public partial class VerticalReportTest
    {
        [Fact]
        public void Build_CustomPropertyUsingProcessor_CorrectProperties()
        {
            VerticalReportBuilder<string> reportBuilder = new VerticalReportBuilder<string>();
            reportBuilder.AddColumn("Value", s => s)
                .AddProcessor(new CustomPropertyProcessor());

            IReportTable<ReportCell> table = reportBuilder.Build(new []
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
            VerticalReportBuilder<string> reportBuilder = new VerticalReportBuilder<string>();
            reportBuilder.AddColumn("Value", s => s)
                .AddProperty(new BoldProperty());

            IReportTable<ReportCell> table = reportBuilder.Build(new []
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
                .AllBeOfType<BoldProperty>();
        }

        private class CustomPropertyProcessor : IReportCellProcessor
        {
            public void Process(ReportCell cell)
            {
                cell.AddProperty(new CustomProperty(true));
            }
        }

        private class CustomProperty : IReportCellProperty
        {
            public bool Assigned { get; }

            public CustomProperty(bool assigned)
            {
                this.Assigned = assigned;
            }
        }
    }
}