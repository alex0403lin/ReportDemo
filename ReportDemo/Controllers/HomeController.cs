using Microsoft.Reporting.WebForms;
using ReportDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportDemo.Controllers
{
    public class HomeController : Controller
    {
        DemoCustomersReportEntities db = new DemoCustomersReportEntities();
        public ActionResult CustomersList()
        {
            return View(db.Customers.ToList());
        }

        public ActionResult Reports (string ReportType)
        {
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/Reports/CustomersReport.rdlc");

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "CustomersDataSet";
            reportDataSource.Value = db.Customers.ToList();
            localReport.DataSources.Add(reportDataSource);

            string reportType = ReportType;
            string mimeType;
            string encoding;
            string fileNameExtension;

            switch (reportType)
            {
                case "Excel":
                    fileNameExtension = "xlsx";
                    break;
                case "Word":
                    fileNameExtension = "docx";
                    break;
                case "PDF":
                    fileNameExtension = "pdf";
                    break;
                case "Image":
                    fileNameExtension = "jpg";
                    break;
                default:
                    break;
            }

            string[] streams;
            Warning[] warnings;
            byte[] rendredByte;
            rendredByte = localReport.Render(reportType, "", out mimeType, out encoding,
                out fileNameExtension, out streams, out warnings);
            Response.AddHeader("content-disposition", "attachment;filename= Customer_report." + fileNameExtension);

            return File(rendredByte, fileNameExtension); 
        }
    }
}