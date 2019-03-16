using Aspose.Cells;
using Aspose.Cells.Charts;
using Ceb.Logger;
using Ceb.MerlinTool.WebAPI.Constants;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace Ceb.MerlinTool.WebAPI.Utility
{
    public static class ExcelProcessHelper
    {
        private static System.Type _objType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;

        public static DataTable ExcelToDataTable(string filePath, string sheetName)
        {
            DataTable dtSurveyData = new DataTable();
            string strCellText = string.Empty;
            using (ExcelPackage pck = new ExcelPackage())
            {
                System.IO.FileStream openFile = new System.IO.FileInfo(filePath).OpenRead();
                try
                {
                    pck.Load(openFile);
                    //pck.Load(new System.IO.FileInfo(filePath).OpenRead());
                    var ws = pck.Workbook.Worksheets[sheetName];

                    var hasHeader = true;
                    var startRowCount = 1;

                    foreach (var firstRowCell in ws.Cells[startRowCount, 1, startRowCount, ws.Dimension.End.Column])
                    {
                        dtSurveyData.Columns.Add(hasHeader ? firstRowCell.Text.Trim() : string.Format("Column {0}", firstRowCell.Start.Column));
                    }
                    int colCount = dtSurveyData.Columns.Count;
                    startRowCount++;
                    for (var rowNum = startRowCount; rowNum <= ws.Dimension.End.Row; rowNum++)
                    {
                        var wsRow = ws.Cells[rowNum, 1, rowNum, colCount];
                        var row = dtSurveyData.NewRow();
                        foreach (var cell in wsRow)
                        {
                            strCellText = cell.Text.Trim();
                            row[cell.Start.Column - 1] = strCellText;
                        }
                        dtSurveyData.Rows.Add(row);
                    }
                }
                finally
                {
                    openFile.Close();
                }
                
            }
            ValidateDataTable(dtSurveyData);
            return dtSurveyData;
        }

        static string ValidateDataTable(DataTable dt)
        {
            List<int> rowHavingIsssues = new List<int>();
            List<int> rowToRemove = new List<int>();
            int addRowCounttoAdjust = 3;
            int count = -1;
            foreach (DataRow dr in dt.Rows)
            {
                count++;

                //Remove empty rows
                if (dr[0] == null &&
                    dr[1] == null &&
                    dr[2] == null
                    )
                {
                    rowToRemove.Add(count);
                    continue;
                }

                //Remove empty rows
                if (String.IsNullOrWhiteSpace(dr[0].ToString()) &&
                    String.IsNullOrWhiteSpace(dr[1].ToString()) &&
                    String.IsNullOrWhiteSpace(dr[2].ToString())
                    )
                {
                    rowToRemove.Add(count);
                    continue;
                }

                //Reject excel sheet if any column having issue
                if (dr[0] == null ||
                    dr[1] == null ||
                    dr[2] == null ||
                    String.IsNullOrWhiteSpace(dr[0].ToString()) ||
                    String.IsNullOrWhiteSpace(dr[1].ToString()) ||
                    String.IsNullOrWhiteSpace(dr[2].ToString())
                    )
                {
                    rowHavingIsssues.Add(count);
                }
            }

            if (rowHavingIsssues.Count() > 0)
                return "Cannot upload please fix errors for rows #" + string.Join(",", rowHavingIsssues.Select(x => (x + addRowCounttoAdjust).ToString()));

            foreach (int ind in rowToRemove.OrderByDescending(x => x))
            {
                dt.Rows.RemoveAt(ind);
            }
            return "success";
        }

        public static bool GetColumnByName(ExcelWorksheet worksheet)
        {
            List<string> difference;
            try
            {
                var cells = worksheet.Cells[1, 1, 1, 37];
                List<string> availableCols = cells.GroupBy(c => new { c.Start.Row, c.Start.Column }).Select(rcg => Convert.ToString(cells[rcg.Key.Row, rcg.Key.Column].Value)).ToList<string>();
                if (worksheet.Name == ConfigurationManager.AppSettings["SheetName_2"])
                    difference = Constants.MerlinConstants.ExcelRequiredColumnsPerceptionGap.Except(availableCols).ToList();
                else
                    difference = Constants.MerlinConstants.ExcelRequiredColumns.Except(availableCols).ToList();
                if (difference == null || difference.Count == 0)//all columns exists
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                return false;
            }
        }

        public static bool ExportDashboardExcel(DataTable dtRating, string exportFilePath, string title, string subtitle, string company, string timePeriod
            , Dictionary<string, string> columnTitles, string page)
        {
            Range range;
            string path = AppDomain.CurrentDomain.BaseDirectory;
            Aspose.Cells.License lic = new Aspose.Cells.License();
            string excelTemplate = path + @"\Template\ExcelTemplate.xlsx";//path + @"\Template\Copy of Overall Supplier Ratings.xlsx";//
            int col = 0, rowStartIndex = Convert.ToInt16(ConfigurationManager.AppSettings["startRow"]);
            Style excelStyle = new Style();
            List<string> columnNames = new List<string>();
            Style style;
            try
            {
                lic.SetLicense(File.OpenRead(path + @"\Template\Aspose.Total.lic"));
                System.IO.File.Copy(excelTemplate, exportFilePath);

                Workbook workbook = new Workbook(exportFilePath);
                Worksheet workSheet = workbook.Worksheets[0];
                workSheet.Name = MerlinConstants.ExportMappings[page];

                Cells cells = workSheet.Cells;

                #region update company details & title
                cells.Rows[6][0].Value = string.Format("Institution     : {0}", company);
                if (page.Equals(MerlinConstants.TRENDOVERALLSCORE) || page.Equals(MerlinConstants.METRICTRENDS) || page.Equals(MerlinConstants.KPITRENDS))
                    cells.Rows[7][0].Value = string.Format("Time Period : {0}", "All");
                else
                    cells.Rows[7][0].Value = string.Format("Time Period : {0}", timePeriod);
                cells.Rows[9][0].Value = title;
                cells.Rows[10][0].Value = subtitle;
                #endregion

                if (dtRating.Columns.Contains("fk_CategoryId"))
                    dtRating.Columns.Remove("fk_CategoryId");

                if (dtRating.Columns.Contains("fk_Metric"))
                    dtRating.Columns.Remove("fk_Metric");

                if (dtRating.Columns.Contains("rank"))
                    dtRating.Columns.Remove("rank");

                if (dtRating.Columns.Contains("pk_SupplierId"))
                    dtRating.Columns.Remove("pk_SupplierId");

                dtRating.DefaultView.Sort = dtRating.Columns[0].ColumnName + " asc";
                dtRating = dtRating.DefaultView.ToTable();

                #region update headers
                for (int i = 0; i < dtRating.Columns.Count; i++, col++)
                {
                    string columnName = dtRating.Columns[i].ColumnName;
                    cells.Rows[rowStartIndex - 1][col].PutValue((columnTitles != null && columnTitles.ContainsKey(columnName)) ? columnTitles[columnName] : columnName);
                    cells.Rows[rowStartIndex - 1][col].SetStyle(cells.Rows[rowStartIndex - 1][0].GetStyle());
                    columnNames.Add(columnName);
                }
                #endregion

                workSheet.Cells.ImportDataTable(dtRating, false, rowStartIndex, 0, true);

                if (columnNames.Count > 1)
                {
                    int firstColumn = (page.Equals(MerlinConstants.OVERALLGRIDTITLE)) ? 1 : 0;
                    int totalColumns = page.Equals(MerlinConstants.OVERALLGRIDTITLE) || page.Equals(MerlinConstants.OVERALLTITLE) ? columnNames.Count - 1 : columnNames.Count;
                    range = workSheet.Cells.CreateRange(rowStartIndex, firstColumn, dtRating.Rows.Count, totalColumns);//cells
                    style = cells.Rows[rowStartIndex][0].GetStyle();
                    style.Number = 4;
                    range.SetStyle(style);
                }
                else
                {
                    range = workSheet.Cells.CreateRange(rowStartIndex, 0, dtRating.Rows.Count, 1);
                    style = cells.Rows[rowStartIndex][0].GetStyle();
                    style.Number = 4;
                    range.SetStyle(style);
                }

                if (columnNames.Contains("NoOfRespondants"))
                {
                    range = workSheet.Cells.CreateRange(rowStartIndex, columnNames.IndexOf("NoOfRespondants"), dtRating.Rows.Count, 1);
                    style = cells.Rows[rowStartIndex][0].GetStyle();
                    style.Number = 0;
                    range.SetStyle(style);
                }

                workSheet.Cells.DeleteRow(rowStartIndex + dtRating.Rows.Count);

                #region ApplyConditionalFormatting
                if (page.Equals(MerlinConstants.OVERALLGRIDTITLE) || page.Equals(MerlinConstants.SUPPLIERBYKPI) || page.Equals(MerlinConstants.PERCEPTIONKPISCORE) || page.Equals(MerlinConstants.TRENDKPISCORE) || page.Equals(MerlinConstants.KPITRENDS))
                {
                    int index = workSheet.ConditionalFormattings.Add();
                    FormatConditionCollection fcs = workSheet.ConditionalFormattings[index];

                    // Sets the conditional format range.
                    CellArea ca = new CellArea();
                    ca.StartRow = rowStartIndex;
                    ca.EndRow = (rowStartIndex + dtRating.Rows.Count) - 1;
                    ca.StartColumn = 1;
                    ca.EndColumn = columnNames.Count - 1;//skip column headers
                    fcs.AddArea(ca);

                    int conditionIndex = fcs.AddCondition(FormatConditionType.CellValue, OperatorType.Between, ConfigurationManager.AppSettings["range1From"], ConfigurationManager.AppSettings["range1To"]);
                    FormatCondition fc = fcs[conditionIndex];
                    fc.Style.BackgroundColor = ColorTranslator.FromHtml(ConfigurationManager.AppSettings["range1Color"]);

                    int conditionIndex1 = fcs.AddCondition(FormatConditionType.CellValue, OperatorType.Between, ConfigurationManager.AppSettings["range2From"], ConfigurationManager.AppSettings["range2To"]);
                    FormatCondition fc1 = fcs[conditionIndex1];
                    fc1.Style.BackgroundColor = ColorTranslator.FromHtml(ConfigurationManager.AppSettings["range2Color"]);

                    int conditionIndex2 = fcs.AddCondition(FormatConditionType.CellValue, OperatorType.Between, ConfigurationManager.AppSettings["range3From"], ConfigurationManager.AppSettings["range3To"]);
                    FormatCondition fc2 = fcs[conditionIndex2];
                    fc2.Style.BackgroundColor = ColorTranslator.FromHtml(ConfigurationManager.AppSettings["range3Color"]);

                    int conditionIndex3 = fcs.AddCondition(FormatConditionType.CellValue, OperatorType.Between, ConfigurationManager.AppSettings["range4From"], ConfigurationManager.AppSettings["range4To"]);
                    FormatCondition fc3 = fcs[conditionIndex3];
                    fc3.Style.BackgroundColor = ColorTranslator.FromHtml(ConfigurationManager.AppSettings["range4Color"]);

                    int conditionIndex0 = fcs.AddCondition(FormatConditionType.CellValue, OperatorType.Equal, "0.00", "0");
                    FormatCondition fc0 = fcs[conditionIndex0];
                    fc0.Style.Font.Color = Color.LightGray;// ColorTranslator.FromHtml(ConfigurationManager.AppSettings["colorIfzero"]);
                    //fc0.Text = "";
                    fc0.Style.BackgroundColor = Color.LightGray;// ColorTranslator.FromHtml(ConfigurationManager.AppSettings["colorIfzero"]);

                    if (page.Equals(MerlinConstants.SUPPLIERBYKPI))
                    {
                        for (int cellCounter = rowStartIndex; cellCounter < (rowStartIndex + dtRating.Rows.Count); cellCounter++)
                        {
                            int indexCellText = workSheet.ConditionalFormattings.Add();
                            FormatConditionCollection fcsTextColl = workSheet.ConditionalFormattings[indexCellText];

                            // Sets the conditional format range.
                            CellArea caText = new CellArea();
                            caText.StartRow = cellCounter;
                            caText.EndRow = cellCounter;
                            caText.StartColumn = 1;
                            caText.EndColumn = 1;//skip column headers
                            fcsTextColl.AddArea(caText);

                            int conditionIndexText = fcsTextColl.AddCondition(FormatConditionType.Expression);
                            FormatCondition fcText = fcsTextColl[conditionIndexText];
                            fcText.Formula1 = string.Format("=$C${0}<$D${0}", (cellCounter + 1));
                            fcText.Style.Font.Color = ColorTranslator.FromHtml(ConfigurationManager.AppSettings["KPIRiskIndicator"]);
                        }
                    }
                }
                #endregion

                switch (page)
                {
                    case MerlinConstants.OVERALLTITLE:
                    case MerlinConstants.SUPPLIERBYMETRIC:
                    case MerlinConstants.PERCEPTIONMETRICSCORE:
                    case MerlinConstants.CATEGORYANALYSISTITLE:
                    case MerlinConstants.TRENDMETRICSCORE:
                        int index = workSheet.Charts.Add(ChartType.Bar, rowStartIndex - 1, 4, rowStartIndex + dtRating.Rows.Count, 6);
                        Aspose.Cells.Charts.Chart chart = workSheet.Charts[index];
                        char seriesColn1, seriesColn2;
                        seriesColn1 = (page.Equals(MerlinConstants.OVERALLTITLE) || page.Equals(MerlinConstants.CATEGORYANALYSISTITLE)
                            || page.Equals(MerlinConstants.SUPPLIERBYMETRIC) || page.Equals(MerlinConstants.PERCEPTIONMETRICSCORE) || page.Equals(MerlinConstants.TRENDMETRICSCORE)) ? 'B' : 'C';
                        seriesColn2 = page.Equals(MerlinConstants.SUPPLIERBYMETRIC) || page.Equals(MerlinConstants.PERCEPTIONMETRICSCORE) || page.Equals(MerlinConstants.CATEGORYANALYSISTITLE) ? 'C' : seriesColn1;

                        chart = GenerateBarChartinExcel(chart, page, (rowStartIndex + dtRating.Rows.Count), 'A', seriesColn1, seriesColn2, dtRating.Rows.Count);
                        break;
                    case MerlinConstants.METRICTRENDS:
                    case MerlinConstants.TRENDOVERALLSCORE:
                        index = workSheet.Charts.Add(ChartType.LineWithDataMarkers, rowStartIndex + dtRating.Rows.Count + 2, 1, 35, 6);
                        chart = workSheet.Charts[index];
                        chart = GenerateLineChartinExcel(chart, page, (rowStartIndex + dtRating.Rows.Count), dtRating.Rows.Count, dtRating.Columns.Count - 1, page);
                        break;
                    default: break;
                }
                //generateChart();f
                workbook.Save(exportFilePath, SaveFormat.Xlsx);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, ex.Message);
                return false;
            }
        }

        private static Aspose.Cells.Charts.Chart GenerateBarChartinExcel(Aspose.Cells.Charts.Chart chart, string chartTitle, int excelRows, char categoryColumn, char seriesColumn1, char seriesColumn2, int numberOfRecords)
        {
            //chart appearance
            chart.Title.Text = chartTitle.Contains("Trend ") ? chartTitle.Remove(0, 6) : chartTitle.Contains("Perception ") ? chartTitle.Remove(0, 11) : chartTitle;
            chart.Title.TextFont.IsBold = true;
            chart.Title.TextFont.Size = 12;
            chart.Title.TextFont.Color = ColorTranslator.FromHtml("#ffa500");

            chart.ShowLegend = false;
            chart.NSeries.Add(string.Format("{1}15:{2}{0}", excelRows, seriesColumn1, seriesColumn2), true);// B: C
            chart.NSeries.CategoryData = string.Format("{1}15:{1}{0}", excelRows, categoryColumn);//A:A

            //Set the DataLabels in the chart
            Aspose.Cells.Charts.DataLabels datalabels;
            for (int series = 0; series < chart.NSeries.Count; series++)
            {
                for (int i = 0; i < chart.NSeries[series].Points.Count; i++)
                {
                    datalabels = chart.NSeries[series].Points[i].DataLabels;
                    datalabels.TextFont.Color = Color.Black;
                    datalabels.TextFont.Name = "Arial";
                    datalabels.TextFont.Size = 8;
                    //datalabels.Position = Aspose.Cells.Charts.LabelPositionType.InsideBase;
                    //datalabels.ShowCategoryName = true;
                    datalabels.ShowValue = true;
                }
            }

            chart.CategoryAxis.MajorGridLines.IsVisible = false;
            chart.CategoryAxis.MinorGridLines.IsVisible = false;
            chart.CategoryAxis.MajorTickMark = TickMarkType.None;
            chart.CategoryAxis.MinorTickMark = TickMarkType.None;

            chart.SeriesAxis.MajorGridLines.IsVisible = false;
            chart.SeriesAxis.MinorGridLines.IsVisible = false;
            chart.SeriesAxis.MajorTickMark = TickMarkType.None;
            chart.SeriesAxis.MinorTickMark = TickMarkType.None;

            chart.ValueAxis.IsVisible = false;
            chart.ValueAxis.MajorGridLines.IsVisible = false;
            chart.ValueAxis.MinorGridLines.IsVisible = false;
            chart.ValueAxis.MajorTickMark = TickMarkType.None;
            chart.ValueAxis.MinorTickMark = TickMarkType.None;

            chart.ChartArea.TextFont.Name = "Arial";
            chart.ChartArea.TextFont.Size = 8;
            chart.ChartArea.IsAutomaticSize = false;
            if (numberOfRecords < 10)
                chart.ChartObject.Height = numberOfRecords * 70;

            chart.PlotArea.Area.Formatting = FormattingType.Custom;
            chart.PlotArea.BackgroundMode = BackgroundMode.Transparent;
            chart.PlotArea.Area.ForegroundColor = Color.White;
            chart.PlotArea.Border.IsVisible = false;
            chart.CategoryAxis.IsPlotOrderReversed = true;

            chart.NSeries.IsColorVaried = false;
            for (int series = 0; series < chart.NSeries.Count; series++)
            {
                chart.NSeries[series].Area.Formatting = FormattingType.Custom;
                chart.NSeries[series].Area.ForegroundColor = series == 0 ? ColorTranslator.FromHtml("#4F81BD") : ColorTranslator.FromHtml("#C0504D");
                chart.NSeries[series].DataLabels.ShowValue = true;
                chart.NSeries[series].DataLabels.Position = LabelPositionType.OutsideEnd;
                chart.NSeries[series].DataLabels.TextDirection = TextDirectionType.LeftToRight;
                chart.NSeries[series].DataLabels.TextHorizontalAlignment = TextAlignmentType.Left;
                chart.NSeries[series].DataLabels.TextVerticalAlignment = TextAlignmentType.Center;
            }

            chart.AutoScaling = true;
            chart.CategoryAxis.AxisBetweenCategories = true;
            chart.CategoryAxis.MajorGridLines.IsVisible = false;
            chart.IsRectangularCornered = true;
            chart.ShowDataTable = false;
            chart.SizeWithWindow = true;
            return chart;
        }

        private static Aspose.Cells.Charts.Chart GenerateLineChartinExcel(Aspose.Cells.Charts.Chart chart, string chartTitle, int excelRows, int numberOfRecords
            , int columnscount, string page)
        {
            int startRow = Convert.ToInt16(ConfigurationManager.AppSettings["startRow"]);

            //chart appearance
            chart.Title.Text = chartTitle.Contains("Trend ") ? chartTitle.Remove(0, 6) : chartTitle.Contains("Perception ") ? chartTitle.Remove(0, 11) : chartTitle;

            chart.Title.TextFont.IsBold = true;
            chart.Title.TextFont.Size = 12;
            chart.Title.TextFont.Color = ColorTranslator.FromHtml("#ffa500");
            chart.ChartObject.Width = 600;
            chart.ChartObject.Height = 500;
            chart.ShowLegend = true;
            chart.Legend.Position = LegendPositionType.Bottom;
            string lastColn = CellsHelper.ColumnIndexToName(columnscount);
            //string series = string.Empty, cateogry = string.Empty;
            if (page.Equals(MerlinConstants.TRENDOVERALLSCORE))
            {
                chart.NSeries.Add(string.Format("A15:{0}{1}", lastColn, excelRows), false);//"=MetricTrends!$A$14:$C$20", false);
                chart.NSeries.CategoryData = string.Format("A{0}:{1}{0}", startRow, lastColn);//A:A
            }
            else
            {
                chart.NSeries.Add(string.Format("B15:{0}{1}", lastColn, excelRows), false);//"=MetricTrends!$A$14:$C$20", false);
                chart.NSeries.CategoryData = string.Format("B{0}:{1}{0}", startRow, lastColn);//A:A
            }
            //chart.NSeries.Add(string.Format("A15:{0}{1}", 'C', excelRows), false);//"=MetricTrends!$A$14:$C$20", false);
            //chart.NSeries.CategoryData = string.Format("B14:C{0}", 14);//A:A


            //Set the DataLabels in the chart
            Aspose.Cells.Charts.DataLabels datalabels;
            int seriesColn = startRow + 1;
            for (int series = 0; series < chart.NSeries.Count; series++)
            {
                chart.NSeries[series].Name = page.Equals(MerlinConstants.TRENDOVERALLSCORE) ? "Time Period" : string.Format("=A{0}", seriesColn++);

                for (int i = 0; i < chart.NSeries[series].Points.Count; i++)
                {
                    datalabels = chart.NSeries[series].Points[i].DataLabels;
                    datalabels.TextFont.Color = Color.Black;
                    datalabels.TextFont.Name = "Arial";
                    datalabels.TextFont.Size = 8;
                    datalabels.Position = Aspose.Cells.Charts.LabelPositionType.InsideBase;
                    //datalabels.ShowCategoryName = true;
                    datalabels.ShowValue = true;
                }
            }

            chart.CategoryAxis.MajorGridLines.IsVisible = false;
            chart.CategoryAxis.MinorGridLines.IsVisible = false;
            chart.CategoryAxis.MajorTickMark = TickMarkType.None;
            chart.CategoryAxis.MinorTickMark = TickMarkType.None;

            chart.SeriesAxis.MajorGridLines.IsVisible = false;
            chart.SeriesAxis.MinorGridLines.IsVisible = false;
            chart.SeriesAxis.MajorTickMark = TickMarkType.None;
            chart.SeriesAxis.MinorTickMark = TickMarkType.None;

            chart.ValueAxis.IsVisible = true;

            chart.SecondValueAxis.IsVisible = true;
            chart.ValueAxis.MajorGridLines.IsVisible = false;
            chart.ValueAxis.MinorGridLines.IsVisible = false;
            chart.ValueAxis.MajorTickMark = TickMarkType.None;
            chart.ValueAxis.MinorTickMark = TickMarkType.None;

            chart.ChartArea.TextFont.Name = "Arial";
            chart.ChartArea.TextFont.Size = 8;
            chart.ChartArea.IsAutomaticSize = false;
            if (numberOfRecords < 10)
                chart.ChartObject.Height = numberOfRecords * 70;

            chart.PlotArea.Area.Formatting = FormattingType.Custom;
            chart.PlotArea.BackgroundMode = BackgroundMode.Transparent;
            chart.PlotArea.Area.ForegroundColor = Color.White;
            chart.PlotArea.Border.IsVisible = false;
            //chart.CategoryAxis.IsPlotOrderReversed = true;

            chart.NSeries.IsColorVaried = false;
            for (int series = 0; series < chart.NSeries.Count; series++)
            {
                chart.NSeries[series].Area.Formatting = FormattingType.Custom;
                chart.NSeries[series].Area.ForegroundColor = series == 0 ? ColorTranslator.FromHtml("#4F81BD") : ColorTranslator.FromHtml("#C0504D");
                chart.NSeries[series].DataLabels.ShowValue = true;
                chart.NSeries[series].DataLabels.Position = LabelPositionType.OutsideEnd;
                chart.NSeries[series].DataLabels.TextDirection = TextDirectionType.LeftToRight;
                chart.NSeries[series].DataLabels.TextHorizontalAlignment = TextAlignmentType.Left;
                chart.NSeries[series].DataLabels.TextVerticalAlignment = TextAlignmentType.Center;
            }

            chart.AutoScaling = true;
            chart.CategoryAxis.AxisBetweenCategories = true;
            chart.CategoryAxis.MajorGridLines.IsVisible = false;
            chart.IsRectangularCornered = true;
            chart.ShowDataTable = false;
            chart.SizeWithWindow = true;
            return chart;
        }
    }
}