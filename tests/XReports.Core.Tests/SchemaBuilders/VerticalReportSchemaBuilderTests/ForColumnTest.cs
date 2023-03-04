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
    public class ForColumnTest
    {
        [Fact]
        public void ForColumnByTitleShouldSwitchContextToColumnWithTheTitle()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder("Column1", "Column2");

            IReportSchemaCellsProviderBuilder<int> cellsProviderBuilder = schemaBuilder.ForColumn("Column1");

            CustomProperty property = new CustomProperty();
            cellsProviderBuilder.AddHeaderProperties(property);
            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Column1", property),
                    ReportCellHelper.CreateReportCell("Column2"),
                },
            });
        }

        [Fact]
        public void ForColumnByTitleShouldThrowWhenTitleIsInDifferentCase()
        {
            const string columnName = "Column";
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder(columnName);

            Action action = () => schemaBuilder.ForColumn(columnName.ToUpperInvariant());

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void ForColumnByTitleShouldThrowWhenTitleDoesNotExist()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder("Column");

            Action action = () => schemaBuilder.ForColumn("Column1");

            action.Should().ThrowExactly<ArgumentException>();
        }

        [Fact]
        public void ForColumnByTitleShouldThrowWhenTitleIsNull()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder("Column");

            Action action = () => schemaBuilder.ForColumn((string)null);

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void ForColumnByTitleShouldSwitchContextToFirstOccurenceOfTitle()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder("Column1", "Column2", "Column1");

            IReportSchemaCellsProviderBuilder<int> cellsProviderBuilder = schemaBuilder.ForColumn("Column1");

            CustomProperty property = new CustomProperty();
            cellsProviderBuilder.AddHeaderProperties(property);
            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Column1", property),
                    ReportCellHelper.CreateReportCell("Column2"),
                    ReportCellHelper.CreateReportCell("Column1"),
                },
            });
        }

        [Fact]
        public void ForColumnByIndexShouldSwitchContextToColumnWithTheIndex()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder("Column1", "Column2");

            IReportSchemaCellsProviderBuilder<int> cellsProviderBuilder = schemaBuilder.ForColumn(0);

            CustomProperty property = new CustomProperty();
            cellsProviderBuilder.AddHeaderProperties(property);
            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Column1", property),
                    ReportCellHelper.CreateReportCell("Column2"),
                },
            });
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(2)]
        public void ForColumnByIndexShouldThrowWhenIndexIsOutOfRange(int index)
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder("Column1", "Column2");

            Action action = () => schemaBuilder.ForColumn(index);

            action.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void ForColumnByIdShouldSwitchContextToColumnWithTheId()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder();
            schemaBuilder.AddColumn(new ColumnId("1"), "Column1", new EmptyCellsProvider<int>());
            schemaBuilder.AddColumn(new ColumnId("2"), "Column2", new EmptyCellsProvider<int>());

            IReportSchemaCellsProviderBuilder<int> cellsProviderBuilder = schemaBuilder.ForColumn(new ColumnId("1"));

            CustomProperty property = new CustomProperty();
            cellsProviderBuilder.AddHeaderProperties(property);
            IReportTable<ReportCell> table = schemaBuilder.BuildSchema().BuildReportTable(Enumerable.Empty<int>());
            table.HeaderRows.Should().Equal(new[]
            {
                new[]
                {
                    ReportCellHelper.CreateReportCell("Column1", property),
                    ReportCellHelper.CreateReportCell("Column2"),
                },
            });
        }

        [Fact]
        public void ForColumnByIdShouldThrowWhenIdIsNull()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder();
            schemaBuilder.AddColumn(new ColumnId("1"), "Column1", new EmptyCellsProvider<int>());
            schemaBuilder.AddColumn(new ColumnId("2"), "Column2", new EmptyCellsProvider<int>());

            Action action = () => schemaBuilder.ForColumn((ColumnId)null);

            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void ForColumnByIdShouldThrowWhenIdDoesNotExist()
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = this.CreateSchemaBuilder();
            schemaBuilder.AddColumn(new ColumnId("1"), "Column1", new EmptyCellsProvider<int>());
            schemaBuilder.AddColumn(new ColumnId("2"), "Column2", new EmptyCellsProvider<int>());

            Action action = () => schemaBuilder.ForColumn(new ColumnId("3"));

            action.Should().ThrowExactly<ArgumentException>();
        }

        private VerticalReportSchemaBuilder<int> CreateSchemaBuilder(params string[] columns)
        {
            VerticalReportSchemaBuilder<int> schemaBuilder = new VerticalReportSchemaBuilder<int>();

            foreach (string column in columns)
            {
                schemaBuilder.AddColumn(column, new EmptyCellsProvider<int>());
            }

            return schemaBuilder;
        }

        private class CustomProperty : ReportCellProperty
        {
        }
    }
}
