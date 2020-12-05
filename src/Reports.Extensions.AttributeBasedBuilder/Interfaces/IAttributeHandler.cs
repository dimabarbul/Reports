using System;
using Reports.Core.SchemaBuilders;

namespace Reports.Extensions.AttributeBasedBuilder.Interfaces
{
    public interface IAttributeHandler
    {
        void Handle<TSourceEntity>(ReportSchemaBuilder<TSourceEntity> builder, Attribute attribute);
    }
}
