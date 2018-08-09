using DAL.Models;
using System;
using System.Data.SqlClient;

namespace DAL.Mapping
{
    class MovieMapper
    {
        public MovieDO MapReaderToSingle(SqlDataReader reader)
        {
            MovieDO result = new MovieDO();

            //Checks to make sure there is some sort of value.
            if (reader["MovieId"] != DBNull.Value)
            {
                result.MovieId = (long)reader["MovieId"];
            }
            if (reader["Title"] != DBNull.Value)
            {
                result.Title = (string)reader["Title"];
            }
            if (reader["Director"] != DBNull.Value)
            {
                result.Director = (string)reader["Director"];
            }
            if (reader["MoneyMadeOpeningWeekend"] != DBNull.Value)
            {
                result.OpeningWeekend = (string)reader["MoneyMadeOpeningWeekend"];
            }
            if (reader["YearReleased"] != DBNull.Value)
            {
                result.YearReleased = (short)reader["YearReleased"];
            }
            return result;
        }
    }
}
