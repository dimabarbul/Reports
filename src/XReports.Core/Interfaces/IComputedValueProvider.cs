namespace XReports.Interfaces
{
    public interface IComputedValueProvider<in TSourceEntity, out TValue>
    {
        TValue GetValue(TSourceEntity entity);
    }
}