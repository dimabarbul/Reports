using System;
using System.Linq;
using FluentAssertions;
using XReports.Interfaces;
using XReports.Models;
using XReports.ReportCellsProviders;
using XReports.SchemaBuilders;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.VerticalReportSchemaBuilderTests
{
    public class AddColumnTest
    {
        [Fact]
        public void AddColumnShouldAddColumnAtEnd()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);
            const string columnName = "TheColumn";

            schemaBuilder.AddColumn(columnName, new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Column1"),
                    ReportCellHelper.CreateReportCell("Column2"),
                    ReportCellHelper.CreateReportCell(columnName),
                },
            });
        }

        [Fact]
        public void AddColumnShouldAddColumnWithExistingTitle()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            schemaBuilder.AddColumn("Column1", new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Column1"),
                    ReportCellHelper.CreateReportCell("Column2"),
                    ReportCellHelper.CreateReportCell("Column1"),
                },
            });
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void AddColumnShouldAddColumnWithEmptyTitle(string title)
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);

            schemaBuilder.AddColumn(title, new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell(title),
                },
            });
        }

        [Fact]
        public void AddColumnShouldThrowWhenTitleIsNull()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(0);

            Action action = () => schemaBuilder.AddColumn(null, new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void AddColumnShouldAddColumnWithId()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);
            ColumnId id = new ColumnId("Column");
            const string columnName = "TheColumn";

            schemaBuilder.AddColumn(id, columnName, new EmptyCellsProvider<int>());

            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Column1"),
                    ReportCellHelper.CreateReportCell("Column2"),
                    ReportCellHelper.CreateReportCell(columnName),
                },
            });
        }

        [Fact]
        public void AddColumnShouldThrowWhenIdIsNull()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(2);

            Action action = () => schemaBuilder.AddColumn(null, "TheColumn", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void AddColumnShouldThrowWhenIdExists()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = new VerticalReportSchemaBuilder<int>();
            ColumnId id = new ColumnId("Column");
            schemaBuilder.AddColumn(id, "Column1", new EmptyCellsProvider<int>());

            Action action = () => schemaBuilder.AddColumn(id, "Column2", new EmptyCellsProvider<int>());

            action.Should().ThrowExactly<ArgumentException>();
        }

        private VerticalReportSchemaBuilder<int> CreateSchemaBuilder(int columnsCount)
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = new VerticalReportSchemaBuilder<int>();

            for (int i = 0; i < columnsCount; i++)
            {
                schemaBuilder.AddColumn($"Column{i + 1}", new EmptyCellsProvider<int>());
            }

            return schemaBuilder;
        }
    }
}
