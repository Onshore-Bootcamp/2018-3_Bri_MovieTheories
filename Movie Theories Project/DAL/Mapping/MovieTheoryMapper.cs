using DAL.Models;
using System;
using System.Data.SqlClient;

namespace DAL.Mapping
{
    class MovieTheoryMapper
    {
        public MovieTheoryDO MapReaderToSingle(SqlDataReader reader)
        {
            MovieTheoryDO result = new MovieTheoryDO();

            //Checks to make sure there is some sort of value.
            if (reader["MovieTheoryId"] != DBNull.Value)
            {
                result.MovieTheoryID = (long)reader["MovieTheoryId"];
            }
            if (reader["TitleOfTheory"] != DBNull.Value)
            {
                result.TitleOfTheory = (string)reader["TitleOfTheory"];
            }
            if (reader["Theory"] != DBNull.Value)
            {
                result.Theory = (string)reader["Theory"];
            }
            if (reader["Status"] != DBNull.Value)
            {
                result.Status = (string)reader["Status"];
            }
            if (reader["MovieId"] != DBNull.Value)
            {
                result.MovieId = (long)reader["MovieId"];
            }

            return result;
        }
    }
}
