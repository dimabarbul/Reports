using System;
using FluentAssertions;
using XReports.Models;
using XReports.ReportCellsProviders;
using XReports.ReportSchemaCellsProviders;
using XReports.Tests.Common.Assertions;
using XReports.Tests.Common.Helpers;
using Xunit;

namespace XReports.Core.Tests.ReportSchemaCellsProviders.ReportSchemaCellsProviderBuilderTests
{
    public class AddHeaderPropertiesTest
    {
        [Fact]
        public void AddHeaderPropertiesShouldAddCustomHeaderProperties()
        {
            ReportSchemaCellsProviderBuilder<string> builder = new ReportSchemaCellsProviderBuilder<string>(
                "Value", new ComputedValueReportCellsProvider<string, string>(s => s));

            builder.AddHeaderProperties(new CustomHeaderProperty1(), new CustomHeaderProperty2());

            ReportSchemaCellsProvider<string> provider = builder.Build(Array.Empty<ReportCellProperty>());
            ReportCell headerCell = provider.CreateHeaderCell();
            headerCell.Should().Equal(ReportCellHelper.CreateReportCell(
                "Value", new CustomHeaderProperty1(), new CustomHeaderProperty2()));
        }

        [Fact]
        public void AddHeaderPropertiesShouldNotThrowWhenPropertyOfSameTypeAddedMultipleTimes()
        {
            ReportSchemaCellsProviderBuilder<string> builder = new ReportSchemaCellsProviderBuilder<string>(
                "Value", new ComputedValueReportCellsProvider<string, string>(s => s));

            builder.AddHeaderProperties(new CustomHeaderProperty1(), new CustomHeaderProperty1());

            ReportSchemaCellsProvider<string> provider = builder.Build(Array.Empty<ReportCellProperty>());
            ReportCell headerCell = provider.CreateHeaderCell();
            headerCell.Should().Equal(ReportCellHelper.CreateReportCell(
                "Value", new CustomHeaderProperty1(), new CustomHeaderProperty1()));
        }

        [Fact]
        public void AddHeaderPropertiesShouldThrowWhenSomePropertyIsNull()
        {
            ReportSchemaCellsProviderBuilder<string> builder = new ReportSchemaCellsProviderBuilder<string>(
                "Value", new ComputedValueReportCellsProvider<string, string>(s => s));

            Action action = () => builder.AddHeaderProperties(
                new CustomHeaderProperty1(), new CustomHeaderProperty2(), null);

            action.Should().ThrowExactly<ArgumentException>();
        }

        private class CustomHeaderProperty1 : ReportCellProperty
        {
        }

        private class CustomHeaderProperty2 : ReportCellProperty
        {
        }
    }
}
