using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Bogus;
using Bogus.Extensions;
using Microsoft.AspNetCore.Mvc;
using Reports.Core;
using Reports.Core.Extensions;
using Reports.Core.Interfaces;
using Reports.Core.Models;
using Reports.Core.PropertyHandlers;
using Reports.Core.SchemaBuilders;
using Reports.Demos.MVC.Reports;
using Reports.Excel.EpplusWriter;
using Reports.Html.StringWriter;

namespace Reports.Demos.MVC.Controllers.CustomProperties
{
    public class ReplaceEmptyController : Controller
    {
        private const int RecordsCount = 10;

        public async Task<IActionResult> Index()
        {
            IReportTable<ReportCell> reportTable = this.BuildReport();
            IReportTable<HtmlReportCell> htmlReportTable = this.ConvertToHtml(reportTable);
            string tableHtml = await this.WriteReportToString(htmlReportTable);

            return this.View(new ViewModel() { ReportTableHtml = tableHtml });
        }

        public IActionResult Download()
        {
            IReportTable<ReportCell> reportTable = this.BuildReport();
            IReportTable<ExcelReportCell> excelReportTable = this.ConvertToExcel(reportTable);

            Stream excelStream = this.WriteExcelReportToStream(excelReportTable);
            return this.File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Replace Empty Cells.xlsx");
        }

        private Stream WriteExcelReportToStream(IReportTable<ExcelReportCell> reportTable)
        {
            return new EpplusWriter().WriteToStream(reportTable);
        }

        private IReportTable<ReportCell> BuildReport()
        {
            VerticalReportSchemaBuilder<Entity> reportBuilder = new VerticalReportSchemaBuilder<Entity>();
            reportBuilder
                .AddColumn("First Name", e => e.FirstName)
                .AddColumn("Last Name", e => e.LastName)
                .AddColumn("Email", e => e.Email)
                .AddProperties(new ReplaceEmptyProperty("-"));
            reportBuilder.AddColumn("Score", e => e.Score)
                .AddProperties(new ReplaceEmptyProperty("(no score)"));

            IReportTable<ReportCell> reportTable = reportBuilder.BuildSchema().BuildReportTable(this.GetData());
            return reportTable;
        }

        private IReportTable<HtmlReportCell> ConvertToHtml(IReportTable<ReportCell> reportTable)
        {
            ReportConverter<HtmlReportCell> htmlConverter = new ReportConverter<HtmlReportCell>(new[]
            {
                new CustomFormatPropertyHtmlHandler(),
            });

            return htmlConverter.Convert(reportTable);
        }

        private IReportTable<ExcelReportCell> ConvertToExcel(IReportTable<ReportCell> reportTable)
        {
            ReportConverter<ExcelReportCell> excelConverter = new ReportConverter<ExcelReportCell>(new []
            {
                new CustomFormatPropertyExcelHandler(),
            });

            return excelConverter.Convert(reportTable);
        }

        private async Task<string> WriteReportToString(IReportTable<HtmlReportCell> htmlReportTable)
        {
            return await new BootstrapStringWriter(new StringCellWriter()).WriteToStringAsync(htmlReportTable);
        }

        public class ViewModel
        {
            public string ReportTableHtml { get; set; }
        }

        private IEnumerable<Entity> GetData()
        {
            return new Faker<Entity>()
                .RuleFor(e => e.FirstName, f => f.Name.FirstName())
                .RuleFor(e => e.LastName, f => f.Name.LastName())
                .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.FirstName, e.LastName).OrNull(f))
                .RuleFor(e => e.Score, f => f.Random.Int(1, 10).OrNull(f))
                .Generate(RecordsCount);
        }

        private class Entity
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public int? Score { get; set; }
        }

        private class ReplaceEmptyProperty : ReportCellProperty
        {
            public string Text { get; }

            public ReplaceEmptyProperty(string text)
            {
                this.Text = text;
            }
        }

        private class CustomFormatPropertyHtmlHandler : PropertyHandler<ReplaceEmptyProperty, HtmlReportCell>
        {
            protected override void HandleProperty(ReplaceEmptyProperty property, HtmlReportCell cell)
            {
                if (string.IsNullOrEmpty(cell.Html))
                {
                    cell.Html = property.Text;
                }
            }
        }

        private class CustomFormatPropertyExcelHandler : PropertyHandler<ReplaceEmptyProperty, ExcelReportCell>
        {
            protected override void HandleProperty(ReplaceEmptyProperty property, ExcelReportCell cell)
            {
                if (cell.InternalValue == null || (cell.ValueType == typeof(string) && string.IsNullOrEmpty(cell.InternalValue)))
                {
                    cell.InternalValue = property.Text;
                }
            }
        }
    }
}
