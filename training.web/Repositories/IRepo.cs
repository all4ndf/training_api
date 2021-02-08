using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using training.web.Models;

namespace training.web.Repositories
{
    interface IRepo
    {
        TransactionStatus SavePatientInformation(tblUsers x);
        TransactionStatus SaveCharge(tblChargeMain x, List<tblChargeDetails> addedCharges);
        TransactionStatus GetListOfUsers(ref List<Object> lst, string username, string fullName);
        TransactionStatus GetUserDetails(ref Object u, string Id);
    }
}
