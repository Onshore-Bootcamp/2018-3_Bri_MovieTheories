using DAL;
using DAL.Models;
using Movie_Theories_Project.Mapping;
using Movie_Theories_Project.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using System.Web.Mvc;
using MovieProjectBLL.Model;
using MovieProjectBLL;

namespace Movie_Theories_Project.Controllers
{
    public class MovieTheoryController : Controller
    {
        //Logger
        private static ErrorLogger logger = new ErrorLogger();

        //Mapper.
        private static MovieTheoryMapper mapper = new MovieTheoryMapper();

        //Logic Layer
        private static StatusLogic statusLogic = new StatusLogic();

        //Data Access
        private readonly MovieTheoryDAO movieTheoryDataAccess;
        public MovieTheoryController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DataSource"].ConnectionString;

            movieTheoryDataAccess = new MovieTheoryDAO(connectionString);
        }

        //All movie theories.
        public ActionResult AllMovieTheories()
        {
            //Return type.
            ActionResult response;

            try
            {
                //Access to all theories in the DAO.
                List<MovieTheoryDO> allMovieTheories = movieTheoryDataAccess.AllMovieTheories();
                List<MovieTheoryPO> mappedMovieTheories = new List<MovieTheoryPO>();

                //Foreach theory in all the theories to map.
                foreach (MovieTheoryDO dataObject in allMovieTheories)
                {
                    mappedMovieTheories.Add(mapper.MapDoToPo(dataObject));
                }

                //Call to get the meaningful calculation.
                List<KeyValuePair<string, float>> statusPercentages = CountStatus();

                response = View(mappedMovieTheories);

            }
            catch (Exception ex)
            {
                logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                response = RedirectToAction("Login", "Account");
            }
            return response;
        }

