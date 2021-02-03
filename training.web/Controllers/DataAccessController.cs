using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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



    }
    }
