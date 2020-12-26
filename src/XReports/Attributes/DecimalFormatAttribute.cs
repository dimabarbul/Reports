namespace XReports.Attributes
{
    public class DecimalFormatAttribute : AttributeBase
    {
        public int Precision { get; }

        public DecimalFormatAttribute(int precision)
        {
            this.Precision = precision;
        }
    }
}