using System;
using XReports.Schema;
using XReports.SchemaBuilders;
using XReports.SchemaBuilders.ReportCellProviders;
using XReports.Table;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Core.Tests.SchemaBuilders.ReportColumnBuilderTests
{
    public class AddDynamicPropertiesTest
    {
        [Fact]
        public void AddDynamicPropertiesShouldAddProperties()
        {
            ReportColumnBuilder<int> builder = new ReportColumnBuilder<int>(
                "Column", new ComputedValueReportCellProvider<int, int>(x => x));

            builder.AddDynamicProperties(x => x > 0 ? (IReportCellProperty)new CustomProperty2() : new CustomProperty1());

            IReportColumn<int> provider = builder.Build(Array.Empty<IReportCellProperty>(), Array.Empty<IReportCellProcessor<int>>());
            ReportCell headerCell = provider.CreateHeaderCell();
            ReportCell zeroCell = provider.CreateCell(0).Clone();
            ReportCell oneCell = provider.CreateCell(1).Clone();

            headerCell.Should().Equal(ReportCellHelper.CreateReportCell("Column"));
            zeroCell.Should().Equal(ReportCellHelper.CreateReportCell(0, new CustomProperty1()));
            oneCell.Should().Equal(ReportCellHelper.CreateReportCell(1, new CustomProperty2()));
        }

        [Fact]
        public void AddDynamicPropertiesShouldIgnoreNulls()
        {
            ReportColumnBuilder<int> builder = new ReportColumnBuilder<int>("Column", new ComputedValueReportCellProvider<int, int>(x => x));

            builder.AddDynamicProperties(_ => new IReportCellProperty[]
            {
                new CustomProperty1(),
                new CustomProperty2(),
                null,
            });

            IReportColumn<int> provider = builder.Build(Array.Empty<IReportCellProperty>(), Array.Empty<IReportCellProcessor<int>>());
            ReportCell cell = provider.CreateCell(0);

            cell.Should().Equal(ReportCellHelper.CreateReportCell(0, new CustomProperty1(), new CustomProperty2()));
        }

        private class CustomProperty1 : IReportCellProperty
        {
        }

        private class CustomProperty2 : IReportCellProperty
        {
        }
    }
}
