using Ceb.Logger;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace Ceb.MerlinTool.WebAPI.ExportHelper
{
    public class HighChartExportPhantomJS
    {
        private static string sBatchFile = string.Empty;
        private static string sPhanthomJsLocation = string.Empty;
        private static string chartImgFile = string.Empty;
        private static string sJsonGenerationPath = string.Empty;
        private static System.Type _objType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
        private static string sChartOutputFiles = string.Empty;
        private static List<string> sJsonFilePath = null;

        /// <summary>
        /// Static method which accepts parameters for export the chart to ppt using phanthomjs utility
        /// </summary>
        /// <param name="jsonInputfiles"></param>
        /// <returns></returns>
        public static string Export(string p_json)
        {
            sJsonFilePath = new List<string>();
            string path = AppDomain.CurrentDomain.BaseDirectory;
            bool done = false;
            try
            {
                sPhanthomJsLocation = Path.Combine(path, @"Scripts\phantomjs");
                sBatchFile = Path.Combine(path, @"Scripts\phantomjs\chart_export.bat");
                sJsonGenerationPath = ConfigurationManager.AppSettings["ExportFolderPath"];

                //Write Json to a physical file
                p_json = p_json.Replace("\"fmtFUNC()", "function()").Replace("}fmtFUNC\"", "}");
                sJsonFilePath.Add(sJsonGenerationPath + System.Guid.NewGuid().ToString("N") + "ExportChart.json");              
                File.AppendAllText(sJsonFilePath[0].ToString(), p_json);

                //Perform Export process using Phanthom Utility
                foreach (string jsonDataFile in sJsonFilePath)
                {
                    using (System.Diagnostics.Process process = new System.Diagnostics.Process())
                    {
                        process.StartInfo = new System.Diagnostics.ProcessStartInfo();
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.FileName = sBatchFile;

                        chartImgFile = jsonDataFile.Replace(".json", ".png").Replace(".js", ".png");
                        //%1\phantomjs %1\highcharts-convert.js -infile %2 -outfile %3 -width %4 -height %5 -constr Chart
                        process.StartInfo.Arguments = string.Format("\"{0}\" \"{1}\" \"{2}\" \"{3}\" \"{4}\"", sPhanthomJsLocation, jsonDataFile, chartImgFile,1200,500);
                        process.StartInfo.CreateNoWindow = true;
                        process.Start();
                    }
                    sChartOutputFiles = chartImgFile;
                }
                
                while (!done)
                {
                    if (File.Exists(sChartOutputFiles))
                    {
                        done = true;
                        break;
                    }
                    System.Threading.Thread.Sleep(1000);
                }
                return sChartOutputFiles;
            }
            catch (NullReferenceException ex)
            {
                string strErrorMessage = string.Format("{0}{1}", "An error occured while doing the HighChart export option.", ex.Message);
                LogHelper.Error(_objType, ex, strErrorMessage);
                return null;
            }
            catch (TypeInitializationException ex)
            {
                string strErrorMessage = string.Format("{0}{1}", "An error occured while fetching the HighChart export option.", ex.Message);
                LogHelper.Error(_objType, ex, strErrorMessage);
                return null;
            }
            catch (FileNotFoundException ex)
            {
                string strErrorMessage = string.Format("{0}{1}", "An error occured while fetching the JSON file.", ex.Message);
                LogHelper.Error(_objType, ex, strErrorMessage);
                return null;
            }
        }
    }
}