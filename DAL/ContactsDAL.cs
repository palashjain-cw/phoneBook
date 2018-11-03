using System;
using System.Collections.Generic;
using Entities;
using System.Data;
using MySql.Data.MySqlClient;
using Dapper;
using System.Configuration;

namespace DAL
{
    public class ContactsDAL
    {
        private string _connString = ConfigurationManager.ConnectionStrings["DatabaseConnect"].ConnectionString;
        private int _pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
        public bool AddContactDetail(ContactDetail contactDetail)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("mobile", contactDetail.Mobile);
                param.Add("Name", contactDetail.Name);
                param.Add("Email", contactDetail.Email);
                var query = @"Insert into ContactDetail (Mobile , Name, Email)
                              values(@mobile, @Name, @Email);";
                //var query = @"Create Table ContactDetail 
                //                (
                //                   Id int Primary Key Auto_Increment,
                //                    Mobile varchar(10) unique,
                //                    Name varchar(25),
                //                    Email varchar(30) unique)";



                using (IDbConnection conn = new MySqlConnection(_connString))
                {
                    return conn.Execute(query, param) > 0;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool DeleteContactDetail(string mobileNumber)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("mobile", mobileNumber);
                var query = @"DELETE FROM ContactDetail WHERE Mobile = @mobile";
                using (IDbConnection conn = new MySqlConnection(_connString))
                {
                    return conn.Execute(query, param, commandType: CommandType.Text) > 0;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<ContactDetail> GetAllContactDetail(int pageId)
        {
            try
            {
                int recordsFrom = (pageId - 1) * _pageSize;
                var param = new DynamicParameters();
                param.Add("offset", recordsFrom);
                param.Add("Limit", _pageSize);
                var query = @"select * from ContactDetail order by Id LIMIT @Limit OFFSET @offset";
                using (IDbConnection conn = new MySqlConnection(_connString))
                {
                    return conn.Query<ContactDetail>(query,param, commandType: CommandType.Text);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool UpdateContactDetail(string name, ContactDetail updatedDetail)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("name", name);
                param.Add("updatedName", updatedDetail.Name);
                param.Add("updatedMobile", updatedDetail.Mobile);
                param.Add("updatedEmail", updatedDetail.Email);
                var query = @"Update ContactDetail 
                                set Mobile = @updatedMobile,
                                Name = @updatedName,
                                Email = @updatedEmail
                                where Name = @name;";
                using (IDbConnection conn = new MySqlConnection(_connString))
                {
                    return conn.Execute(query, param, commandType: CommandType.Text) > 0;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<ContactDetail> SearchContactDetail(string searchString, int pageId)
        {
            try
            {
                int recordsFrom = (pageId - 1) * _pageSize;
                var param = new DynamicParameters();
                param.Add("searchitem", '%' + searchString + '%');
                param.Add("offset", recordsFrom);
                param.Add("Limit", _pageSize);
                var query = @"select * from ContactDetail 
                            where Name like @searchitem OR Email like @searchItem order by Id
                            LIMIT @Limit OFFSET @offset;";
                using (IDbConnection conn = new MySqlConnection(_connString))
                {
                    return conn.Query<ContactDetail>(query,param, commandType: CommandType.Text);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
