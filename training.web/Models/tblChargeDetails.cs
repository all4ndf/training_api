using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace training.web.Models
{
    public class tblChargeDetails
    {
        public int Id { get; set; }

        public string ItemId { get; set; }
        public Guid? FkIdtblChargeMain { get; set; }

        public string Description { get; set; }

        public decimal? Price { get; set; }

        public decimal? Qty { get; set; }

    }
}