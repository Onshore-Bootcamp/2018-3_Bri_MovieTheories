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
    public class AccountController : Controller
    {
        //Logger
        private static ErrorLogger logger = new ErrorLogger();

        //Map for PO.
        private static UserMapper mapper = new UserMapper();

        //DAO access
        private readonly UserDAO userDataAccess;
        public AccountController()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DataSource"].ConnectionString;

            userDataAccess = new UserDAO(connectionString);
        }

        //Getting register info.
        [HttpGet]
        public ActionResult Register()
        {
            ActionResult response;

            //Only null sessions can register.
            if (Session["Role"] == null)
            {
                try
                {
                    response = View();
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

        //Post registering info.
        [HttpPost]
        public ActionResult Register(UserPO form)
        {
            ActionResult response;

            //Anyone without an account or signed in can register.
            if (Session["Role"] == null)
            {
                //Making sure everything that is needed has been inserted
                if (ModelState.IsValid)
                {
                    try
                    {
                        UserDO dataObject = new UserDO()
                        {
                            //Setting the info correctly for registering.
                            Username = form.Username,
                            Password = form.Password,
                            FirstName = form.FirstName,
                            Email = form.Email,

                            //Everyone starts as just user.
                            Role = 1,
                            RoleName = "User"
                        };
                        userDataAccess.RegisterUser(dataObject);

                        //Hold onto the username for easier login.
                        TempData["Username"] = dataObject.Username;

                        response = RedirectToAction("Login", "Account");
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
            return response;
        }

        //Getting login info.
        [HttpGet]
        public ActionResult Login()
        {
            Login login = new Login();

            //Anyone not logged in can login.
            if (Session["Role"] == null)
            {
                try
                {
                    //Bringing in that username for easy login.
                    if (TempData.ContainsKey("Username"))
                    {
                        login.Username = (string)TempData["Username"];
                    }
                }
                catch (Exception ex)
                {
                    logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                    View(login);
                }
            }
            else
            {
                RedirectToAction("Index", "Home");
            }
            return View(login);
        }

        //Post login.
        [HttpPost]
        public ActionResult Login(Login form)
        {
            ActionResult response;

            //Anyone that hadn't logged in can log in.
            if (Session["Role"] == null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        //Log in by username.
                        UserDO userDataObject = userDataAccess.ViewUser(form.Username);

                        //Making sure ID isn't 0.
                        if (userDataObject.UserId != default(int) && userDataObject.Password.Equals(form.Password) && userDataObject.Username.Equals(form.Username))
                        {
                            //Make it possible for the users to see and do certain things.
                            Session["Username"] = userDataObject.Username;
                            Session["Role"] = userDataObject.Role;

                            response = RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "The Username and/or Password field(s) are incorrect.");
                            response = View(form);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                        response = View(form);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "The Username and/or Password field(s) are empty.");
                    response = View(form);
                }
            }
            else
            {
                response = RedirectToAction("Index", "Home");
            }
            return response;
        }

        //Logs accounts out.
        public ActionResult Logout()
        {
            ActionResult response;

            //Only works if the session of role isn't null.
            if (Session["Role"] != null)
            {
                try
                {
                    Session.Clear();

                    response = RedirectToAction("Login", "Account");
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

        //Everybody with an acocunt allowed to see all users.
        public ActionResult AllUsers()
        {
            ActionResult response;

            //Anyone logged in can see all users.
            if (Session["Role"] != null)
            {
                try
                {
                    List<UserDO> allUsers = userDataAccess.ViewAllUsers();
                    List<UserPO> mappedUsers = new List<UserPO>();

                    //Map all users.
                    foreach (UserDO dataObject in allUsers)
                    {
                        mappedUsers.Add(mapper.MapDoToPo(dataObject));
                    }
                    response = View(mappedUsers);
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

        //Role 3 allowed details and users of their own info.
        public ActionResult UserDetails(long userID)
        {
            ActionResult response;

            //Making sure the session isn't null.
            if (Session["Role"] != null)
            {
                if (userID > 0)
                {
                    try
                    {
                        UserDO user = userDataAccess.ViewUserByID(userID);
                        UserPO detailsUser = mapper.MapDoToPo(user);

                        //If they are admin or if their username is equal to session.
                        if ((int)Session["Role"] == 3 || (string)Session["Username"] == detailsUser.Username)
                        {
                            response = View(detailsUser);
                        }
                        else
                        {
                            response = RedirectToAction("AllUsers", "Account");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                        response = RedirectToAction("AllUsers", "Account");
                    }
                }
                else
                {
                    response = RedirectToAction("AllUsers", "Account");
                }
            }
            else
            {
                response = RedirectToAction("Register", "Account");
            }
            return response;
        }

        //Role 3 allowed to update and users of their own account are allow to update everything besides role.
        [HttpGet]
        public ActionResult UpdateUser(long userID)
        {
            ActionResult response;

            //Makes sure the session isn't null.
            if (Session["Role"] != null)
            {
                if (userID > 0)
                {
                    try
                    {
                        UserDO user = userDataAccess.ViewUserByID(userID);
                        UserPO detailsUser = mapper.MapDoToPo(user);

                        //Admins and if the username equals the session can see their own details.
                        if ((int)Session["Role"] == 3 || (string)Session["Username"] == detailsUser.Username)
                        {
                            //ViewBag dropdown list for admins to change roles for anybody.
                            ViewBag.RoleDropDown = new List<SelectListItem>();
                            ViewBag.RoleDropDown.Add(new SelectListItem { Text = "User", Value = 1.ToString() });
                            ViewBag.RoleDropDown.Add(new SelectListItem { Text = "Moderator", Value = 2.ToString() });
                            ViewBag.RoleDropDown.Add(new SelectListItem { Text = "Admin", Value = 3.ToString() });

                            response = View(detailsUser);
                        }
                        else
                        {
                            response = RedirectToAction("AllUsers", "Account");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                        response = RedirectToAction("AllUsers", "Account");
                    }
                }
                else
                {
                    response = RedirectToAction("AllUsers", "Account");
                }
            }
            else
            {
                response = RedirectToAction("Register", "Account");
            }
            return response;
        }

        //Post updated info.
        [HttpPost]
        public ActionResult UpdateUser(UserPO form)
        {
            ActionResult response;

            //Makes sure session isn't null.
            if (Session["Role"] != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        UserDO userDO = mapper.MapPoToDo(form);

                        //Sets the rolename.
                        if (form.Role == 1)
                        {
                            userDO.RoleName = "User";
                        }
                        else if (form.Role == 2)
                        {
                            userDO.RoleName = "Moderator";
                        }
                        else if (form.Role == 3)
                        {
                            userDO.RoleName = "Admin";
                        }
                        else
                        {
                            userDO.RoleName = default(string);
                        }

                        userDataAccess.UpdateUser(userDO);

                        //Only for admins and for if the session equals the account.
                        if ((int)Session["Role"] == 3 || (string)Session["Username"] == userDO.Username)
                        {
                            //Dropdown list for admins to use on roles.
                            ViewBag.RoleDropDown = new List<SelectListItem>();
                            ViewBag.RoleDropDown.Add(new SelectListItem { Text = "User", Value = 1.ToString() });
                            ViewBag.RoleDropDown.Add(new SelectListItem { Text = "Moderator", Value = 2.ToString() });
                            ViewBag.RoleDropDown.Add(new SelectListItem { Text = "Admin", Value = 3.ToString() });

                            response = RedirectToAction("AllUsers", "Account");
                        }
                        else
                        {
                            response = RedirectToAction("AllUsers", "Account");
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                        response = RedirectToAction("AllUsers", "Account");
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

        //Delete for only role 3.
        [HttpGet]
        public ActionResult DeleteUser(long userID)
        {
            ActionResult response;

            //Only admins can delete user.
            if (Session["Role"] != null)
            {
                if ((int)Session["Role"] == 3 && userID > 0)
                {
                    try
                    {
                        UserDO user = userDataAccess.ViewUserByID(userID);
                        UserPO deleteUser = mapper.MapDoToPo(user);
                        userDataAccess.DeleteUser(userID);

                        response = RedirectToAction("AllUsers", "Account");
                    }
                    catch (Exception ex)
                    {
                        logger.ErrorLog(MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex);
                        response = RedirectToAction("UserDetails", "Account");
                    }
                }
                else
                {
                    response = RedirectToAction("AllUsers", "Account");
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