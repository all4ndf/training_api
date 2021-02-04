using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace training.web.Models
{
    public class tblGeneralLedgerMain
    {
        public Guid Id { get; set; }

        public string SourceDoc { get; set; }

        public string AddedBy { get; set; }

        public string DateAdded { get; set; }

        public string UpdatedBy { get; set; }

        public string DateUpdated { get; set; }

        public string DeletedBy { get; set; }

        public string DateDeleted { get; set; }
    }
}