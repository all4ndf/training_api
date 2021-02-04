using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace training.web.Models
{
    public class tblGeneralLedgerDetails
    {
        public int Id { get; set; }

        public Guid? FkIdtblGeneralLedgerMain { get; set; }

        public string AccountCode { get; set; }

        public decimal? Debit { get; set; }

        public decimal? Credit { get; set; }
    }
}