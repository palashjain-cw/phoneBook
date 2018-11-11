using System;
using System.Collections.Generic;
using Entities;
using System.Data;
using MySql.Data.MySqlClient;
using Dapper;
using System.Configuration;
using Interface;

namespace DAL
{
    public class ContactsDAL : IContactsDetailsDAL
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


        public bool DeleteContactDetail(string name)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("name", name);
                var query = @"DELETE FROM ContactDetail WHERE Name = @name";
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

        public List<ContactDetail> GetAllContactDetail(char startingChar)
        {
            try
            {
                var param = new DynamicParameters();
                param.Add("startingChar", startingChar);
                var query = @"select * from ContactDetail where LEFT(Name , 1) = @startingchar order by Name;";
                using (IDbConnection conn = new MySqlConnection(_connString))
                {
                    return conn.Query<ContactDetail>(query,param, commandType: CommandType.Text).AsList();
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
        public List<CharCountMapping> GetContactCountByChar()
        {
            try
            {
               
                var query = @"SELECT
                            LEFT(Name, 1) as InitialChar,
                            COUNT(*) AS Total
                            FROM ContactDetail
                            GROUP BY LEFT(Name, 1)
                            Order By Left(Name, 1);";
                using (IDbConnection conn = new MySqlConnection(_connString))
                {
                    return conn.Query<CharCountMapping>(query, commandType: CommandType.Text).AsList();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
