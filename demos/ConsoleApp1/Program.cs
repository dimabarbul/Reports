﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Reports;
using Reports.Builders;
using Reports.Excel.EpplusWriter;
using Reports.Excel.Models;
using Reports.Extensions;
using Reports.Extensions.Properties;
using Reports.Extensions.Properties.Handlers.Excel;
using Reports.Extensions.Properties.Handlers.StandardHtml;
using Reports.Html.HtmlStringWriter;
using Reports.Html.Models;
using Reports.Interfaces;
using Reports.Models;

namespace ConsoleApp1
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            VerticalReportBuilder<(int, decimal)> builder = CreateBuilder();
            IReportTable<ReportCell> reportTable = BuildReportTable(builder);

            Console.Write("Export to excel? y=excel n=html: ");
            bool isExcel = Console.ReadLine()?.ToLower() == "y";

            if (isExcel)
            {
                ExportToExcel(reportTable);
            }
            else
            {
                await ExportToHtmlAsync(reportTable);
            }

            return;
        }

        private static VerticalReportBuilder<(int, decimal)> CreateBuilder()
        {
            VerticalReportBuilder<(int, decimal)> builder = new VerticalReportBuilder<(int, decimal)>();
            builder.AddColumn("Now", i => DateTime.Now)
                .AddProperty(new DateTimeFormatProperty("dd MMM yyyy"));
            builder.AddColumn("Now", i => DateTime.Now)
                .AddProperty(new DateTimeFormatProperty("dd/MM/yyyy HH:mm:ss"));
            builder.AddColumn("Integer", i => i.Item1);
            builder.AddColumn("Without formatting", i => i.Item2);
            builder.AddColumn("With 2 decimals", i => i.Item2)
                .AddProperty(new DecimalFormatProperty(2));
            builder.AddColumn("String", i => i.Item2.ToString());
            builder.AddColumn("Is odd", i => i.Item1 % 2 == 0
                ? "YES"
                : (string) null);
            builder.AddColumn("With max.length", i => "Looooooooooong")
                .AddProperty(new MaxLengthProperty(5));
            builder.AddColumn("Percent", i => i.Item2)
                .AddProperty(new PercentFormatProperty(1));
            builder.AddColumn("Colored", i => i.Item1 % 10)
                .AddProperty(new ColorProperty(Color.Yellow, Color.Black));

            return builder;
        }

        private static IReportTable<ReportCell> BuildReportTable(VerticalReportBuilder<(int, decimal)> builder)
        {
            Random random = new Random(DateTime.Now.Millisecond);
            IReportTable<ReportCell> reportTable = builder.Build(Enumerable.Range(1, 10000)
                .Select(x => (random.Next(), (decimal) random.NextDouble())));
            return reportTable;
        }

        private static void ExportToExcel(IReportTable<ReportCell> reportTable)
        {
            ReportConverter<ExcelReportCell> converter = new ReportConverter<ExcelReportCell>(
                new IPropertyHandler<ExcelReportCell>[]
                {
                    new ExcelAlignmentPropertyHandler(),
                    new ExcelBoldPropertyHandler(),
                    new ExcelColorPropertyHandler(),
                    new ExcelDateTimeFormatPropertyHandler(),
                    new ExcelDecimalFormatPropertyHandler(),
                    new ExcelMaxLengthPropertyHandler(),
                    new ExcelPercentFormatPropertyHandler(),
                }
            );
            IReportTable<ExcelReportCell> excelReportTable = converter.Convert(reportTable);

            const string fileName = "/tmp/report.xlsx";

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            ExcelWriter writer = new ExcelWriter();
            Stopwatch sw = Stopwatch.StartNew();
            writer.WriteToFile(excelReportTable, fileName);
            sw.Stop();

            Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds} ms");
        }

        private static async Task ExportToHtmlAsync(IReportTable<ReportCell> reportTable)
        {
            ReportConverter<HtmlReportCell> converter = new ReportConverter<HtmlReportCell>(
                new IPropertyHandler<HtmlReportCell>[]
                {
                    new StandardHtmlAlignmentPropertyHandler(),
                    new StandardHtmlBoldPropertyHandler(),
                    new StandardHtmlColorPropertyHandler(),
                    new StandardHtmlDateTimeFormatPropertyHandler(),
                    new StandardHtmlDecimalFormatPropertyHandler(),
                    new StandardHtmlMaxLengthPropertyHandler(),
                    new StandardHtmlPercentFormatPropertyHandler(),
                }
            );
            IReportTable<HtmlReportCell> htmlReportTable = converter.Convert(reportTable);

            const string fileName = "/tmp/report.html";

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            GeneralCellWriter cellWriter = new GeneralCellWriter();
            HtmlStringWriter writer = new HtmlStringWriter(cellWriter.WriteHeaderCell, cellWriter.WriteBodyCell);
            Stopwatch sw = Stopwatch.StartNew();
            await writer.WriteToFileAsync(htmlReportTable, fileName);
            sw.Stop();

            Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds} ms");
        }
    }
}
