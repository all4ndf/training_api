using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace training.web.Models
{
    public class TransactionStatus
    {
        //0-1
        public int status { get; set; }
        public string message { get; set; }
        public object param { get; set; }
    }
}