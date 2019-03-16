using Aspose.Pdf;
using Aspose.Pdf.Facades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ceb.MerlinTool.Controllers
{
    [SessionExpireFilterAttribute]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Overall()
        {
            ViewBag.Page = "Overall";
            return View();
        }

        [HttpPost]
        public ActionResult Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            //string ExportFilePath = Path.Combine(ConfigurationManager.AppSettings["ExportFolderPath"], Guid.NewGuid().ToString() + ".pdf");

            //ExportToPDF(ExportFilePath, "", "", "", "", fileContents, base64);

            return File(fileContents, contentType, fileName);
        }

        public bool ExportToPDF(string exportFilePath, string title, string subtitle, string company, string timePeriod, byte[] bufferData, string base64)
        {
            //Aspose.Pdf.License lic = new Aspose.Pdf.License();
            //lic.SetLicense(System.IO.File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + @"\Template\Aspose.Total.lic"));

            FileStream fs2 = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\Template\ExportTemplate.pdf", FileMode.Open, FileAccess.Read);

            byte[] buffer2 = bufferData;
            byte[] buffer1 = new byte[Convert.ToInt32(fs2.Length)];
            //int i = 0;
            //// Read PDF file contents into byte arrays
            //i = fs1.Read(buffer1, 0, Convert.ToInt32(fs1.Length));
            int i = fs2.Read(buffer1, 0, Convert.ToInt32(fs2.Length));

            // Now, first convert byte arrays into MemoryStreams and then concatenate those streams
            using (MemoryStream pdfStream = new MemoryStream())
            {
                using (MemoryStream fileStream1 = new MemoryStream(buffer1))
                {
                    using (MemoryStream fileStream2 = new MemoryStream(buffer2))
                    {
                        // Create instance of PdfFileEditor class to concatenate streams
                        PdfFileEditor pdfEditor = new PdfFileEditor();
                        // Concatenate both input MemoryStreams and save to putput MemoryStream
                        pdfEditor.Concatenate(fileStream1, fileStream2, pdfStream);
                        // Convert MemoryStream back to byte array
                        byte[] data = pdfStream.ToArray();
                        // Create a FileStream to save the output PDF file
                        FileStream output = new FileStream(exportFilePath, FileMode.Create,
                        FileAccess.Write);
                        // Write byte array contents in the output file stream
                        output.Write(data, 0, data.Length);
                        // Close output file
                        output.Close();
                    }
                }
            }

            fs2.Close();

            //byte[] bytes = Convert.FromBase64String(base64);
            //string newPdfFile = Path.Combine(ConfigurationManager.AppSettings["ExportFolderPath"], Guid.NewGuid().ToString() + ".pptx");
            //System.IO.FileStream stream = new FileStream(newPdfFile, FileMode.CreateNew);
            //System.IO.BinaryWriter writer = new BinaryWriter(stream);
            //writer.Write(bytes, 0, bytes.Length);
            //writer.Close();

            //Document pdfDocument2 = new Document(newPdfFile);
            //// Open second document
            //Document pdfDocument1 = new Document(AppDomain.CurrentDomain.BaseDirectory + @"\Template\ExportTemplate.pdf");

            //// Add pages of second document to the first
            //pdfDocument1.Pages.Add(pdfDocument2.Pages);

            //// Save concatenated output file
            //pdfDocument1.Save(Path.Combine(ConfigurationManager.AppSettings["ExportFolderPath"], Guid.NewGuid().ToString() + "final" + ".pdf"));
            return true;
        }

    }
}