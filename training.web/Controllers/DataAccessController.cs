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
            x.Id = data.Id == null ? Guid.Empty : new Guid(data.Id.Value.ToString());
            x.Username =  data.Username == null ? "" : data.Username.Value;
            x.Fullname = data.Fullname == null ? "" : data.Fullname.Value;
            x.EmailAddress = data.EmailAddress == null ? "" : data.EmailAddress.Value;
            x.MobileNo = data.MobileNo == null ? "" : data.MobileNo.Value;
            x.AddedBy = "comlogik";
            x.DateAdded = DateTime.Now;
            TransactionStatus stat = new TransactionStatus();


          
                stat = repo.SavePatientInformation(x);
            
       

            

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
                message = stat.message,
                param=stat.param
            });



        }



        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/getlistofusers")]

        public HttpResponseMessage GetListOfUsers() {

            var queryItems = Request.RequestUri.ParseQueryString();


            TransactionStatus stat = new TransactionStatus();
            string username = queryItems["UserName"] == null ? "" : queryItems["UserName"];
            string fullName = queryItems["FullName"] == null ? "" : queryItems["FullName"];
        



            List<Object> lst = new List<Object>();
            stat = repo.GetListOfUsers(ref lst, username,fullName);

            return Request.CreateResponse(HttpStatusCode.OK, lst);


        }


        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("api/getuserdetails")]

        public HttpResponseMessage GetUserDetails()
        {

            var queryItems = Request.RequestUri.ParseQueryString();


            TransactionStatus stat = new TransactionStatus();
            string Id = queryItems["Id"] == null ? "" : queryItems["Id"];





            Object u = null;
            stat = repo.GetUserDetails(ref u, Id);



            return Request.CreateResponse(HttpStatusCode.OK, u);


        }

    }
}
