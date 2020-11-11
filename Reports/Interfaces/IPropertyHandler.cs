namespace Reports.Interfaces
{
    public interface IPropertyHandler<in TResultReportCell>
    {
        int Priority { get; }
        void Handle(IReportCellProperty property, TResultReportCell cell);
    }
}