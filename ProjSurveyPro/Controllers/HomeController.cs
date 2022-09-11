using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjSurveyPro.Models;
using System.IO;
using CaptchaMvc.HtmlHelpers;
using System.Net.Mail;

namespace ProjSurveyPro.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        dbEMRSEntities obj = new dbEMRSEntities();

        public ActionResult Index()
        {
            Session.Abandon();
            return View();
        }

        public ActionResult MeetUs()
        {
            return View();
        }

        [HttpGet]
        [ActionName("ReachUs")]
        public ActionResult ReachUsGet()
        {
            return View();
        }

        [HttpPost]
        [ActionName("ReachUs")]
        public ActionResult ReachUsPost(ClsQueryMst Query)
        {
            if (ModelState.IsValid)
            {
                if (this.IsCaptchaValid("Captcha is not valid"))
                {
                    obj.uspSaveQueryMst(Query.QueryID, Query.QueryDisc, Query.EmailID);
                    return View("Index");
                }
                else
                {
                    ViewBag.Error = "Captcha is not valid";
                    return View();
                }
            }
            else
            {
                ViewBag.Error = "Captcha is not valid";
                return View();
            }
        }

        public ActionResult Service()
        {
            return View();
        }

        public ActionResult AboutSystem()
        {
            return View();
        }

        public ActionResult FAQ()
        {
            return View();
        }

        [HttpGet]
        [ActionName("SignIn")]
        public ActionResult SignInGet()
        {
            return View();
        }

        [HttpPost]
        [ActionName("SignIn")]
        public ActionResult SignInPost()
        {
            string Email = Request["Email"];
            string Password = Request["password"];
            var Area = obj.CheckLogin(Email, Password).ToList();
            string Designation = Area.SingleOrDefault().Designation;
            if (Designation != null)
            {
                if (this.IsCaptchaValid("Captcha is not valid"))
                {
                    if (Designation == "User")
                    {
                        Session["UserID"] = Area.SingleOrDefault().ID;
                        Session["UserImage"] = Area.SingleOrDefault().Imag;
                        Session["Name"] = Area.SingleOrDefault().Name;
                        Session["Designation"] = Designation;
                        Session["UserLastLoginDt"] = Area.SingleOrDefault().LastLoginDt;
                        return RedirectToAction("Index", new { area = "User", Status = 1 });
                    }
                    else if (Designation == "Client")
                    {
                        Session["ClientID"] = Area.SingleOrDefault().ID;
                        Session["ClientImage"] = Area.SingleOrDefault().Imag;
                        Session["Name"] = Area.SingleOrDefault().Name;
                        Session["Designation"] = Designation;
                        Session["ClientLastLoginDt"] = Area.SingleOrDefault().LastLoginDt;
                        return RedirectToAction("Index", new { area = "Client", Status = 1 });
                    }
                    else if (Designation == "Employee")
                    {
                        Session["EmpID"] = Area.SingleOrDefault().ID;
                        Session["EmpImage"] = Area.SingleOrDefault().Imag;
                        Session["Name"] = Area.SingleOrDefault().Name;
                        Session["Designation"] = Designation;
                        Session["EmpLastLoginDt"] = Area.SingleOrDefault().LastLoginDt;
                        return RedirectToAction("Index", new { area = "Employee", Status = 1 });
                    }
                    else
                    {
                        Session["AdminID"] = Area.SingleOrDefault().ID;
                        Session["AdminImage"] = Area.SingleOrDefault().Imag;
                        Session["Name"] = Area.SingleOrDefault().Name;
                        Session["Designation"] = Designation;
                        Session["LastLoginDt"] = Area.SingleOrDefault().LastLoginDt;
                        return RedirectToAction("Index", new { area = "Admin", Status = 1 });
                    }
                }
                else
                {
                    Session["Invalid"] = "1";
                    ViewBag.Invalid = "Re-Enter E-mail ID or Password";
                    ViewBag.Error = "Captcha is not valid";
                    return View();
                }
            }
            else
            {
                Session["Invalid"] = "1";
                ViewBag.Invalid = "Invalid E-mail ID or Password";
                return View();
            }
        }

        [HttpGet]
        [ActionName("UserSignUp")]
        public ActionResult UserSignUpGet()
        {
            Session["Image"] = "0";
            var State = obj.GetState();
            ViewBag.SList = new SelectList(State, "StateID", "StateName");
            return View();
        }

        [HttpPost]
        [ActionName("UserSignUp")]
        public ActionResult UserSignUpPost(ClsUserMst User, HttpPostedFileBase UserImage)
        {
            DateTime date = DateTime.Now;
            DateTime dt = date.AddYears(-18);
            User.JoiningDate = date.Date;
            User.IsActive = false;
            var State = obj.GetState();
            ViewBag.SList = new SelectList(State, "StateID", "StateName");
            if (User.StateID != null)
            {
                List<CityMst> City = new List<CityMst>();
                City = obj.CityMsts.Where(C => C.StateID == User.StateID).ToList();
                if (User.CityID != null)
                {
                    ViewBag.CList = new SelectList(City, "CityID", "CityName", User.CityID);
                }
                else
                {
                    ViewBag.CList = new SelectList(City, "CityID", "CityName", "StateID");
                }
            }
            if (ModelState.IsValid)
            {
                if (User.DtBirth != date.Date & User.DtBirth <= dt)
                {
                    if (UserImage != null)
                    {
                        if ((UserImage.ContentType == "image/png" || UserImage.ContentType == "image/jpeg") && (UserImage.ContentLength <= 4194304))
                        {
                            var extension = Path.GetExtension(UserImage.FileName);
                            string fileName = "User" + date.ToString("yyyyMMddhhmmssfff") + extension;
                            User.UserImage = "~/Contents/appimg/User/" + fileName;
                            UserImage.SaveAs(Path.Combine(Server.MapPath("~/Contents/appimg/User/"), fileName));
                            try
                            {
                                obj.uspSaveUserMst(User.UserID, User.UserName, User.Gender, User.UserAddress, User.CityID, User.StateID, User.MobNo, User.EmailID, User.DtBirth, User.UserImage, User.JoiningDate, User.IsActive);
                                return View("Index");
                            }
                            catch (Exception e)
                            {
                                var sql = e.InnerException as System.Data.SqlClient.SqlException;
                                if (sql.Number == 2627)
                                {
                                    Session["Email"] = "1";
                                    ViewBag.Email = "EmailID Already Exist...";
                                }
                                return View();
                            }
                        }
                        else
                        {
                            Session["Image"] = "1";
                            if (UserImage.ContentLength <= 4194304)
                            {
                                ViewBag.ImageType = "User Image Size Must Be less then 4MB";
                            }
                            else
                            {
                                ViewBag.ImageType = "User Image Must Be in jpg or png Format";
                            }
                            return View();
                        }
                    }
                    else
                    {
                        User.UserImage = "~/Contents/appimg/user.png";
                        try
                        {
                            obj.uspSaveUserMst(User.UserID, User.UserName, User.Gender, User.UserAddress, User.CityID, User.StateID, User.MobNo, User.EmailID, User.DtBirth, User.UserImage, User.JoiningDate, User.IsActive);
                            return View("Index");
                        }
                        catch (Exception e)
                        {
                            var sql = e.InnerException as System.Data.SqlClient.SqlException;
                            if (sql.Number == 2627)
                            {
                                Session["Email"] = "1";
                                ViewBag.Email = "EmailID Already Exist...";
                            }
                            return View();
                        }
                    }
                }
                else
                {
                    Session["Date"] = "1";
                    ViewBag.Date = "Enter Valid Date (Your Age Must be 18+)...";
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult FillCity(int stateid)
        {
            List<CityMst> CityList = new List<CityMst>();
            CityList = obj.CityMsts.Where(m => m.StateID == stateid).ToList();
            SelectList CList = new SelectList(CityList, "CityID", "CityName", "StateID");
            return Json(CList);
        }

        [HttpGet]
        [ActionName("CompanySignUp")]
        public ActionResult CompanySignUpGet()
        {
            Session["Image"] = "0";
            return View();
        }

        [HttpPost]
        [ActionName("CompanySignUp")]
        public ActionResult CompanySignUpPost(ClsClientMst Client, HttpPostedFileBase CompanyLogo)
        {
            DateTime date = DateTime.Now;
            Client.JoiningDate = date.Date;
            Client.IsActive = false;
            if (ModelState.IsValid)
            {
                if (CompanyLogo != null)
                {
                    if ((CompanyLogo.ContentType == "image/png" || CompanyLogo.ContentType == "image/jpeg") && (CompanyLogo.ContentLength <= 4194304))
                    {
                        var extension = Path.GetExtension(CompanyLogo.FileName);
                        string fileName = "Client" + date.ToString("yyyyMMddhhmmssfff") + extension;
                        Client.CompanyLogo = "~/Contents/appimg/Client/" + fileName;
                        CompanyLogo.SaveAs(Path.Combine(Server.MapPath("~/Contents/appimg/Client/"), fileName));
                        try
                        {
                            obj.uspSaveClientMst(Client.ClientID, Client.CompName, Client.CompAddress, Client.OwnerName, Client.CompanyLogo, Client.ContactNo, Client.MobileNo, Client.GSTNo, Client.WebsiteName, Client.EmailID, Client.JoiningDate, Client.IsActive);
                            return View("Index");
                        }
                        catch (Exception e)
                        {
                            var sql = e.InnerException as System.Data.SqlClient.SqlException;
                            if (sql.Number == 2627)
                            {
                                Session["Email"] = "1";
                                ViewBag.Email = "EmailID Already Exist...";
                            }
                            return View();
                        }
                    }
                    else
                    {
                        Session["Image"] = "1";
                        if (CompanyLogo.ContentLength <= 4194304)
                        {
                            ViewBag.ImageType = "Company Logo Size Must Be lessthen 4MB";
                        }
                        else
                        {
                            ViewBag.ImageType = "Company Logo Must Be in jpg or png Format";
                        }
                        return View();
                    }
                }
                else
                {
                    Client.CompanyLogo = "~/Contents/appimg/CompanyLogo.png";
                    try
                    {
                        obj.uspSaveClientMst(Client.ClientID, Client.CompName, Client.CompAddress, Client.OwnerName, Client.CompanyLogo, Client.ContactNo, Client.MobileNo, Client.GSTNo, Client.WebsiteName, Client.EmailID, Client.JoiningDate, Client.IsActive);
                        return View("Index");
                    }
                    catch (Exception e)
                    {
                        var sql = e.InnerException as System.Data.SqlClient.SqlException;
                        if (sql.Number == 2627)
                        {
                            Session["Email"] = "1";
                            ViewBag.Email = "EmailID Already Exist...";
                        }
                        return View();
                    }
                }
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        [ActionName("RecoverPassword")]
        public ActionResult RecoverPasswordGet()
        {
            return View();
        }

        [HttpPost]
        [ActionName("RecoverPassword")]
        public ActionResult RecoverPasswordPost()
        {
            string EmailID = Request["Email"];
            if (this.IsCaptchaValid("Captcha is not valid"))
            {

                try
                {
                    string Passwd = obj.uspGetRecoverPasswd(EmailID).SingleOrDefault().ToString();
                    try
                    {
                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                        mail.From = new MailAddress("prosurvey63@gmail.com");
                        mail.To.Add(EmailID);
                        mail.Subject = "Confirmation Mail";
                        mail.Body = "Welcome to Survey Pro...";
                        mail.Body = "Your Login Password is : " + Passwd;
                        SmtpServer.Port = 587;
                        SmtpServer.Credentials = new System.Net.NetworkCredential("prosurvey63@gmail.com", "surveypro07102147");
                        SmtpServer.EnableSsl = true;
                        SmtpServer.Send(mail);
                    }
                    catch
                    {
                    }
                }
                catch
                {
                    Session["Invalid"] = "1";
                    ViewBag.Invalid = "Re-Enter E-mail ID";
                    ViewBag.Error = "Captcha is not valid";
                    return View();
                }
                return View("SignIn");
            }
            else
            {
                Session["Invalid"] = "1";
                ViewBag.Invalid = "Invalid E-mail ID";
                return View();
            }
        }

        public ActionResult ViewPackages()
        {
            return View(obj.GetPackage(1).ToList());
        }

        public ActionResult ViewProducts()
        {
            ViewBag.Products = obj.GetActiveProduct().ToList();
            return View();
        }

        public ActionResult ProductDetails(short PID, short CID)
        {
            ProductMst Product = obj.ProductMsts.Single(P => P.ClientID == CID & P.ProductID == PID);
            Session["SRP"] = "Static";
            Session["Btn"] = "Static";
            return View(Product);
        }

        [HttpGet]
        [ActionName("Feedback")]
        public ActionResult FeedbackGet(short PID, short CID)
        {
            Session["Rating"] = "0";
            Session["PID"] = PID;
            Session["CID"] = CID;
            return View();
        }

        [HttpPost]
        [ActionName("Feedback")]
        public ActionResult FeedbackPost(ClsFeedbackMst Feedback)
        {
            byte R = Convert.ToByte(Request["Rating"]);
            if (ModelState.IsValid & R != 0)
            {
                short PID = Convert.ToInt16(Session["PID"]);
                short CID = Convert.ToInt16(Session["CID"]);
                if (this.IsCaptchaValid("Captcha is not Valid"))
                {
                    obj.uspSaveFeedbackMst(Feedback.FeedbackID, CID, PID, Feedback.FeedbackDisc, R, Feedback.UserEmail, Feedback.UserContactNo);
                    ViewBag.Products = obj.GetActiveProduct().ToList();
                    return View("ViewProducts");
                }
                else
                {
                    ViewBag.Error = "Captcha is not valid";
                    return View();
                }
            }
            else
            {
                Session["Rating"] = "1";
                ViewBag.Rating = "Rate Product...";
                return View();
            }
        }
    }
}
