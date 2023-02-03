using XReports.Attributes;
using XReports.Interfaces;
using XReports.ReportCellsProviders;

namespace XReports.Tests.SchemaBuilders.AttributeBasedBuilderTests
{
    public partial class ComplexHeaderTest
    {
        [ComplexHeader(0, "Personal", 2, 3)]
        private class VerticalOneComplexHeaderByIndexes
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string FullName { get; set; }

            [ReportVariable(3, "Age")]
            public int Age { get; set; }
        }

        [ComplexHeader(0, "Personal", "Name", "Age")]
        private class VerticalOneComplexHeaderByTitles
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string FullName { get; set; }

            [ReportVariable(3, "Age")]
            public int Age { get; set; }
        }

        [HorizontalReport]
        [ComplexHeader(0, "Personal", 2, 3)]
        private class HorizontalOneComplexHeaderByIndexes
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string FullName { get; set; }

            [ReportVariable(3, "Age")]
            public int Age { get; set; }
        }

        [HorizontalReport]
        [ComplexHeader(0, "Personal", "Name", "Age")]
        private class HorizontalOneComplexHeaderByTitles
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string FullName { get; set; }

            [ReportVariable(3, "Age")]
            public int Age { get; set; }
        }

        [ComplexHeader(0, "Personal", 3, 5)]
        private class VerticalByIndexesWithGapsInIndexes
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(3, "Name")]
            public string FullName { get; set; }

            [ReportVariable(5, "Age")]
            public int Age { get; set; }
        }

        [HorizontalReport]
        [ComplexHeader(0, "Personal", 3, 5)]
        private class HorizontalByIndexesWithGapsInIndexes
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(3, "Name")]
            public string FullName { get; set; }

            [ReportVariable(5, "Age")]
            public int Age { get; set; }
        }

        [ComplexHeader(0, "Common", 1)]
        [ComplexHeader(0, "Employee Info", 2, 5)]
        [ComplexHeader(0, 2, "Dept. Info", 6)]
        [ComplexHeader(1, "Personal", "Name", "Age")]
        [ComplexHeader(1, "Job Info", 4, 5)]
        [ComplexHeader(2, "Sensitive", "Salary")]
        private class VerticalMultipleLevelsOfComplexHeader
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string FullName { get; set; }

            [ReportVariable(3, "Age")]
            public int Age { get; set; }

            [ReportVariable(4, "Job Title")]
            public string JobTitle { get; set; }

            [ReportVariable(5, "Salary")]
            public decimal Salary { get; set; }

            [ReportVariable(6, "Employee # in Department")]
            public int DepartmentEmployeeCount { get; set; }
        }

        [HorizontalReport]
        [ComplexHeader(0, "Common", 1)]
        [ComplexHeader(0, "Employee Info", 2, 5)]
        [ComplexHeader(0, 2, "Dept. Info", 6)]
        [ComplexHeader(1, "Personal", "Name", "Age")]
        [ComplexHeader(1, "Job Info", 4, 5)]
        [ComplexHeader(2, "Sensitive", "Salary")]
        private class HorizontalMultipleLevelsOfComplexHeader
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string FullName { get; set; }

            [ReportVariable(3, "Age")]
            public int Age { get; set; }

            [ReportVariable(4, "Job Title")]
            public string JobTitle { get; set; }

            [ReportVariable(5, "Salary")]
            public decimal Salary { get; set; }

            [ReportVariable(6, "Employee # in Department")]
            public int DepartmentEmployeeCount { get; set; }
        }

        [VerticalReport(PostBuilder = typeof(PostBuilder))]
        [ComplexHeader(0, "Complex Header", 1, 2)]
        private class VerticalByIndexesWithColumnFromPostBuilder
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Age")]
            public string Age { get; set; }

            private class PostBuilder : IVerticalReportPostBuilder<VerticalByIndexesWithColumnFromPostBuilder>
            {
                public void Build(IVerticalReportSchemaBuilder<VerticalByIndexesWithColumnFromPostBuilder> builder)
                {
                    builder.InsertColumnBefore("Age", "Name", new EmptyCellsProvider<VerticalByIndexesWithColumnFromPostBuilder>());
                }
            }
        }

        [HorizontalReport(PostBuilder = typeof(PostBuilder))]
        [ComplexHeader(0, "Complex Header", 1, 2)]
        private class HorizontalByIndexesWithColumnFromPostBuilder
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Age")]
            public string Age { get; set; }

            private class PostBuilder : IHorizontalReportPostBuilder<HorizontalByIndexesWithColumnFromPostBuilder>
            {
                public void Build(IHorizontalReportSchemaBuilder<HorizontalByIndexesWithColumnFromPostBuilder> builder)
                {
                    builder.InsertRowBefore("Age", "Name", new EmptyCellsProvider<HorizontalByIndexesWithColumnFromPostBuilder>());
                }
            }
        }

        [VerticalReport(PostBuilder = typeof(PostBuilder))]
        [ComplexHeader(0, "Complex Header", "ID", "Age")]
        private class VerticalByTitlesWithColumnFromPostBuilder
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Age")]
            public string Age { get; set; }

            private class PostBuilder : IVerticalReportPostBuilder<VerticalByTitlesWithColumnFromPostBuilder>
            {
                public void Build(IVerticalReportSchemaBuilder<VerticalByTitlesWithColumnFromPostBuilder> builder)
                {
                    builder.InsertColumnBefore("Age", "Name", new EmptyCellsProvider<VerticalByTitlesWithColumnFromPostBuilder>());
                }
            }
        }

        [HorizontalReport(PostBuilder = typeof(PostBuilder))]
        [ComplexHeader(0, "Complex Header", "ID", "Age")]
        private class HorizontalByTitlesWithColumnFromPostBuilder
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Age")]
            public string Age { get; set; }

            private class PostBuilder : IHorizontalReportPostBuilder<HorizontalByTitlesWithColumnFromPostBuilder>
            {
                public void Build(IHorizontalReportSchemaBuilder<HorizontalByTitlesWithColumnFromPostBuilder> builder)
                {
                    builder.InsertRowBefore("Age", "Name", new EmptyCellsProvider<HorizontalByTitlesWithColumnFromPostBuilder>());
                }
            }
        }

        [ComplexHeader(0, "Personal", 0, 1)]
        private class VerticalWithWrongStartIndex
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string FullName { get; set; }

            [ReportVariable(3, "Age")]
            public int Age { get; set; }
        }

        [ComplexHeader(0, "Personal", 1, 2)]
        private class VerticalWithWrongEndIndex
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(3, "Name")]
            public string FullName { get; set; }

            [ReportVariable(5, "Age")]
            public int Age { get; set; }
        }

        [HorizontalReport]
        [ComplexHeader(0, "Personal", 0, 1)]
        private class HorizontalWithWrongStartIndex
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(2, "Name")]
            public string FullName { get; set; }

            [ReportVariable(3, "Age")]
            public int Age { get; set; }
        }

        [HorizontalReport]
        [ComplexHeader(0, "Personal", 1, 2)]
        private class HorizontalWithWrongEndIndex
        {
            [ReportVariable(1, "ID")]
            public int Id { get; set; }

            [ReportVariable(3, "Name")]
            public string FullName { get; set; }

            [ReportVariable(5, "Age")]
            public int Age { get; set; }
        }
    }
}