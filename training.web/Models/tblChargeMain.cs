using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace training.web.Models
{
    public class tblChargeMain
    {
        public Guid Id { get; set; }

        public string ChargeSlipNo { get; set; }

        public string DeptCode { get; set; }

        public string CaseNo { get; set; }

        public string AddedBy { get; set; }

        public DateTime? DateAdded { get; set; }
    }
}