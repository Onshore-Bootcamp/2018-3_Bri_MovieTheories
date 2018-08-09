using DAL;
using DAL.Models;
using Movie_Theories_Project.Mapping;
using Movie_Theories_Project.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Web.Mvc;

namespace Movie_Theories_Project.Controllers
{
    public class MovieController : Controller
    {
        //Logger
        private static ErrorLogger logger = new ErrorLogger();

        //Map.
        private static MovieMapper mapper = new MovieMapper();

        //Data Access.
        private readonly MovieDAO movieDataAccess;
        public MovieController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DataSource"].ConnectionString;

            movieDataAccess = new MovieDAO(connectionString);
        }

        //All movies show.
        public ActionResult Index()
        {
            ActionResult response;

            //Anyone with an acocunt can see all movies.
            if (Session["Role"] != null)
            {
                try
                {
                    List<MovieDO> allMovies = movieDataAccess.ViewAllMovies();
                    List<MoviePO> mappedMovies = new List<MoviePO>();

                    //Map for all movies.
                    foreach (MovieDO dataObject in allMovies)
                    {
                        mappedMovies.Add(mapper.MapDoToPo(dataObject));
                    }
                    response = View(mappedMovies);
                }
                catch (Exception ex)
                {
                    logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                    response = RedirectToAction("Index", "Home");
                }
            }
            else
            {
                response = RedirectToAction("Register", "Account");
            }
            return response;
        }

        //Details for movie details.
        public ActionResult MovieDetails(long id)
        {
            ActionResult response;

            //Anyone with an account can see movie details.
            if (Session["Role"] != null)
            {
                if (id > 0)
                {
                    try
                    {
                        //Map for one single movie for details.
                        MovieDO movieDO = movieDataAccess.ViewMovie(id);
                        MoviePO moviePO = mapper.MapDoToPo(movieDO);

                        response = View(moviePO);
                    }
                    catch (Exception ex)
                    {
                        logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                        response = RedirectToAction("Index", "Movie");
                    }
                }
                else
                {
                    response = RedirectToAction("Index", "Movie");
                }
            }
            else
            {
                response = RedirectToAction("Register", "Account");
            }
            return response;
        }

        //The get update info for movies
        [HttpGet]
        public ActionResult UpdateMovie(long id)
        {
            ActionResult response;

            //Mods and Admins can update movies.
            if (Session["Role"] != null)
            {
                if ((int)Session["Role"] != 1 && id > 0)
                {
                    try
                    {
                        MovieDO movieDO = movieDataAccess.ViewMovie(id);
                        MoviePO moviePO = mapper.MapDoToPo(movieDO);

                        response = View(moviePO);
                    }
                    catch (Exception ex)
                    {
                        logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                        response = RedirectToAction("Index", "Movie");
                    }
                }
                else
                {
                    response = RedirectToAction("Index", "Movie");
                }
            }
            else
            {
                response = RedirectToAction("Register", "Account");
            }
            return response;
        }

        //Post update info for movies.
        [HttpPost]
        public ActionResult UpdateMovie(MoviePO form)
        {
            ActionResult response;

            //Admins and Mods can update movies.
            if (Session["Role"] != null)
            {
                if ((int)Session["Role"] != 1)
                {
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            MovieDO movieDO = mapper.MapPoToDo(form);
                            movieDataAccess.UpdateMovie(movieDO);

                            response = RedirectToAction("Index", "Movie");
                        }
                        catch (Exception ex)
                        {
                            logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                            response = RedirectToAction("Index", "Movie");
                        }
                    }
                    else
                    {
                        response = View(form);
                    }
                }
                else
                {
                    response = RedirectToAction("Index", "Movie");
                }
            }
            else
            {
                response = RedirectToAction("Register", "Account");
            }
            return response;
        }

        //Get adding info for a new movie.
        [HttpGet]
        public ActionResult AddMovie()
        {
            ActionResult response;

            //Anyone with an account can add movies.
            if (Session["Role"] != null)
            {
                try
                {
                    response = View();
                }
                catch (Exception ex)
                {
                    logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                    response = RedirectToAction("Index", "Movie");
                }
            }
            else
            {
                response = RedirectToAction("Register", "Account");
            }
            return response;
        }

        //Post added info for a new movie.
        [HttpPost]
        public ActionResult AddMovie(MoviePO form)
        {
            ActionResult response;

            //Anyone with an account can add a movie.
            if (Session["Role"] != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        MovieDO newMovie = mapper.MapPoToDo(form);
                        movieDataAccess.AddMovie(newMovie);

                        response = RedirectToAction("Index", "Movie");
                    }
                    catch (Exception ex)
                    {
                        logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                        response = RedirectToAction("Index", "Movie");
                    }
                }
                else
                {
                    response = View(form);
                }
            }
            else
            {
                response = RedirectToAction("Register", "Account");
            }
            return response;
        }

        //Getting details for movies.
        [HttpGet]
        public ActionResult DeleteMovie(int id)
        {
            ActionResult response;

            //Only Admins can delete movies.
            if (Session["Role"] != null)
            {
                if ((int)Session["Role"] == 3 && id > 0)
                {
                    try
                    {
                        MovieDO movie = movieDataAccess.ViewMovie(id);
                        MoviePO deletedMovie = mapper.MapDoToPo(movie);
                        movieDataAccess.DeleteMovie(deletedMovie.MovieId);

                        response = RedirectToAction("Index", "Movie");
                    }
                    catch (Exception ex)
                    {
                        logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                        response = RedirectToAction("Index", "Movie");
                    }
                }
                else
                {
                    response = RedirectToAction("Index", "Movie");
                }
            }
            else
            {
                response = RedirectToAction("Register", "Account");
            }
            return response;
        }
    }
}