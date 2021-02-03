using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using training.web.Models;

namespace training.web.Repositories
{
    public class Repo : IRepo

    {

        private string GetConnstr()
        {
            string serverName = ConfigurationManager.AppSettings["serverName"];
            string databaseName = ConfigurationManager.AppSettings["databaseName"];


            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = serverName;
            builder.UserID = "training";
            builder.Password = "training";
            builder.InitialCatalog = databaseName;
            builder.MaxPoolSize = 5000;
            builder.MultipleActiveResultSets = true;

            return builder.ConnectionString;
        }

        public TransactionStatus SavePatientInformation(tblUsers x)
        {
           TransactionStatus stat  =new TransactionStatus();

           using (IDbConnection conn = new SqlConnection(GetConnstr()))
           {

               try
               {

                   conn.Execute("insert into tblUsers(Id,Username,Fullname,EmailAddress,MobileNo,AddedBy,DateAdded)" +
                                "values(@Id,@Username,@Fullname,@EmailAddress,@MobileNo,@AddedBy,@DateAdded)", new
                   {
                       Id = Guid.NewGuid(),
                       Username = x.Username,
                       Fullname = x.Fullname,
                       EmailAddress = x.EmailAddress,
                       MobileNo = x.MobileNo,
                       AddedBy = x.AddedBy,
                       DateAdded = x.DateAdded

                   }, commandType: CommandType.Text);
                   stat.status = 1;
                   stat.message = "User information successfully saved!";
               }
               catch (Exception e)
               {
                   stat.status = 0;
                   stat.message = e.Message;
               }



           }

           return stat;


        }
    }
}