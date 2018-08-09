using DAL.Mapping;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace DAL
{
    public class MovieTheoryDAO
    {
        //Error Logger
        private static ErrorLogger logger = new ErrorLogger();

        //Mapper
        private static MovieTheoryMapper mapper = new MovieTheoryMapper();

        //SQL Connection.
        private readonly string connectionStrings;
        public MovieTheoryDAO(string connectionString)
        {
            connectionStrings = connectionString;
        }

        //View all movie theories
        public List<MovieTheoryDO> AllMovieTheories()
        {
            //Return type
            List<MovieTheoryDO> displayMovieTheories = new List<MovieTheoryDO>();

            try
            {
                //Setting up connection to sql and making new command.
                using (SqlConnection connection = new SqlConnection(connectionStrings))
                using (SqlCommand viewMovies = new SqlCommand("VIEW_ALL_MOVIE_THEORIES", connection))
                {
                    viewMovies.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    //Setting up reader and using it.
                    using (SqlDataReader sqlDataReader = viewMovies.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            //Mapping to the DO from the reader.
                            MovieTheoryDO movieTheory = mapper.MapReaderToSingle(sqlDataReader);

                            displayMovieTheories.Add(movieTheory);
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
            return displayMovieTheories;
        }

        //View specific theories connected to one movie.
        public List<MovieTheoryDO> ViewAllMovieTheories(long movieID)
        {
            List<MovieTheoryDO> displayMovieTheories = new List<MovieTheoryDO>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStrings))
                using (SqlCommand viewTheoriesByMovieID = new SqlCommand("VIEW_MOVIE_THEORIES_BY_MOVIEID", connection))
                {
                    viewTheoriesByMovieID.CommandType = CommandType.StoredProcedure;

                    //Setting up parameters.
                    viewTheoriesByMovieID.Parameters.AddWithValue("@MovieID", movieID);

                    connection.Open();

                    using (SqlDataReader sqlDataReader = viewTheoriesByMovieID.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            MovieTheoryDO movieTheory = mapper.MapReaderToSingle(sqlDataReader);

                            displayMovieTheories.Add(movieTheory);
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

            return displayMovieTheories;
        }

        //View one movie theory.
        public MovieTheoryDO ViewMovieTheory(long movieTheoryID)
        {
            MovieTheoryDO movieTheory = new MovieTheoryDO();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStrings))
                using (SqlCommand viewOneTheory = new SqlCommand("VIEW_MOVIE_THEORIES", connection))
                {
                    viewOneTheory.CommandType = CommandType.StoredProcedure;

                    viewOneTheory.Parameters.AddWithValue("@MovieTheoryID", movieTheoryID);

                    connection.Open();

                    using (SqlDataReader reader = viewOneTheory.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            //Mapping to the DO
                            movieTheory = mapper.MapReaderToSingle(reader);
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
            return movieTheory;
        }

        //Add a movie theory.
        public void AddMovieTheory(MovieTheoryDO addMovieTheory)
        {
            //Returning nothing and DO required.

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStrings))
                using (SqlCommand addTheory = new SqlCommand("ADD_MOVIE_THEORY", connection))
                {
                    addTheory.CommandType = CommandType.StoredProcedure;

                    addTheory.Parameters.AddWithValue("@TitleOfTheory", addMovieTheory.TitleOfTheory);
                    addTheory.Parameters.AddWithValue("@Theory", addMovieTheory.Theory);
                    addTheory.Parameters.AddWithValue("@Status", addMovieTheory.Status);
                    addTheory.Parameters.AddWithValue("@MovieId", addMovieTheory.MovieId);

                    connection.Open();

                    //Allows it to perform the action.
                    addTheory.ExecuteNonQuery();
                }

            }
            catch (SqlException sqlex)
            {
                logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);
            }
        }

        //Update theory.
        public void UpdateMovieTheory(MovieTheoryDO movieTheory)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStrings))
                using (SqlCommand updateTheory = new SqlCommand("UPDATE_MOVIE_THEORIES", connection))
                {
                    updateTheory.CommandType = CommandType.StoredProcedure;

                    updateTheory.Parameters.AddWithValue("@MovieTheoryId", movieTheory.MovieTheoryID);
                    updateTheory.Parameters.AddWithValue("@TitleOfTheory", movieTheory.TitleOfTheory);
                    updateTheory.Parameters.AddWithValue("@Theory", movieTheory.Theory);
                    updateTheory.Parameters.AddWithValue("@Status", movieTheory.Status);
                    updateTheory.Parameters.AddWithValue("@MovieId", movieTheory.MovieId);

                    connection.Open();

                    updateTheory.ExecuteNonQuery();
                }

            }
            catch (SqlException sqlex)
            {
                logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);
            }
        }

        //Delete theory by id.
        public void DeleteMovieTheory(long movieTheoryId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStrings))
                using (SqlCommand deleteTheory = new SqlCommand("DELETE_MOVIE_THEORY", connection))
                {
                    deleteTheory.CommandType = CommandType.StoredProcedure;

                    deleteTheory.Parameters.AddWithValue("@MovieTheoryId", movieTheoryId);

                    connection.Open();

                    deleteTheory.ExecuteNonQuery();
                }

            }
            catch (SqlException sqlex)
            {
                logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);
            }
        }
    }
}
