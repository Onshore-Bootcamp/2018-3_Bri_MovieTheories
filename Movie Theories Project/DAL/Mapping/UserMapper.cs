using DAL.Models;
using System;
using System.Data.SqlClient;

namespace DAL.Mapping
{
    public class UserMapper
    {
        public UserDO MapReaderToSingle(SqlDataReader reader)
        {
            UserDO result = new UserDO();

            //Checks to make sure there is some sort of value.
            if (reader["UserId"] != DBNull.Value)
            {
                result.UserId = (long)reader["UserId"];
            }
            if (reader["Username"] != DBNull.Value)
            {
                result.Username = (string)reader["Username"];
            }
            if (reader["Password"] != DBNull.Value)
            {
                result.Password = (string)reader["Password"];
            }
            if (reader["FirstName"] != DBNull.Value)
            {
                result.FirstName = (string)reader["FirstName"];
            }
            if (reader["Email"] != DBNull.Value)
            {
                result.Email = (string)reader["Email"];
            }
            if (reader["Role"] != DBNull.Value)
            {
                result.Role = (int)reader["Role"];
            }
            if (reader["RoleName"] != DBNull.Value)
            {
                result.RoleName = (string)reader["RoleName"];
            }

            return result;
        }
    }
}
