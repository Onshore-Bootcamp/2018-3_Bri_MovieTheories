using DAL.Mapping;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace DAL
{
    public class UserDAO
    {
        //Error logging
        private static ErrorLogger logger = new ErrorLogger();

        //Mapper
        private static UserMapper mapper = new UserMapper();

        //SQL Connection
        private readonly string connectionStrings;
        public UserDAO(string connectionString)
        {
            connectionStrings = connectionString;
        }

        //View all users.
        public List<UserDO> ViewAllUsers()
        {
            List<UserDO> displayUsers = new List<UserDO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStrings))
                using (SqlCommand viewUsers = new SqlCommand("VIEW_ALL_USERS", connection))
                {
                    viewUsers.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader sqlDataReader = viewUsers.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            UserDO user = mapper.MapReaderToSingle(sqlDataReader);

                            displayUsers.Add(user);
                        }
                    }
                }
            }
            catch (SqlException sqlex)
            {
                logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);
            }
            catch (Exception ex)
            {
                logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
            }
            return displayUsers;
        }

        //View one user by username for logging in.
        public UserDO ViewUser(string username)
        {
            UserDO user = new UserDO();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStrings))
                using (SqlCommand command = new SqlCommand("VIEW_USER", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Username", username);

                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            //Ternary to get proper values for the user.
                            user.UserId = reader["UserId"] != DBNull.Value ? (long)reader["UserId"] : 0;
                            user.Username = reader["Username"] != DBNull.Value ? (string)reader["Username"] : null;
                            user.Password = reader["Password"] != DBNull.Value ? (string)reader["Password"] : null;
                            user.FirstName = reader["FirstName"] != DBNull.Value ? (string)reader["FirstName"] : null;
                            user.Email = reader["Email"] != DBNull.Value ? (string)reader["Email"] : null;
                            user.Role = reader["Role"] != DBNull.Value ? (int)reader["Role"] : 0;
                            user.RoleName = reader["RoleName"] != DBNull.Value ? (string)reader["RoleName"] : null;
                        }
                    }
                }
            }
            catch (SqlException sqlex)
            {
                logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);
            }
            catch (Exception ex)
            {
                logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
            }
            return user;
        }

        //View user by id.
        public UserDO ViewUserByID(long userID)
        {
            UserDO user = new UserDO();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStrings))
                using (SqlCommand viewOneTheory = new SqlCommand("VIEW_USER_BY_ID", connection))
                {
                    viewOneTheory.CommandType = CommandType.StoredProcedure;

                    viewOneTheory.Parameters.AddWithValue("@UserId", userID);

                    connection.Open();

                    using (SqlDataReader reader = viewOneTheory.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            user = mapper.MapReaderToSingle(reader);
                        }
                    }
                }
            }
            catch (SqlException sqlex)
            {
                logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);
            }
            catch (Exception ex)
            {
                logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
            }
            return user;
        }

        //Add a new user
        public void RegisterUser(UserDO userRegister)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStrings))
                using (SqlCommand command = new SqlCommand("REGISTER_USER", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Username", userRegister.Username);
                    command.Parameters.AddWithValue("@Password", userRegister.Password);
                    command.Parameters.AddWithValue("@FirstName", userRegister.FirstName);
                    command.Parameters.AddWithValue("@Email", userRegister.Email);
                    command.Parameters.AddWithValue("@RoleName", userRegister.RoleName);

                    connection.Open();

                    command.ExecuteNonQuery();
                }

            }
            catch (SqlException sqlex)
            {
                logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);
            }
            catch (Exception ex)
            {
                logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        //Update a user
        public void UpdateUser(UserDO userUpdate)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStrings))
                using (SqlCommand command = new SqlCommand("UPDATE_USERS", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserId", userUpdate.UserId);
                    command.Parameters.AddWithValue("@Username", userUpdate.Username);
                    command.Parameters.AddWithValue("@Password", userUpdate.Password);
                    command.Parameters.AddWithValue("@FirstName", userUpdate.FirstName);
                    command.Parameters.AddWithValue("@Email", userUpdate.Email);
                    command.Parameters.AddWithValue("@Role", userUpdate.Role);
                    command.Parameters.AddWithValue("@RoleName", userUpdate.RoleName);

                    connection.Open();

                    command.ExecuteNonQuery();
                }

            }
            catch (SqlException sqlex)
            {
                logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);
            }
            catch (Exception ex)
            {
                logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
            }
        }

        //Delete by id.
        public void DeleteUser(long userID)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStrings))
                using (SqlCommand deleteUser = new SqlCommand("DELETE_USER", connection))
                {
                    deleteUser.CommandType = CommandType.StoredProcedure;

                    deleteUser.Parameters.AddWithValue("@UserId", userID);

                    connection.Open();

                    deleteUser.ExecuteNonQuery();
                }

            }
            catch (SqlException sqlex)
            {
                logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);
            }
            catch (Exception ex)
            {
                logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
            }
        }
    }
}
