using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Antlr.Runtime.Misc;
using Newtonsoft.Json;
using training.web.Models;
using training.web.Repositories;

namespace training.web.Controllers
{
    public class DataAccessController : ApiController
    {

        private IRepo repo = new Repo();

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/saveuserinformation")]
        public HttpResponseMessage SaveUserInformation([FromBody] dynamic data)
        {

            tblUsers x =new tblUsers();
            x.Username =  data.Username == null ? "" : data.Username.Value;
            x.Fullname = data.Fullname == null ? "" : data.Fullname.Value;
            x.EmailAddress = data.EmailAddress == null ? "" : data.EmailAddress.Value;
            x.MobileNo = data.MobileNo == null ? "" : data.MobileNo.Value;
            x.AddedBy = "comlogik";
            x.DateAdded = DateTime.Now;
           

            TransactionStatus stat = repo.SavePatientInformation(x);

            

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
               stat = stat.status,
               message = stat.message
            });



        }




        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/savecharge")]
        public HttpResponseMessage SaveCharge([FromBody] dynamic data)
        {

            tblChargeMain x = new tblChargeMain();
            x.CaseNo = data.CaseNo == null ? "" : data.CaseNo.Value;
            x.DeptCode = data.DeptCode == null ? "" : data.DeptCode.Value;
            x.AddedBy = "comlogik";
            x.DateAdded = DateTime.Now;

            List<tblChargeDetails> addedCharges  = new List<tblChargeDetails>();

            var charges = data.addedCharges;

            foreach (var a in charges)
            {
                tblChargeDetails y = new tblChargeDetails();
                y.Description = a.Description.ToString();
                y.ItemId =a.ItemId.ToString();
                y.Qty= Convert.ToInt32(a.Quantity.ToString());
                y.Price = Convert.ToDecimal(a.Price.ToString());

                addedCharges.Add(y);

            }








            TransactionStatus stat = repo.SaveCharge(x, addedCharges);



            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                stat = stat.status,
                message = stat.message
            });



        }



    }
}
