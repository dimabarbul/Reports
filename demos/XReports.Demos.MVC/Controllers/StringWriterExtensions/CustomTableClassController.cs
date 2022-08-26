using System.Collections.Generic;
using System.Linq;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using XReports.Demos.MVC.XReports;
using XReports.Extensions;
using XReports.Interfaces;
using XReports.Models;
using XReports.SchemaBuilders;
using XReports.Writers;

namespace XReports.Demos.MVC.Controllers.StringWriterExtensions
{
    public class CustomTableClassController : Controller
    {
        private const int RecordsCount = 10;

        public IActionResult Index()
        {
            IReportTable<ReportCell> reportTable = this.BuildReport();
            IReportTable<HtmlReportCell> htmlReport = this.Convert(reportTable);
            HtmlStringWriter bootstrapHtmlStringWriter = new BootstrapHtmlStringWriter(new HtmlStringCellWriter());
            HtmlStringWriter htmlStringWriter = new HtmlStringWriter(new HtmlStringCellWriter());

            ViewModel vm = new ViewModel()
            {
                RegularTableHtml = htmlStringWriter.WriteToString(htmlReport),
                TableHtml = bootstrapHtmlStringWriter.WriteToString(htmlReport),
            };
            return this.View(vm);
        }

        private IReportTable<ReportCell> BuildReport()
        {
            VerticalReportSchemaBuilder<Entity> builder = new VerticalReportSchemaBuilder<Entity>();
            builder.AddColumn("First name", e => e.FirstName);
            builder.AddColumn("Last name", e => e.LastName);
            builder.AddColumn("Email", e => e.Email);
            builder.AddColumn("Age", e => e.Age);

            return builder.BuildSchema().BuildReportTable(this.GetData());
        }

        private IEnumerable<Entity> GetData()
        {
            return new Faker<Entity>()
                .RuleFor(e => e.FirstName, f => f.Name.FirstName())
                .RuleFor(e => e.LastName, f => f.Name.LastName())
                .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.FirstName, e.LastName))
                .RuleFor(e => e.Age, f => f.Random.Int(18, 57))
                .Generate(RecordsCount);
        }

        private IReportTable<HtmlReportCell> Convert(IReportTable<ReportCell> reportTable)
        {
            return new ReportConverter<HtmlReportCell>(Enumerable.Empty<IPropertyHandler<HtmlReportCell>>())
                .Convert(reportTable);
        }

        public class ViewModel
        {
            public string RegularTableHtml { get; set; }

            public string TableHtml { get; set; }
        }

        private class Entity
        {
            public string FirstName { get; set; }

            public string LastName { get; set; }

            public int Age { get; set; }

            public string Email { get; set; }
        }
    }
}
