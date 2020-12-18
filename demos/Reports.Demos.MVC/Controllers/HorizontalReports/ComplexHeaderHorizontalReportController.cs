using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using Bogus;
using Microsoft.AspNetCore.Mvc;
using Reports.Core;
using Reports.Core.Enums;
using Reports.Core.Extensions;
using Reports.Core.Interfaces;
using Reports.Core.Models;
using Reports.Core.SchemaBuilders;
using Reports.Demos.MVC.Reports;
using Reports.Excel.EpplusWriter;
using Reports.Extensions.Properties;
using Reports.Extensions.Properties.PropertyHandlers.Excel;
using Reports.Extensions.Properties.PropertyHandlers.Html;

namespace Reports.Demos.MVC.Controllers.HorizontalReports
{
    public class ComplexHeaderHorizontalReportController : Controller
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
            return this.File(excelStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Horizontal.xlsx");
        }

        private Stream WriteExcelReportToStream(IReportTable<ExcelReportCell> reportTable)
        {
            EpplusWriter writer = new EpplusWriter();
            return writer.WriteToStream(reportTable);
        }

        private IReportTable<ReportCell> BuildReport()
        {
            ReportCellProperty centerAlignment = new AlignmentProperty(AlignmentType.Center);
            BoldProperty bold = new BoldProperty();

            HorizontalReportSchemaBuilder<Entity> reportBuilder = new HorizontalReportSchemaBuilder<Entity>();
            reportBuilder
                .AddHeaderRow(0, "Metrics", e => e.Name)
                .AddProperties(centerAlignment)
                .AddHeaderProperties(centerAlignment);
            reportBuilder.AddRow("Age", e => e.Age)
                .AddHeaderProperties(bold)
                .AddProperties(centerAlignment);
            reportBuilder.AddRow("Min. Score", e => e.MinScore)
                .AddProperties(centerAlignment);
            reportBuilder.AddRow("Max. Score", e => e.MaxScore)
                .AddProperties(centerAlignment);
            reportBuilder.AddRow("Avg. Score", e => e.AverageScore)
                .AddProperties(new DecimalFormatProperty(2), centerAlignment);

            reportBuilder.AddComplexHeader(0, "Score", "Min. Score", "Avg. Score");
            reportBuilder.AddComplexHeaderProperties("Score", new ColorProperty(Color.Blue));

            return reportBuilder.BuildSchema().BuildReportTable(this.GetData());
        }

        private IReportTable<HtmlReportCell> ConvertToHtml(IReportTable<ReportCell> reportTable)
        {
            ReportConverter<HtmlReportCell> htmlConverter = new ReportConverter<HtmlReportCell>(new IPropertyHandler<HtmlReportCell>[]
            {
                new DecimalFormatPropertyHtmlHandler(),
                new AlignmentPropertyHtmlHandler(),
                new BoldPropertyHtmlHandler(),
                new ColorPropertyHtmlHandler(),
            });

            return htmlConverter.Convert(reportTable);
        }

        private IReportTable<ExcelReportCell> ConvertToExcel(IReportTable<ReportCell> reportTable)
        {
            ReportConverter<ExcelReportCell> excelConverter = new ReportConverter<ExcelReportCell>(new IPropertyHandler<ExcelReportCell>[]
            {
                new DecimalFormatPropertyExcelHandler(),
                new AlignmentPropertyExcelHandler(),
                new BoldPropertyExcelHandler(),
                new ColorPropertyExcelHandler(),
            });

            return excelConverter.Convert(reportTable);
        }

        private async Task<string> WriteReportToString(IReportTable<HtmlReportCell> htmlReportTable)
        {
            return await new BootstrapStringWriter().WriteToStringAsync(htmlReportTable);
        }

        public class ViewModel
        {
            public string ReportTableHtml { get; set; }
        }

        private IEnumerable<Entity> GetData()
        {
            return new Faker<Entity>()
                .RuleFor(e => e.Name, f => f.Name.FullName())
                .RuleFor(e => e.MinScore, f => f.Random.Int(1, 10))
                .RuleFor(e => e.MaxScore, (f, e) => f.Random.Int(e.MinScore, 10))
                .RuleFor(e => e.AverageScore, (f, e) => f.Random.Decimal(e.MinScore, e.MaxScore))
                .RuleFor(e => e.Age, f => f.Random.Int(18, 63))
                .RuleFor(e => e.Email, (f, e) => f.Internet.Email(e.Name))
                .Generate(RecordsCount);
        }

        private class Entity
        {
            public string Name { get; set; }
            public decimal AverageScore { get; set; }
            public int MinScore { get; set; }
            public int MaxScore { get; set; }
            public int Age { get; set; }
            public string Email { get; set; }
        }
    }
}