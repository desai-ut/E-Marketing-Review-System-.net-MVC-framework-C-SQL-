using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data;
using ProjSurveyPro.Models;
using System.IO;
using System.Dynamic;
using CaptchaMvc.HtmlHelpers;
using System.Net.Mail;

namespace ProjSurveyPro.Areas.User.Controllers
{
    public class HomeController : Controller
    {
        dbEMRSEntities obj = new dbEMRSEntities();
        //
        // GET: /User/Home/

        public ActionResult Index(Int16? Status)
        {
            if (Session["UserID"] != null)
            {
                short UserID = Convert.ToInt16(Session["UserID"]);
                var Report = obj.GetUserReport(UserID).ToList();
                ViewBag.AClient = Report.SingleOrDefault().AClient;
                ViewBag.AttemptedReview = Report.SingleOrDefault().AttemptedReview;
                ViewBag.AttemptedSurvey = Report.SingleOrDefault().AttemptedSurvey;
                ViewBag.Product = Report.SingleOrDefault().Product;
                ViewBag.RedeemRewardPoint = Report.SingleOrDefault().RedeemRewardPoint;
                ViewBag.RewardPoint = Report.SingleOrDefault().RewardPoint;
                ViewBag.LastLoginDate = Session["UserLastLoginDt"];

                ViewBag.CClient = Report.SingleOrDefault().AClient;
                ViewBag.CProduct = Report.SingleOrDefault().Product;
                ViewBag.CAttemptedReview = Report.SingleOrDefault().AttemptedReview;
                ViewBag.CAttemptedSurvey = Report.SingleOrDefault().AttemptedSurvey;
                if (Status != null)
                {
                    string Designation = Session["Designation"].ToString();
                    obj.UpdateLastLoginDate(UserID, Designation);
                }
                return View();
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        [HttpGet]
        [ActionName("UpdateProfile")]
        public ActionResult UpdateProfileGet()
        {
            if (Session["UserID"] != null)
            {
                short UserID = Convert.ToInt16(Session["UserID"]);
                UserMst User = obj.UserMsts.Single(U => U.UserID == UserID);
                var State = obj.GetState();
                ViewBag.SList = new SelectList(State, "StateID", "StateName");
                List<CityMst> City = obj.CityMsts.Where(m => m.StateID == User.StateID).ToList();
                ViewBag.CList = new SelectList(City, "CityID", "CityName", "StateID");
                return View(User);
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        [HttpPost]
        [ActionName("UpdateProfile")]
        public ActionResult UpdateProfilePost(ClsUserMst User)
        {
            if (Session["UserID"] != null)
            {
                DateTime date = DateTime.Now;
                DateTime dt = date.AddYears(-18);
                short UserID = Convert.ToInt16(Session["UserID"]);
                User.UserID = UserID;
                var State = obj.GetState();
                ViewBag.SList = new SelectList(State, "StateID", "StateName");
                List<CityMst> City = obj.CityMsts.Where(m => m.StateID == User.StateID).ToList();
                ViewBag.CList = new SelectList(City, "CityID", "CityName", "StateID");
                if (ModelState.IsValid)
                {
                    if (User.DtBirth != date & User.DtBirth <= dt)
                    {
                        try
                        {
                            obj.uspSaveUserMst(User.UserID, User.UserName, User.Gender, User.UserAddress, User.CityID, User.StateID, User.MobNo, User.EmailID, User.DtBirth, User.UserImage, User.JoiningDate, User.IsActive);
                            UserMst objUser = obj.UserMsts.Single(U => U.UserID == UserID);
                            return View("UpdateProfile", objUser);
                        }
                        catch (Exception e)
                        {
                            var sql = e.InnerException as System.Data.SqlClient.SqlException;
                            if (sql.Number == 2627)
                            {
                                Session["Email"] = "1";
                                ViewBag.Email = "EmailID Already Exist...";
                            }
                            UserMst UserObj = obj.UserMsts.Single(U => U.UserID == User.UserID);
                            return View(UserObj);
                        }
                    }
                    else
                    {
                        Session["Date"] = "1";
                        ViewBag.Date = "Enter Valid Date (Your Age Must be 18+)...";
                        UserMst UserObj = obj.UserMsts.Single(U => U.UserID == User.UserID);
                        return View(UserObj);
                    }
                }
                else
                {
                    UserMst UserObj = obj.UserMsts.Single(U => U.UserID == User.UserID);
                    return View(UserObj);
                }
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
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

        [HttpPost]
        public ActionResult UpdateImage(HttpPostedFileBase UserImage)
        {
            if (Session["UserID"] != null)
            {
                var UserID = Convert.ToInt16(Session["UserID"]);
                UserMst objUser = obj.UserMsts.Single(U => U.UserID == UserID);
                var State = obj.GetState();
                ViewBag.SList = new SelectList(State, "StateID", "StateName");
                List<CityMst> City = obj.CityMsts.Where(m => m.StateID == objUser.StateID).ToList();
                ViewBag.CList = new SelectList(City, "CityID", "CityName", "StateID");
                if (UserImage != null)
                {
                    if ((UserImage.ContentType == "image/png" || UserImage.ContentType == "image/jpeg") && (UserImage.ContentLength <= 4194304))
                    {
                        DateTime date = DateTime.Now;
                        var extension = Path.GetExtension(UserImage.FileName);
                        string fileName = "User" + date.ToString("yyyyMMddhhmmssfff") + extension;
                        UserMst User = new UserMst();
                        User.UserID = UserID;
                        User.UserImage = "~/Contents/appimg/User/" + fileName;
                        UserImage.SaveAs(Path.Combine(Server.MapPath("~/Contents/appimg/User/"), fileName));
                        obj.uspSaveUpdateImageUser(User.UserID, User.UserImage);
                        UserMst objUserUpdate = obj.UserMsts.Single(U => U.UserID == UserID);
                        Session["UserImage"] = objUserUpdate.UserImage;
                        return View("UpdateProfile", objUserUpdate);
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
                        return View("UpdateProfile", objUser);
                    }
                }
                else
                {
                    return View("UpdateProfile", objUser);
                }
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult ViewProducts()
        {
            return View(obj.GetActiveProduct().ToList());
        }

        public ActionResult ProductDetails(short PID, short CID)
        {
            ProductMst Product = obj.ProductMsts.Single(P => P.ClientID == CID & P.ProductID == PID);
            SurveyMst Survey = obj.SurveyMsts.Single(S => S.ClientID == CID & S.ProductID == PID);
            Product.SurveyMsts.SingleOrDefault().SurveyID = Survey.SurveyID;
            Session["SRP"] = "User";
            Session["Btn"] = "User";
            return View(Product);
        }

        [HttpGet]
        [ActionName("ChangePassword")]
        public ActionResult ChangePasswordGet()
        {
            return View();
        }

        [HttpPost]
        [ActionName("ChangePassword")]
        public ActionResult ChangePasswordPost(string Password, string NewPassword, string ConfirmPassword)
        {
            if (Session["UserID"] != null)
            {
                short UserID = Convert.ToInt16(Session["UserID"]);
                if (Password != "" && NewPassword != "" && ConfirmPassword != "")
                {
                    if (NewPassword.CompareTo(ConfirmPassword) == 0)
                    {
                        var Status = obj.uspSaveChangePasswdUser(UserID, Password, NewPassword);
                        if (Convert.ToInt16(Status.SingleOrDefault().Value) == UserID)
                        {
                            UserMst User = obj.UserMsts.Single(A => A.UserID == UserID);
                            try
                            {
                                string EmailID = User.EmailID;
                                MailMessage mail = new MailMessage();
                                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                                mail.From = new MailAddress("prosurvey63@gmail.com");
                                mail.To.Add(EmailID);
                                mail.Subject = "Confirmation Mail";
                                mail.Body = "Welcome to Survey Pro... " + Environment.NewLine + "Your New Paswword is : " + NewPassword;
                                SmtpServer.Port = 587;
                                SmtpServer.Credentials = new System.Net.NetworkCredential("prosurvey63@gmail.com", "surveypro07102147");
                                SmtpServer.EnableSsl = true;
                                SmtpServer.Send(mail);
                            }
                            catch
                            {
                            }
                            return View("Index");
                        }
                        else
                        {
                            Session["password"] = "1";
                            ViewBag.Pass = "Incorrect Password Required...";
                            return View();
                        }
                    }
                    else
                    {
                        Session["Confirm password"] = "1";
                        ViewBag.CPass = "Password Not Match...";
                        return View();
                    }
                }
                else
                {
                    if (Password == "")
                    {
                        Session["password"] = "1";
                        ViewBag.Pass = "Password Required...";
                    }
                    if (NewPassword == "")
                    {
                        Session["New password"] = "1";
                        ViewBag.NPass = "New Password Required..";
                    }
                    if (ConfirmPassword == "")
                    {
                        Session["Confirm password"] = "1";
                        ViewBag.CPass = "Confirm Password Required...";
                    }
                    return View();
                }
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        [HttpGet]
        [ActionName("GiveSurvey")]
        public ActionResult GiveSurveyGet(short SID, short PID, short CID)
        {
            if (Session["UserID"] != null)
            {
                short UserID = Convert.ToInt16(Session["UserID"]);
                var Cnt = obj.SurveyCntIncrement(UserID, SID);
                short Status = Convert.ToInt16(Cnt.SingleOrDefault().Status);
                if (Status == 1)
                {
                    ProductMst Product = obj.ProductMsts.Single(P => P.ClientID == CID & P.ProductID == PID);
                    ClientMst Client = obj.ClientMsts.Single(C => C.ClientID == CID);
                    ViewBag.ProductName = Product.ProductName;
                    ViewBag.ModelName = Product.ModelName;
                    ViewBag.CompName = Client.CompName;
                    ViewBag.OwnerName = Client.OwnerName;
                    ViewBag.ContactNo = Client.ContactNo;
                    ViewBag.EmailID = Client.EmailID;
                    ViewBag.CompanyLogo = Client.CompanyLogo;
                    Session["ClientID"] = CID;
                    Session["ProductID"] = PID;
                    Session["SurveyID"] = SID;
                    ViewBag.SurveyID = SID;
                    ViewBag.ProductID = PID;
                    ViewBag.ClientID = CID;
                    return View(obj.GetSurveyQuesForAns(SID).ToList());
                }
                else
                {
                    ProductMst Product = obj.ProductMsts.Single(P => P.ClientID == CID & P.ProductID == PID);
                    Product.SurveyMsts.SingleOrDefault().SurveyID = SID;
                    Session["Error"] = "1";
                    ViewBag.Error = "Sry, Survey Expired..";
                    Session["SRP"] = "User";
                    Session["Btn"] = "User";
                    return View("ProductDetails", Product);
                }
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        [HttpPost]
        [ActionName("GiveSurvey")]
        public ActionResult GiveSurveyPost()
        {
            if (Session["UserID"] != null)
            {
                short SID = Convert.ToInt16(Session["SurveyID"]);
                short CID = Convert.ToInt16(Session["ClientID"]);
                short PID = Convert.ToInt16(Session["ProductID"]);
                short UID = Convert.ToInt16(Session["UserID"]);
                byte CGID = Convert.ToByte(Request["CategoryID"]);

                string Category;
                byte A1, A2, A3;
                Category = Request["Category"].ToString();
                short QID = Convert.ToInt16(Request["QuestionID"]);

                if (Category.Equals("Like-Dislike"))
                {
                    A1 = Convert.ToByte(Request["QuesRes1"]);
                    if (A1 != 0)
                    {
                        obj.uspSaveSurveyAns(SID, CGID, QID, UID, A1);
                    }
                }
                else if (Category.Equals("MCQ"))
                {
                    A2 = Convert.ToByte(Request["QuesRes"]);
                    if (A2 != 0)
                    {
                        obj.uspSaveSurveyAns(SID, CGID, QID, UID, A2);
                    }
                }
                else if (Category.Equals("Rating"))
                {
                    A3 = Convert.ToByte(Request["Rating"]);
                    if (A3 != 0)
                    {
                        obj.uspSaveSurveyAns(SID, CGID, QID, UID, A3);
                    }
                }

                ProductMst Product = obj.ProductMsts.Single(P => P.ClientID == CID & P.ProductID == PID);
                ClientMst Client = obj.ClientMsts.Single(C => C.ClientID == CID);
                ViewBag.ProductName = Product.ProductName;
                ViewBag.ModelName = Product.ModelName;
                ViewBag.CompName = Client.CompName;
                ViewBag.OwnerName = Client.OwnerName;
                ViewBag.ContactNo = Client.ContactNo;
                ViewBag.EmailID = Client.EmailID;
                ViewBag.CompanyLogo = Client.CompanyLogo;
                ViewBag.SurveyID = SID;
                ViewBag.ProductID = PID;
                ViewBag.ClientID = CID;
                return View(obj.GetSurveyQuesForAns(SID).ToList());
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        [HttpGet]
        [ActionName("GiveReview")]
        public ActionResult GiveReviewGet(short SID, short PID, short CID)
        {
            ProductMst Product = obj.ProductMsts.Single(P => P.ClientID == CID & P.ProductID == PID);
            ClientMst Client = obj.ClientMsts.Single(C => C.ClientID == CID);
            ViewBag.ProductName = Product.ProductName;
            ViewBag.ModelName = Product.ModelName;
            ViewBag.CompName = Client.CompName;
            ViewBag.OwnerName = Client.OwnerName;
            ViewBag.ContactNo = Client.ContactNo;
            ViewBag.EmailID = Client.EmailID;
            ViewBag.CompanyLogo = Client.CompanyLogo;
            Session["ClientID"] = CID;
            Session["ProductID"] = PID;
            Session["SurveyID"] = SID;
            ViewBag.SurveyID = SID;
            ViewBag.ProductID = PID;
            ViewBag.ClientID = CID;
            return View(obj.GetReviewQuesForAns(SID).ToList());
        }

        [HttpPost]
        [ActionName("GiveReview")]
        public ActionResult GiveReviewPost()
        {
            if (Session["UserID"] != null)
            {
                short SID = Convert.ToInt16(Session["SurveyID"]);
                short CID = Convert.ToInt16(Session["ClientID"]);
                short PID = Convert.ToInt16(Session["ProductID"]);
                short UID = Convert.ToInt16(Session["UserID"]);
                byte CGID = Convert.ToByte(Request["CategoryID"]);
                short QID = Convert.ToInt16(Request["QuestionID"]);
                string A = Request["Description"].ToString();
                if (A != null)
                {
                    obj.uspSaveReviewAns(SID, CGID, QID, UID, A);
                }
                ProductMst Product = obj.ProductMsts.Single(P => P.ClientID == CID & P.ProductID == PID);
                ClientMst Client = obj.ClientMsts.Single(C => C.ClientID == CID);
                ViewBag.ProductName = Product.ProductName;
                ViewBag.ModelName = Product.ModelName;
                ViewBag.CompName = Client.CompName;
                ViewBag.OwnerName = Client.OwnerName;
                ViewBag.ContactNo = Client.ContactNo;
                ViewBag.EmailID = Client.EmailID;
                ViewBag.CompanyLogo = Client.CompanyLogo;
                ViewBag.SurveyID = SID;
                ViewBag.ProductID = PID;
                ViewBag.ClientID = CID;
                return View(obj.GetReviewQuesForAns(SID).ToList());
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult ShowReviewUser()
        {
            if (Session["UserID"] != null)
            {
                short UID = Convert.ToInt16(Session["UserID"]);
                short CID = Convert.ToInt16(Session["ClientID"]);
                short PID = Convert.ToInt16(Session["ProductID"]);
                short SID = Convert.ToInt16(Session["SurveyID"]);
                ProductMst Product = obj.ProductMsts.Single(P => P.ClientID == CID & P.ProductID == PID);
                ClientMst Client = obj.ClientMsts.Single(C => C.ClientID == CID);
                ViewBag.ProductName = Product.ProductName;
                ViewBag.ModelName = Product.ModelName;
                ViewBag.CompName = Client.CompName;
                ViewBag.OwnerName = Client.OwnerName;
                ViewBag.ContactNo = Client.ContactNo;
                ViewBag.EmailID = Client.EmailID;
                ViewBag.CompanyLogo = Client.CompanyLogo;
                ViewBag.SurveyID = SID;
                ViewBag.ProductID = PID;
                ViewBag.ClientID = CID;
                return View(obj.GetReviewAnsForUser(UID, SID).ToList());
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult ShowSurveyUser()
        {
            if (Session["UserID"] != null)
            {
                short UID = Convert.ToInt16(Session["UserID"]);
                short CID = Convert.ToInt16(Session["ClientID"]);
                short PID = Convert.ToInt16(Session["ProductID"]);
                short SID = Convert.ToInt16(Session["SurveyID"]);
                ProductMst Product = obj.ProductMsts.Single(P => P.ClientID == CID & P.ProductID == PID);
                ClientMst Client = obj.ClientMsts.Single(C => C.ClientID == CID);
                ViewBag.ProductName = Product.ProductName;
                ViewBag.ModelName = Product.ModelName;
                ViewBag.CompName = Client.CompName;
                ViewBag.OwnerName = Client.OwnerName;
                ViewBag.ContactNo = Client.ContactNo;
                ViewBag.EmailID = Client.EmailID;
                ViewBag.CompanyLogo = Client.CompanyLogo;
                ViewBag.SurveyID = SID;
                ViewBag.ProductID = PID;
                ViewBag.ClientID = CID;
                return View(obj.GetSurveyAnsForUser(UID, SID).ToList());
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult SurveySuccess(short SID)
        {
            SurveyMst Survey = obj.SurveyMsts.Single(S => S.SurveyID == SID);
            ViewBag.ProductName = Survey.ProductMst.ProductName;
            return View();
        }

        public ActionResult EWalletEntry(short SID)
        {
            if (Session["UserID"] != null)
            {
                short UserID = Convert.ToInt16(Session["UserID"]);
                obj.uspSaveEwalletMst(SID, UserID);
                return RedirectToAction("ViewProducts", obj.GetActiveProduct());
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        [HttpGet]
        [ActionName("TransferReward")]
        public ActionResult TransferRewardGet()
        {
            if (Session["UserID"] != null)
            {
                short UserID = Convert.ToInt16(Session["UserID"]);
                var RP = obj.GetUserEwallet(UserID).ToList();
                ViewBag.RewardPoint = RP.FirstOrDefault().RewardPoint;
                Session["RP"] = "0";
                Session["TransferAmt"] = "0";
                Session["TransferTo"] = "0";
                Session["UserName"] = "0";
                return View();
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        [HttpPost]
        [ActionName("TransferReward")]
        public ActionResult TransferRewardPost()
        {
            if (Session["UserID"] != null)
            {
                short UserID = Convert.ToInt16(Session["UserID"]);
                var RP = obj.GetUserEwallet(UserID).ToList();
                short RewardPoint = Convert.ToInt16(Request["RP"]);
                short TransferAmt = Convert.ToInt16(Request["TransferAmt"]);
                string TransferTo = Request["TransferTo"];
                string UserName = Request["UserName"];
                if (RewardPoint >= 3000 & TransferAmt >= 1000 & UserName != null & TransferTo != null)
                {
                    TransferAmount TA = new TransferAmount();
                    DateTime date = DateTime.Now;
                    TA.RefNo = "Ref" + date.Day + date.Month + date.Year + UserID;
                    TA.TotalTransferAmount = Convert.ToInt16(TransferAmt / 30);
                    if (this.IsCaptchaValid("Captcha is not valid"))
                    {
                        obj.uspSaveTransferAmount(UserID, TA.TAID, date.Date, TransferAmt, TA.TotalTransferAmount, TransferTo, UserName, TA.RefNo);
                        obj.uspSaveRedeemReward(UserID);
                        UserMst User = obj.UserMsts.Single(U => U.UserID == UserID);
                        try
                        {
                            string EmailID = User.EmailID;
                            MailMessage mail = new MailMessage();
                            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                            mail.From = new MailAddress("prosurvey63@gmail.com");
                            mail.To.Add(EmailID);
                            mail.Subject = "Confirmation Mail";
                            mail.Body = "Welcome to Survey Pro... " + Environment.NewLine + "Your Reward Point:" + TransferAmt + "is Successfully Transfer to Your"
                                + TransferTo + "Account";
                            SmtpServer.Port = 587;
                            SmtpServer.Credentials = new System.Net.NetworkCredential("prosurvey63@gmail.com", "surveypro07102147");
                            SmtpServer.EnableSsl = true;
                            SmtpServer.Send(mail);
                        }
                        catch
                        {
                        }
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.Error = "Captcha is not valid";
                        ViewBag.RewardPoint = RP.FirstOrDefault().RewardPoint;
                        return View();
                    }
                }
                else
                {
                    if (RewardPoint <= 3000)
                    {
                        Session["RP"] = "1";
                        ViewBag.RP = "You Don't have Enough Points";
                    }
                    if (TransferAmt <= 1000)
                    {
                        Session["TransferAmt"] = "1";
                        ViewBag.TransferAmt = "You Can't Transfer Lessthen 1000 Points";
                    }
                    if (TransferTo == "")
                    {
                        Session["TransferTo"] = "1";
                        ViewBag.TransferTo = "You Have To Give Wallet Name";
                    }
                    if (UserName == "")
                    {
                        Session["UserName"] = "1";
                        ViewBag.UserName = "You Have To Give User Name";
                    }
                    ViewBag.RewardPoint = RP.FirstOrDefault().RewardPoint;
                    return View();
                }
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult EWalletList()
        {
            if (Session["UserID"] != null)
            {
                short UserID = Convert.ToInt16(Session["UserID"]);
                return View(obj.GetUserEwalletList(UserID));
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult EWalletReport()
        {
            if (Session["UserID"] != null)
            {
                short UserID = Convert.ToInt16(Session["UserID"]);
                var Report = obj.UserWiseEwalletReport(UserID).ToList();
                ViewBag.ApprovedSurvey = Report.SingleOrDefault().ApprovedSurvey;
                ViewBag.NotApprovedSurvey = Report.SingleOrDefault().NotApprovedSurvey;
                return View();
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult TransferAmountReport()
        {
            if (Session["UserID"] != null)
            {
                short UserID = Convert.ToInt16(Session["UserID"]);
                var Report = obj.UserTransferAmountReport(UserID).ToList();
                ViewBag.RewardPoint = Report.SingleOrDefault().RewardPoint;
                ViewBag.TransferRewardPoint = Report.SingleOrDefault().TransferRewardPoint;
                return View();
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }
    }
}
