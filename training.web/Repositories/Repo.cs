using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using Microsoft.Ajax.Utilities;
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
               conn.Open();

               try
               {

                   using (var tr = conn.BeginTransaction())
                   {

                        if (x.Id == Guid.Empty)
                        {
                            var exist = conn.QueryFirstOrDefault("select top 1 * from tblUsers where username=@username", new
                            {
                                username = x.Username
                            }, commandType: CommandType.Text);

                            if (exist != null)
                            {
                                stat.status = 0;
                                stat.message = "Username already exist!";
                                return stat;
                            }


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

                                         }, commandType: CommandType.Text,transaction:tr);
                        }
                        else
                        {
                            conn.Execute("update tblUsers set Fullname=@Fullname," +
                                         "EmailAddress=@EmailAddress,MobileNo=@MobileNo,UpdatedBy=@UpdatedBy,DateUpdated=@DateUpdated where Id=@Id", new
                                         {
                                             Fullname = x.Fullname,
                                             EmailAddress = x.EmailAddress,
                                             MobileNo = x.MobileNo,
                                             UpdatedBy = x.AddedBy,
                                             DateUpdated = x.DateAdded,
                                             Id = x.Id


                                         }, commandType: CommandType.Text ,transaction:tr);
                        }

                        tr.Commit();
                        stat.status = 1;
                        stat.message = "User information successfully saved!";
                    }

                 //  var lst = conn.Query("select * from tblUsers",commandType:CommandType.Text);



                
               }
               catch (Exception e)
               {
                   stat.status = 0;
                   stat.message = e.Message;
               }



           }

           return stat;


        }

        public TransactionStatus SaveCharge(tblChargeMain x, List<tblChargeDetails> addedCharges)
        { 
            TransactionStatus stat = new TransactionStatus();

            using (IDbConnection conn = new SqlConnection(GetConnstr()))
            {

                try
                {

                    Guid Id  = Guid.NewGuid();
                 var result=   conn.ExecuteScalar("udp_SaveCharge",new
                    {
                        Id=Id,
                        DeptCode = x.DeptCode,
                        AddedBy = x.AddedBy,
                        DateAdded =DateTime.Now,
                        CaseNo=x.CaseNo
                    },commandType:CommandType.StoredProcedure);


                    foreach (var a in addedCharges)
                    {
                        conn.Execute("insert into tblChargeDetails(FkIdtblChargeMain,ItemId,Description,Price,Qty)" +
                                     "values(@FkIdtblChargeMain,@ItemId,@Description,@Price,@Qty)",new
                        {
                            FkIdtblChargeMain=Id,
                            ItemId=a.ItemId,
                            Description = a.Description,
                            Price=a.Price,
                            Qty = a.Qty

                        });
                    }



                    stat.status = 1;
                    stat.message =result.ToString();
                    




                }
                catch (Exception e)
                {
                    stat.status = 0;
                    stat.message = e.Message;
                }


            }


            return stat;
        }

        public TransactionStatus GetListOfUsers(ref List<Object> lst, string username, string fullName)
        {
           TransactionStatus stat = new TransactionStatus();
           try
           {
               using (IDbConnection conn = new SqlConnection(GetConnstr()))
               {
                   lst = conn.Query<Object>("udp_GetListOfUsers",new
                   {
                       username=username,
                       fullName=fullName
                   } ,commandType:CommandType.StoredProcedure).ToList();
               }
           }
           catch (Exception e)
           {
               stat.status = 0;
               stat.message = e.Message;
            }

           return stat;
        }

        public TransactionStatus GetUserDetails(ref object u, string Id)
        {
           TransactionStatus stat   = new TransactionStatus();

           try
           {

               using (IDbConnection conn = new SqlConnection(GetConnstr()))
               {

                   u = conn.QueryFirstOrDefault("udp_GetUserDetails",new
                   {
                       Id = Id
                   },commandType:CommandType.StoredProcedure);


               }

            }
            catch (Exception e)
            {
                stat.status = 0;
                stat.message = e.Message;
            }

           return stat;
        }
    }
}