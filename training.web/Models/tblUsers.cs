using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace training.web.Models
{
    public class tblUsers
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Fullname { get; set; }

        public string EmailAddress { get; set; }

        public string MobileNo { get; set; }

        public string AddedBy { get; set; }

        public DateTime? DateAdded { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? DateUpdated { get; set; }

        public string DeletedBy { get; set; }

        public DateTime? DateDeleted { get; set; }


    }
}