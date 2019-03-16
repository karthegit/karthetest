using Aspose.Pdf;
using Aspose.Slides;
//using Aspose.Slides.Ppt;
using Ceb.Logger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;

namespace Ceb.MerlinTool.WebAPI
{
    public class HighChartsExportHelper
    {
        System.Type _objType = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chartJson"></param>
        /// <returns></returns>
        public string ExportChart(string chartJson)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            bool done = false;
            Dictionary<int, string> imageFiles = new Dictionary<int, string>();

            //HighChartExport.PanthomJsExport.BatchFile = Path.Combine(path, @"Scripts\phantomjs\chart_export.bat");
            //HighChartExport.PanthomJsExport.PhanthomJsLocation = Path.Combine(path, @"Scripts\phantomjs");
            //HighChartExport.PptExport.AsposeLicenseFile = Path.Combine(path, @"\Template\Aspose.Total.lic");

            List<string> chartImgFile = null;// HighChartExport.PanthomJsExport.Export(new List<string>() { chartJson });
            if (chartImgFile.Count > 0)
            {
                done = false;
                while (!done)
                {
                    if (File.Exists(chartImgFile[0]))
                    {
                        done = true;
                        break;
                    }
                    System.Threading.Thread.Sleep(1000);
                }
            }
            return chartImgFile[0];
        }

        //Export chart as PPT and PDF
        public bool ExportAllPPTX(string ExportFilePath, string imagePath, string title, string subtitle, string company, string type, string timePeriod, string page)
        {
            bool status = false;
            ITextFrame objTextFrame;
            Shape tempObjShape;
            string pdfPath;
            //string ExportFilePath = Path.Combine(ConfigurationManager.AppSettings["ExportFolderPath"], Guid.NewGuid().ToString() + ".pptx");

            try
            {
                string path = AppDomain.CurrentDomain.BaseDirectory;
                Aspose.Slides.License lic = new Aspose.Slides.License();
                lic.SetLicense(File.OpenRead(path + @"\Template\Aspose.Total.lic"));

                string pptxTemplate = path + @"\Template\ExportTemplate.pptx";
                Presentation pres = new Presentation(pptxTemplate);

                #region
                objTextFrame = ((IAutoShape)pres.Slides[0].FindShapeByAltText("Text Placeholder 5")).TextFrame;

                if (objTextFrame != null)
                {
                    if (page.Equals("Trend Analysis"))
                    {
                        if (subtitle.Contains("Time Period"))
                            timePeriod = (subtitle.Split(',')[0]).Split(':')[1].Trim();
                        else
                            timePeriod = "All";
                    }
                    objTextFrame.Text = objTextFrame.Text.Replace("<Time Period>", timePeriod).Replace("< Institution >", company);
                }

                objTextFrame = ((AutoShape)pres.Slides[1].FindShapeByAltText("title")).TextFrame;
                objTextFrame.Text = title;

                objTextFrame = ((AutoShape)pres.Slides[1].FindShapeByAltText("subtitle")).TextFrame;
                objTextFrame.Text = subtitle;

                #endregion

                tempObjShape =(Aspose.Slides.Shape)pres.Slides[1].FindShapeByAltText("chart");//Converted
                objTextFrame = ((AutoShape)tempObjShape).TextFrame;


                if (File.Exists(imagePath))
                {
                    objTextFrame.Text = String.Empty;
                    tempObjShape.FillFormat.FillType = FillType.Picture;
                    Aspose.Slides.IPPImage objImage = pres.Images.AddImage(new MemoryStream(readImageAsByte(imagePath)));
                    //Image objImage = pres.Slides[1].Parent.Images.AddImage(new MemoryStream(readImageAsByte(imagePath)));

                    tempObjShape.FillFormat.PictureFillFormat.PictureFillMode = PictureFillMode.Stretch;
                    tempObjShape.FillFormat.PictureFillFormat.Picture.Image = (Aspose.Slides.IPPImage)objImage;//Converted
                }
                pres.Save(ExportFilePath,Aspose.Slides.Export.SaveFormat.Pptx);

                if (type.Equals("pdf"))
                {
                    pdfPath = ExportFilePath.Replace(".pptx", ".pdf"); //Path.Combine(ConfigurationManager.AppSettings["ExportFolderPath"], Guid.NewGuid().ToString() + ".pdf");
                    pres.Save(pdfPath, Aspose.Slides.Export.SaveFormat.Pdf);
                }

                status = true;
            }
            catch (Exception ex)
            {
                LogHelper.Error(_objType, ex, "An error occured while Export PPT");
                status = false;
            }
            return status;
        }

        public byte[] readImageAsByte(string imagePath)
        {
            byte[] bytesChartImage = null;

            using (System.Drawing.Image image = new Bitmap(imagePath))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    bytesChartImage = ms.ToArray();
                }
            }

            return bytesChartImage;
        }

    }

}