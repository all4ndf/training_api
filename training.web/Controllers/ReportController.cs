using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using training.web.Models;

namespace training.web.Controllers
{


    public class ReportController : ApiController
    {




        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/downloadreport")]
        public IHttpActionResult GetReport()
        {
            var queryItems = Request.RequestUri.ParseQueryString();
            TransactionStatus stat = new TransactionStatus();
            string reportName = queryItems["reportName"] == null ? "" : queryItems["reportName"];
            string param1 = queryItems["param1"] == null ? "" : queryItems["param1"];
            bool excel = queryItems["isExcel"] == null ? false : Convert.ToBoolean(queryItems["isExcel"]);


            ReportDocument rd = InitializeReport(reportName);
   
            IHttpActionResult response = null;
            if (reportName == "chargeslip2.rpt")
            {
                rd.SetParameterValue("@Id", "{" + param1 + "}");
                rd.SetParameterValue("@Id", "{" + param1 + "}","chargedetails");
                rd.SetParameterValue("testParameter", "sdfsfsdf");
                
            }
            Stream stream = rd.ExportToStream(!excel ? CrystalDecisions.Shared.ExportFormatType.PortableDocFormat : CrystalDecisions.Shared.ExportFormatType.Excel);

            MemoryStream ms = new MemoryStream();

            stream.CopyTo(ms);
            rd.Close();
            rd.Dispose();

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(ms.GetBuffer())
            };
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = "test.pdf"
            };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            response = ResponseMessage(result);
            return response;

        }

        private ReportDocument InitializeReport(string reportName)
        {
            string serverName = ConfigurationManager.AppSettings["serverName"];
            string databaseName = ConfigurationManager.AppSettings["databaseName"];
            string username = "training";
            string password = "training";
            ReportDocument rd = new ReportDocument();
            rd.Load(HttpContext.Current.Server.MapPath("~/reports/" + reportName));
            ConnectionInfo connectInfo = new ConnectionInfo()
            {
                ServerName = serverName,
                DatabaseName = databaseName,
                UserID = username,
                Password = password
            };
            rd.SetDatabaseLogon(username, password);
            Tables CrTables;
            CrTables = rd.Database.Tables;
            TableLogOnInfos crtableLogoninfos = new TableLogOnInfos();
            TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
            ConnectionInfo crConnectionInfo = new ConnectionInfo();

            crConnectionInfo.ServerName = serverName;
            crConnectionInfo.DatabaseName = databaseName;
            crConnectionInfo.UserID = username;
            crConnectionInfo.Password = password;

            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            {
                crtableLogoninfo = CrTable.LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                CrTable.ApplyLogOnInfo(crtableLogoninfo);
            }

            Sections crSections;
            crSections = rd.ReportDefinition.Sections;
            ReportObjects crReportObjects;
            SubreportObject crSubreportObject;
            ReportDocument crSubreportDocument;
            CrystalDecisions.CrystalReports.Engine.Database crDatabase;
            Tables crTables;
            TableLogOnInfo crTableLogOnInfo;
            foreach (Section crSection in crSections)
            {
                crReportObjects = crSection.ReportObjects;
                //loop through all the report objects in there to find all subreports 
                foreach (ReportObject crReportObject in crReportObjects)
                {
                    if (crReportObject.Kind == ReportObjectKind.SubreportObject)
                    {
                        crSubreportObject = (SubreportObject)crReportObject;
                        //open the subreport object and logon as for the general report 
                        crSubreportDocument = crSubreportObject.OpenSubreport(crSubreportObject.SubreportName);
                        crDatabase = crSubreportDocument.Database;
                        crTables = crDatabase.Tables;
                        foreach (CrystalDecisions.CrystalReports.Engine.Table aTable in crTables)
                        {
                            crTableLogOnInfo = aTable.LogOnInfo;
                            crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                            aTable.ApplyLogOnInfo(crTableLogOnInfo);
                        }
                    }

                }


            }

            return rd;
        }

    }
}
