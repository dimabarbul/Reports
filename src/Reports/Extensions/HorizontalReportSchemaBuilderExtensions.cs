using System;
using Reports.Interfaces;
using Reports.ReportCellsProviders;
using Reports.SchemaBuilders;

namespace Reports.Extensions
{
    public static class HorizontalReportSchemaBuilderExtensions
    {
        public static HorizontalReportSchemaBuilder<TEntity> AddRow<TEntity, TValue>(
            this HorizontalReportSchemaBuilder<TEntity> builder, string title, Func<TEntity, TValue> valueSelector)
        {
            EntityPropertyReportCellsProvider<TEntity, TValue> provider = new EntityPropertyReportCellsProvider<TEntity,TValue>(title, valueSelector);

            return builder.AddRow(provider);
        }

        public static HorizontalReportSchemaBuilder<TEntity> AddRow<TEntity, TValue>(
            this HorizontalReportSchemaBuilder<TEntity> builder, string title, IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity,TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(title, valueProvider);

            return builder.AddRow(provider);
        }

        public static HorizontalReportSchemaBuilder<TEntity> AddRow<TEntity, TValue>(
            this HorizontalReportSchemaBuilder<TEntity> builder, string title, IComputedValueProvider<TEntity, TValue> valueProvider)
        {
            ComputedValueProviderReportCellsProvider<TEntity,TValue> provider = new ComputedValueProviderReportCellsProvider<TEntity,TValue>(title, valueProvider);

            return builder.AddRow(provider);
        }

        public static HorizontalReportSchemaBuilder<TEntity> AddHeaderRow<TEntity, TValue>(
            this HorizontalReportSchemaBuilder<TEntity> builder, int rowIndex, string title, Func<TEntity, TValue> valueSelector)
        {
            EntityPropertyReportCellsProvider<TEntity, TValue> provider =
                new EntityPropertyReportCellsProvider<TEntity, TValue>(title, valueSelector);

            return builder.AddHeaderRow(rowIndex, provider);
        }

        public static HorizontalReportSchemaBuilder<TEntity> AddHeaderRow<TEntity, TValue>(
            this HorizontalReportSchemaBuilder<TEntity> builder, int rowIndex, string title, IComputedValueProvider<TEntity, TValue> valueProvider)
        {
            ComputedValueProviderReportCellsProvider<TEntity,TValue> provider = new ComputedValueProviderReportCellsProvider<TEntity,TValue>(title, valueProvider);

            return builder.AddHeaderRow(rowIndex, provider);
        }
    }
}