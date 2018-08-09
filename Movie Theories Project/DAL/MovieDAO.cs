using DAL.Mapping;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace DAL
{
    public class MovieDAO
    {
        //Log errors
        private static ErrorLogger logger = new ErrorLogger();

        //Mapper
        private static MovieMapper mapper = new MovieMapper();

        //SQL Connection
        private readonly string connectionStrings;
        public MovieDAO(string connectionString)
        {
            connectionStrings = connectionString;
        }

        //View all movies
        public List<MovieDO> ViewAllMovies()
        {
            List<MovieDO> displayMovies = new List<MovieDO>();

            try
            {
                //Connecting and commanding to the SQL procedure.
                using (SqlConnection connection = new SqlConnection(connectionStrings))
                using (SqlCommand viewMovies = new SqlCommand("VIEW_ALL_MOVIES", connection))
                {
                    viewMovies.CommandType = CommandType.StoredProcedure;

                    connection.Open();

                    using (SqlDataReader sqlDataReader = viewMovies.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            MovieDO movie = mapper.MapReaderToSingle(sqlDataReader);

                            displayMovies.Add(movie);
                        }
                    }
                }
            }
            //For SQL Exceptions
            catch (SqlException sqlex)
            {
                logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, sqlex);
            }
            //For regular Exceptions.
            catch (Exception ex)
            {
                logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
            }
            return displayMovies;
        }

        //View one movie
        public MovieDO ViewMovie(long movieID)
        {
            MovieDO movie = new MovieDO();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStrings))
                using (SqlCommand viewOneMovie = new SqlCommand("VIEW_MOVIES", connection))
                {
                    viewOneMovie.CommandType = CommandType.StoredProcedure;

                    viewOneMovie.Parameters.AddWithValue("@MovieId", movieID);

                    connection.Open();

                    using (SqlDataReader reader = viewOneMovie.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            movie = mapper.MapReaderToSingle(reader);
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
            return movie;
        }

        //Add a new movie
        public void AddMovie(MovieDO movie)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStrings))
                using (SqlCommand movieAdding = new SqlCommand("ADD_MOVIE", connection))
                {
                    movieAdding.CommandType = CommandType.StoredProcedure;

                    //Parameters.
                    movieAdding.Parameters.AddWithValue("@Title", movie.Title);
                    movieAdding.Parameters.AddWithValue("@Director", movie.Director);
                    movieAdding.Parameters.AddWithValue("@MoneyMadeOpeningWeekend", movie.OpeningWeekend);
                    movieAdding.Parameters.AddWithValue("@YearReleased", movie.YearReleased);

                    connection.Open();

                    movieAdding.ExecuteNonQuery();
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

        //Update Movie
        public void UpdateMovie(MovieDO movie)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStrings))
                using (SqlCommand updateMovie = new SqlCommand("UPDATE_MOVIE", connection))
                {
                    updateMovie.CommandType = CommandType.StoredProcedure;

                    updateMovie.Parameters.AddWithValue("@MovieId", movie.MovieId);
                    updateMovie.Parameters.AddWithValue("@Title", movie.Title);
                    updateMovie.Parameters.AddWithValue("@Director", movie.Director);
                    updateMovie.Parameters.AddWithValue("@MoneyMadeOpeningWeekend", movie.OpeningWeekend);
                    updateMovie.Parameters.AddWithValue("@YearReleased", movie.YearReleased);

                    connection.Open();

                    updateMovie.ExecuteNonQuery();
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
        
        //Delete movie
        public void DeleteMovie(long movieId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionStrings))
                using (SqlCommand deleteMovie = new SqlCommand("DELETE_MOVIE", connection))
                {
                    deleteMovie.CommandType = CommandType.StoredProcedure;

                    deleteMovie.Parameters.AddWithValue("@MovieId", movieId);

                    connection.Open();

                    deleteMovie.ExecuteNonQuery();
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