        //Index of movie theories connected to movies.
        public ActionResult MovieTheoryIndex(long id)
        {
            ActionResult response;

            //Anyone with an account can see the theories specifically connected to movies.
            if (Session["Role"] != null)
            {
                //Check id
                if (id > 0)
                {
                    try
                    {
                        //Accessing the movie theories sharing the same movie id.
                        List<MovieTheoryDO> allMovieTheories = movieTheoryDataAccess.ViewAllMovieTheories(id);
                        List<MovieTheoryPO> mappedMovieTheories = new List<MovieTheoryPO>();

                        foreach (MovieTheoryDO dataObject in allMovieTheories)
                        {
                            mappedMovieTheories.Add(mapper.MapDoToPo(dataObject));
                        }

                        //Getting Movie ID.
                        MoviePO moviePO = new MoviePO();
                        moviePO.MovieTheories = mappedMovieTheories;
                        moviePO.MovieId = id;

                        response = View(moviePO);
                    }
                    catch (Exception ex)
                    {
                        logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                        response = RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    response = RedirectToAction("Index", "Home");
                }
            }
            else
            {
                response = RedirectToAction("Register", "Account");
            }
            return response;
        }

        //Details that everyone to see.
        public ActionResult DetailsForAllTheories(long id)
        {
            ActionResult response;
            
            if (id > 0)
            {
                try
                {
                    //Shows specific movie theory details.
                    MovieTheoryDO movieTheoryDO = movieTheoryDataAccess.ViewMovieTheory(id);
                    MovieTheoryPO movieTheoryPO = mapper.MapDoToPo(movieTheoryDO);

                    response = View(movieTheoryPO);
                }
                catch (Exception ex)
                {
                    logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                    response = RedirectToAction("Index", "Home");
                }
            }
            else
            {
                response = RedirectToAction("Index", "Home");
            }
            return response;
        }

        //Get update info for theories.
        [HttpGet]
        public ActionResult UpdateMovieTheory(long id)
        {
            ActionResult response;

            if (Session["Role"] != null)
            {
                //Admins and Mods can update theories.
                if ((int)Session["Role"] != 1 && id > 0)
                {
                    try
                    {
                        MovieTheoryDO movieTheoryDO = movieTheoryDataAccess.ViewMovieTheory(id);
                        MovieTheoryPO movieTheoryPO = mapper.MapDoToPo(movieTheoryDO);


                        //ViewBag drop down list for status.
                        ViewBag.StatusDropDown = new List<SelectListItem>();
                        ViewBag.StatusDropDown.Add(new SelectListItem { Text = "Tinfoilly", Value = "Tinfoilly" });
                        ViewBag.StatusDropDown.Add(new SelectListItem { Text = "Perchance", Value = "Perchance" });
                        ViewBag.StatusDropDown.Add(new SelectListItem { Text = "Believable", Value = "Believable" });

                        response = View(movieTheoryPO);
                    }
                    catch (Exception ex)
                    {
                        logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                        response = RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    response = RedirectToAction("Index", "Home");
                }
            }
            else
            {
                response = RedirectToAction("Register", "Account");
            }
            return response;
        }

        //Post updated movie theory info.
        [HttpPost]
        public ActionResult UpdateMovieTheory(MovieTheoryPO form)
        {
            ActionResult response;

            //Mods and Admins can update movie theories.
            if (Session["Role"] != null)
            {
                if ((int)Session["Role"] != 1)
                {
                    //Making sure there are no null in required fields.
                    if (ModelState.IsValid)
                    {
                        try
                        {
                            MovieTheoryDO movieTheoryDO = mapper.MapPoToDo(form);
                            movieTheoryDataAccess.UpdateMovieTheory(movieTheoryDO);

                            //Keeping the dropdown list in case something goes wrong.
                            ViewBag.StatusDropDown = new List<SelectListItem>();
                            ViewBag.StatusDropDown.Add(new SelectListItem { Text = "Tinfoilly", Value = "Tinfoilly" });
                            ViewBag.StatusDropDown.Add(new SelectListItem { Text = "Perchance", Value = "Perchance" });
                            ViewBag.StatusDropDown.Add(new SelectListItem { Text = "Believable", Value = "Believable" });

                            //Id of movie needed to make sure the correct theory was updated on the correct movie.
                            response = RedirectToAction("MovieTheoryIndex", "MovieTheory", new { id = form.MovieId });
                        }
                        catch (Exception ex)
                        {
                            logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                            response = RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        response = View(form);
                    }
                }
                else
                {
                    response = RedirectToAction("Index", "Home");
                }
            }
            else
            {
                response = RedirectToAction("Register", "Account");
            }
            return response;
        }

        //Get add info for a new movie theory.
        [HttpGet]
        public ActionResult AddMovieTheory(long movieId)
        {
            ActionResult response;

            //Anyone with an account can add a movie theory.
            if (Session["Role"] != null)
            {
                try
                {
                    //So the movie id is properly taken for the movie theory to be directly connected to it.
                    MovieTheoryPO po = new MovieTheoryPO();
                    po.MovieId = movieId;
                    
                    //Same dropdown list for status.
                    ViewBag.StatusDropDown = new List<SelectListItem>();
                    ViewBag.StatusDropDown.Add(new SelectListItem { Text = "Tinfoilly", Value = po.Status });
                    ViewBag.StatusDropDown.Add(new SelectListItem { Text = "Perchance", Value = po.Status });
                    ViewBag.StatusDropDown.Add(new SelectListItem { Text = "Believable", Value = po.Status });

                    response = View(po);
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

        //Post added movie theory info.
        [HttpPost]
        public ActionResult AddMovieTheory(MovieTheoryPO form)
        {
            ActionResult response;

            //Anyone with an account can add a movie theory.
            if (Session["Role"] != null)
            {
                if (ModelState.IsValid)
                {

                    try
                    {
                        //Keeping the dropdown list, just in case.
                        ViewBag.StatusDropDown = new List<SelectListItem>();
                        ViewBag.StatusDropDown.Add(new SelectListItem { Text = "Tinfoilly", Value = "Tinfoilly" });
                        ViewBag.StatusDropDown.Add(new SelectListItem { Text = "Perchance", Value = "Perchance" });
                        ViewBag.StatusDropDown.Add(new SelectListItem { Text = "Believable", Value = "Believable" });

                        MovieTheoryDO newMovieTheory = mapper.MapPoToDo(form);

                        movieTheoryDataAccess.AddMovieTheory(newMovieTheory);

                        response = RedirectToAction("MovieTheoryIndex", "MovieTheory", new { id = form.MovieId });
                    }
                    catch (Exception ex)
                    {
                        logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                        response = RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    //Keeping the dropdown list, just in case.
                    ViewBag.StatusDropDown = new List<SelectListItem>();
                    ViewBag.StatusDropDown.Add(new SelectListItem { Text = "Tinfoilly", Value = "Tinfoilly" });
                    ViewBag.StatusDropDown.Add(new SelectListItem { Text = "Perchance", Value = "Perchance" });
                    ViewBag.StatusDropDown.Add(new SelectListItem { Text = "Believable", Value = "Believable" });

                    response = View(form);
                }
            }
            else
            {
                response = RedirectToAction("Register", "Account");
            }
            return response;
        }

        //Get delete info for theories.
        [HttpGet]
        public ActionResult DeleteMovieTheory(long id)
        {
            ActionResult response;

            //Only for admins.
            if (Session["Role"] != null)
            {
                if ((int)Session["Role"] == 3 && id > 0)
                {
                    try
                    {
                        //Map the theory in order to delete from the database and the presentation layer.
                        MovieTheoryDO movieTheory = movieTheoryDataAccess.ViewMovieTheory(id);
                        MovieTheoryPO deletedMovieTheory = mapper.MapDoToPo(movieTheory);
                        movieTheoryDataAccess.DeleteMovieTheory(deletedMovieTheory.MovieTheoryID);

                        response = RedirectToAction("Index", "Home");
                    }
                    catch (Exception ex)
                    {
                        logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                        response = RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    response = RedirectToAction("Index", "Home");
                }
            }
            else
            {
                response = RedirectToAction("Register", "Account");
            }
            return response;
        }

        //Logic stuff.
        public List<KeyValuePair<string, float>> CountStatus()
        {
            //To collect only what the status says and the percent of how much it is used.
            List<KeyValuePair<string, float>> statusPercentages = new List<KeyValuePair<string, float>>();

            try
            {
                //Map for all the movie theories to collect all statuses.
                List<MovieTheoryDO> allMovies = movieTheoryDataAccess.AllMovieTheories();
                List<MovieTheoryBO> mappedMovieTheories = new List<MovieTheoryBO>();

                //Collect DO to BO for the logic layer.
                foreach (MovieTheoryDO dataObject in allMovies)
                {
                    mappedMovieTheories.Add(mapper.MapDoToBo(dataObject));
                }

                //The KeyValuePair going to the logic layer in order to get percents.
                statusPercentages = statusLogic.StatusCount(mappedMovieTheories);

                ViewBag.StatusPercent = statusPercentages;
            }
            catch (Exception ex)
            {
                logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
            }
            return statusPercentages;
        }
    }
}