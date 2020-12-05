using System;
using Reports.Interfaces;
using Reports.ReportCellsProviders;
using Reports.SchemaBuilders;

namespace Reports.Extensions
{
    public static class VerticalReportSchemaBuilderExtensions
    {
        public static VerticalReportSchemaBuilder<TEntity> AddColumn<TEntity, TValue>(
            this VerticalReportSchemaBuilder<TEntity> builder, string title, Func<TEntity, TValue> valueSelector)
        {
            EntityPropertyReportCellsProvider<TEntity,TValue> provider = new EntityPropertyReportCellsProvider<TEntity,TValue>(title, valueSelector);

            return builder.AddColumn(provider);
        }

        public static VerticalReportSchemaBuilder<TEntity> AddColumn<TEntity, TValue>(
            this VerticalReportSchemaBuilder<TEntity> builder, string title, IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity,TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(title, valueProvider);

            return builder.AddColumn(provider);
        }

        public static VerticalReportSchemaBuilder<TEntity> AddColumn<TEntity, TValue>(
            this VerticalReportSchemaBuilder<TEntity> builder, string title, IComputedValueProvider<TEntity, TValue> valueProvider)
        {
            ComputedValueProviderReportCellsProvider<TEntity,TValue> provider = new ComputedValueProviderReportCellsProvider<TEntity,TValue>(title, valueProvider);

            return builder.AddColumn(provider);
        }

        public static VerticalReportSchemaBuilder<TEntity> InsertColumn<TEntity, TValue>(
            this VerticalReportSchemaBuilder<TEntity> builder, int index, string title, Func<TEntity, TValue> valueSelector)
        {
            EntityPropertyReportCellsProvider<TEntity,TValue> provider = new EntityPropertyReportCellsProvider<TEntity,TValue>(title, valueSelector);

            return builder.InsertColumn(index, provider);
        }

        public static VerticalReportSchemaBuilder<TEntity> InsertColumn<TEntity, TValue>(
            this VerticalReportSchemaBuilder<TEntity> builder, int index, string title, IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity,TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(title, valueProvider);

            return builder.InsertColumn(index, provider);
        }

        public static VerticalReportSchemaBuilder<TEntity> InsertColumn<TEntity, TValue>(
            this VerticalReportSchemaBuilder<TEntity> builder, int index, string title, IComputedValueProvider<TEntity, TValue> valueProvider)
        {
            ComputedValueProviderReportCellsProvider<TEntity,TValue> provider = new ComputedValueProviderReportCellsProvider<TEntity,TValue>(title, valueProvider);

            return builder.InsertColumn(index, provider);
        }

        public static VerticalReportSchemaBuilder<TEntity> InsertColumnBefore<TEntity, TValue>(
            this VerticalReportSchemaBuilder<TEntity> builder, string beforeTitle, string title, Func<TEntity, TValue> valueSelector)
        {
            EntityPropertyReportCellsProvider<TEntity,TValue> provider = new EntityPropertyReportCellsProvider<TEntity,TValue>(title, valueSelector);

            return builder.InsertColumnBefore(beforeTitle, provider);
        }

        public static VerticalReportSchemaBuilder<TEntity> InsertColumnBefore<TEntity, TValue>(
            this VerticalReportSchemaBuilder<TEntity> builder, string beforeTitle, string title, IValueProvider<TValue> valueProvider)
        {
            ValueProviderReportCellsProvider<TEntity,TValue> provider = new ValueProviderReportCellsProvider<TEntity, TValue>(title, valueProvider);

            return builder.InsertColumnBefore(beforeTitle, provider);
        }

        public static VerticalReportSchemaBuilder<TEntity> InsertColumnBefore<TEntity, TValue>(
            this VerticalReportSchemaBuilder<TEntity> builder, string beforeTitle, string title, IComputedValueProvider<TEntity, TValue> valueProvider)
        {
            ComputedValueProviderReportCellsProvider<TEntity,TValue> provider = new ComputedValueProviderReportCellsProvider<TEntity,TValue>(title, valueProvider);

            return builder.InsertColumnBefore(beforeTitle, provider);
        }
    }
}
