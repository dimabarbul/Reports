using System;
using FluentAssertions;
using Reports.Extensions.AttributeBasedBuilder.AttributeHandlers;
using Reports.Extensions.AttributeBasedBuilder.Attributes;
using Reports.Interfaces;
using Reports.Models;
using Reports.SchemaBuilders;
using Xunit;

namespace Reports.Extensions.Builders.Tests.BuilderHelpers
{
    public partial class EntityAttributeBuilderHelperTest
    {
        [Fact]
        public void BuildVerticalReport_CustomAttributeHandler_Applied()
        {
            AttributeBasedBuilder.AttributeBasedBuilder helper = new AttributeBasedBuilder.AttributeBasedBuilder();
            helper.AddAttributeHandler(new CustomAttributeHandler());

            IReportSchema<EntityWithCustomAttribute> schema = helper.BuildSchema<EntityWithCustomAttribute>();

            IReportTable<ReportCell> reportTable = schema.BuildReportTable(new[]
            {
                new EntityWithCustomAttribute() { Title = "Test" },
            });

            ReportCell[][] cells = this.GetCellsAsArray(reportTable.Rows);

            cells[0][0].Properties.Should().HaveCount(1)
                .And.ContainItemsAssignableTo<CustomProperty>();
        }

        private class EntityWithCustomAttribute
        {
            [ReportVariable(1, "Title")]
            [Custom]
            public string Title { get; set; }
        }

        private class CustomAttribute : Attribute
        {
        }

        private class CustomProperty : ReportCellProperty
        {
        }

        private class CustomAttributeHandler : AttributeHandler<CustomAttribute>
        {
            protected override void HandleAttribute<TSourceEntity>(ReportSchemaBuilder<TSourceEntity> builder, CustomAttribute attribute)
            {
                builder.AddProperties(new CustomProperty());
            }
        }
    }
}
