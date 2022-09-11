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
using System.Net.Mail;

namespace ProjSurveyPro.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        dbEMRSEntities obj = new dbEMRSEntities();

        //
        // GET: /Admin/Home/

        public ActionResult Index(Int16? Status)
        {
            var Report = obj.GetAdminReport().ToList();
            ViewBag.PurchasePackage = Report.SingleOrDefault().PurchasePackage;
            ViewBag.Product = Report.SingleOrDefault().Product;
            ViewBag.AEmployee = Report.SingleOrDefault().AEmployee;
            ViewBag.NEmployee = Report.SingleOrDefault().NewEmployee;
            ViewBag.NPackage = Report.SingleOrDefault().NPackage;
            ViewBag.AProduct = Report.SingleOrDefault().AProduct;
            ViewBag.TResponse = Report.SingleOrDefault().TotalResponse;
            ViewBag.Feedback = Report.SingleOrDefault().Feedback;
            ViewBag.Query = Report.SingleOrDefault().Query;

            ViewBag.AUser = Report.SingleOrDefault().AUser;
            ViewBag.AClient = Report.SingleOrDefault().AClient;
            ViewBag.Question = Report.SingleOrDefault().Question;
            ViewBag.APackage = Report.SingleOrDefault().APackage;
            if (Status != null)
            {
                Byte ID = Convert.ToByte(Session["AdminID"]);
                if (Session["Designation"] != null)
                {
                    string Designation = Session["Designation"].ToString();
                    obj.UpdateLastLoginDate(ID, Designation);
                }
                else
                {
                    return RedirectToAction("SignIn", "Home", new { area = "" });
                }
            }
            return View();
        }

        public ActionResult Package()
        {
            return View(obj.GetPackage(0));
        }

        public ActionResult SetApprovePackage(byte PID, bool Status)
        {
            obj.uspSaveApprovedPackage(PID, Status);
            return RedirectToAction("Package", obj.GetPackage(0));
        }

        public ActionResult Employee()
        {
            if (Session["AdminID"] != null)
            {
                byte AdminID = Convert.ToByte(Session["AdminID"]);
                return View(obj.GetEmployee(AdminID));
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult EmployeeDetails(byte EID)
        {
            EmployeeMst Emp = obj.EmployeeMsts.Single(E => E.EmpID == EID);
            if (Emp.Gender == true)
            {
                ViewBag.Gender = "Male";
            }
            else
            {
                ViewBag.Gender = "Female";
            }
            return View(Emp);
        }

        public ActionResult SetApproveEmployee(byte EID, bool Status)
        {
            if (Session["AdminID"] != null)
            {
                byte AdminID = Convert.ToByte(Session["AdminID"]);
                DateTime date = DateTime.Now;
                obj.uspSaveApprovedEmployee(EID, Status, date.Date);
                try
                {
                    var Emp = obj.GetEmpEIDAndPWD(EID).ToList();
                    string EmailID = Emp.SingleOrDefault().EmailID;
                    string Passwd = Emp.SingleOrDefault().Passwd;
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    mail.From = new MailAddress("prosurvey63@gmail.com");
                    mail.To.Add(EmailID);
                    mail.Subject = "Confirmation Mail";
                    mail.Body = "Welcome to Survey Pro... " + Environment.NewLine + "Your Login Password is : " + Passwd;
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("prosurvey63@gmail.com", "surveypro07102147");
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
                }
                catch
                {
                }
                return RedirectToAction("Employee", obj.GetEmployee(AdminID));
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult Clients()
        {
            return View(obj.GetClient(1));
        }

        public ActionResult ClientDetails(Int16 id)
        {
            ClientMst Client = obj.ClientMsts.Find(id);
            return View(Client);
        }

        public ActionResult Users()
        {
            return View(obj.GetUser(1));
        }

        public ActionResult UserDetails(Int16 id)
        {
            UserMst User = obj.UserMsts.Find(id);
            CityMst City = obj.CityMsts.Single(C => C.StateID == User.StateID & C.CityID == User.CityID);
            ViewBag.State = City.StateMst.StateName;
            ViewBag.City = City.CityName;
            if (User.Gender == true)
            {
                ViewBag.Gender = "Male";
            }
            else
            {
                ViewBag.Gender = "Female";
            }
            return View(User);
        }

        public ActionResult PurchasePackage()
        {
            return View(obj.GetPurchasePackage(0));
        }

        public ActionResult SetPurchasePackage(short PCID, bool Status, byte Duration)
        {
            DateTime date = DateTime.Now;
            PurchasePackage Data = new PurchasePackage();
            Data.PurchaseID = PCID;
            Data.StartingDate = date.Date;
            Data.PaidStatus = Status;
            if (Status == true)
            {
                Data.EndingDate = date.Date.AddMonths(Duration);
                PurchasePackage Client = obj.PurchasePackages.Single(C => C.PurchaseID == PCID);
                try
                {
                    string EmailID = Client.ClientMst.EmailID;
                    string Package = Client.PackageMst.PackageTitle;
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    mail.From = new MailAddress("prosurvey63@gmail.com");
                    mail.To.Add(EmailID);
                    mail.Subject = "Confirmation Mail";
                    mail.Body = "Welcome to Survey Pro... " + Environment.NewLine + "Thank you For Purchase package:" + Package + Environment.NewLine + "Now, You Can Add Product.";
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("prosurvey63@gmail.com", "surveypro07102147");
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
                }
                catch
                {
                }
                obj.uspSaveApprovedPurchasePackage(Data.PurchaseID, Data.StartingDate, Data.EndingDate, Data.PaidStatus);
            }
            else
            {
                Data.EndingDate = date.Date;
                obj.uspSaveApprovedPurchasePackage(Data.PurchaseID, Data.StartingDate, Data.EndingDate, Data.PaidStatus);
            }
            return RedirectToAction("PurchasePackage", obj.GetPurchasePackage(0));
        }

        public ActionResult ActiveProduct()
        {
            return View(obj.GetPublishProduct());
        }

        public ActionResult Activate(short PID, short CID, decimal RP, bool Status)
        {
            short x = Convert.ToInt16((RP * 60 / 100) * 30);
            obj.uspSaveActiveProduct(CID, PID, Status, x, Status);
            try
            {
                ProductMst Product = obj.ProductMsts.Single(P => P.ClientID == CID & P.ProductID == PID);
                string EmailID = Product.ClientMst.EmailID;
                string ProductName = Product.ProductName;
                string ModelName = Product.ModelName;
                string CompName = Product.ProductCompany;
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("prosurvey63@gmail.com");
                mail.To.Add(EmailID);
                mail.Subject = "Confirmation Mail";
                mail.Body = "Welcome to Survey Pro... " + Environment.NewLine + CompName + "Your Product : " + ProductName + Environment.NewLine +
                    "Model Name : " + ModelName + Environment.NewLine + "SurveyPro Status is : Active";
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("prosurvey63@gmail.com", "surveypro07102147");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            catch
            {
            }
            return RedirectToAction("ActiveProduct", obj.GetPublishProduct());
        }

        [HttpGet]
        [ActionName("UpdateProfile")]
        public ActionResult UpdateProfile()
        {
            if (Session["AdminID"] != null)
            {
                byte AdminID = Convert.ToByte(Session["AdminID"]);
                EmployeeMst Emp = obj.EmployeeMsts.Single(E => E.EmpID == AdminID);
                return View(Emp);
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        [HttpPost]
        [ActionName("UpdateProfile")]
        public ActionResult UpdateProfilePost(ClsEmployeeMst Emp)
        {
            if (Session["AdminID"] != null)
            {
                byte AdminID = Convert.ToByte(Session["AdminID"]);
                DateTime date = DateTime.Now;
                DateTime dt = date.AddYears(-20);
                Emp.EmpID = Convert.ToByte(Session["AdminID"]);
                Emp.Designation = "Admin";
                if (ModelState.IsValid)
                {
                    if (Emp.DtBirth != date.Date & Emp.DtBirth <= dt)
                    {
                        try
                        {
                            obj.uspSaveEmployeeMst(Emp.EmpID, Emp.EmpName, Emp.Gender, Emp.EmpAddress, Emp.MobileNo, Emp.EmailID, Emp.DtBirth, Emp.JoiningDate, Emp.Designation, Emp.EmpImage, Emp.IsActive);
                            EmployeeMst Employee = obj.EmployeeMsts.Single(E => E.EmpID == AdminID);
                            return View("UpdateProfile", Employee);
                        }
                        catch (Exception e)
                        {
                            var sql = e.InnerException as System.Data.SqlClient.SqlException;
                            if (sql.Number == 2627)
                            {
                                Session["Email"] = "1";
                                ViewBag.Email = "EmailID Already Exist...";
                            }
                            EmployeeMst Empobj = obj.EmployeeMsts.Single(E => E.EmpID == AdminID);
                            return View(Empobj);
                        }
                    }
                    else
                    {
                        Session["Date"] = "1";
                        ViewBag.Date = "Enter Valid Date (Your Age Must be 20+)...";
                        EmployeeMst Empobj = obj.EmployeeMsts.Single(E => E.EmpID == AdminID);
                        return View(Empobj);
                    }
                }
                else
                {
                    EmployeeMst Empobj = obj.EmployeeMsts.Single(E => E.EmpID == AdminID);
                    return View(Empobj);
                }
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        [HttpPost]
        public ActionResult UpdateImage(HttpPostedFileBase EmpImage)
        {
            if (Session["AdminID"] != null)
            {
                byte AdminID = Convert.ToByte(Session["AdminID"]);
                if (EmpImage != null)
                {
                    if ((EmpImage.ContentType == "image/png" || EmpImage.ContentType == "image/jpeg") && (EmpImage.ContentLength <= 4194304))
                    {
                        DateTime date = DateTime.Now;
                        var extension = Path.GetExtension(EmpImage.FileName);
                        string fileName = "Emp" + date.ToString("yyyyMMddhhmmssfff") + extension;
                        EmployeeMst Emp = new EmployeeMst();
                        Emp.EmpID = AdminID;
                        Emp.EmpImage = "~/Contents/appimg/Employee/" + fileName;
                        EmpImage.SaveAs(Path.Combine(Server.MapPath("~/Contents/appimg/Employee/"), fileName));
                        obj.uspSaveUpdateImageEmployee(Emp.EmpID, Emp.EmpImage);
                        EmployeeMst Employee = obj.EmployeeMsts.Single(E => E.EmpID == AdminID);
                        Session["AdminImage"] = Employee.EmpImage;
                        return View("UpdateProfile", Employee);
                    }
                    else
                    {
                        Session["Image"] = "1";
                        if (EmpImage.ContentLength <= 4194304)
                        {
                            ViewBag.ImageType = "Admin Image Size Must Be less then 4MB";
                        }
                        else
                        {
                            ViewBag.ImageType = "Admin Image Must Be in jpg or png Format";
                        }
                        EmployeeMst Empobj = obj.EmployeeMsts.Single(E => E.EmpID == AdminID);
                        return View("UpdateProfile", Empobj);
                    }
                }
                else
                {
                    EmployeeMst Empobj = obj.EmployeeMsts.Single(E => E.EmpID == AdminID);
                    return View("UpdateProfile", Empobj);
                }
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        [HttpGet]
        [ActionName("ChangePassword")]
        public ActionResult ChangePasswordGet()
        {
            return View();
        }

        [HttpPost]
        [ActionName("ChangePassword")]
        public ActionResult ChangePasswordGet(string Password, string NewPassword, string ConfirmPassword)
        {
            if (Session["AdminID"] != null)
            {
                byte AdminID = Convert.ToByte(Session["AdminID"]);
                if (Password != "" && NewPassword != "" && ConfirmPassword != "")
                {
                    if (NewPassword.CompareTo(ConfirmPassword) == 0)
                    {
                        var Status = obj.uspSaveChangePasswdEmployee(AdminID, Password, NewPassword);
                        if (Convert.ToInt16(Status.SingleOrDefault().Value) == AdminID)
                        {
                            EmployeeMst Admin = obj.EmployeeMsts.Single(A => A.EmpID == AdminID);
                            try
                            {
                                string EmailID = Admin.EmailID;
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
                            ViewBag.Pass = "Incorrect Password...";
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

        public ActionResult ProductDetails(short PID, short CID)
        {
            ProductMst Product = obj.ProductMsts.Single(P => P.ClientID == CID & P.ProductID == PID);
            Session["SRP"] = "Admin";
            Session["Btn"] = "Admin";
            return View(Product);
        }

        public ActionResult SurveyQuesList(short PID, short CID)
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
            return View(obj.GetSurveyQues(CID, PID));
        }

        public ActionResult ReviewQuesList(short PID, short CID)
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
            return View(obj.GetReviewQues(CID, PID));
        }

        [HttpGet]
        [ActionName("Query")]
        public ActionResult QueryGet()
        {
            return View(obj.GetQuery());
        }

        public ActionResult ReplyQuery(short QID, string Status)
        {
            if (Status == "Entry")
            {
                Session["Status"] = "Entry";
            }
            else
            {
                Session["Status"] = "Display";
            }
            QueryMst Query = obj.QueryMsts.Single(Q => Q.QueryID == QID);
            return View(Query);
        }

        [HttpPost]
        [ActionName("Query")]
        public ActionResult QueryPost(QueryMst Query)
        {
            string Btn = Request["Submit"];
            if (Btn == "Submit")
            {
                obj.uspSaveApprovedQuery(Query.QueryID, Query.ReplyDisc, true);
                try
                {
                    string EmailID = Query.EmailID;
                    string ReplyDisc = Query.ReplyDisc;
                    string QueryDisc = Query.QueryDisc;
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    mail.From = new MailAddress("prosurvey63@gmail.com");
                    mail.To.Add(EmailID);
                    mail.Subject = "Confirmation Mail";
                    mail.Body = "Welcome to Survey Pro..." + Environment.NewLine + "Your Query : " + QueryDisc + Environment.NewLine +
                        "Solution : " + ReplyDisc;
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("prosurvey63@gmail.com", "surveypro07102147");
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
                }
                catch
                {
                }
                return View(obj.GetQuery());
            }
            else
            {
                return RedirectToAction("ReplyQuery", Query);
            }
        }

        public ActionResult SystemKeyReport()
        {
            var Report = obj.SystemReport().ToList();
            ViewBag.AProduct = Report.SingleOrDefault().AProduct;
            ViewBag.AUser = Report.SingleOrDefault().AUser;
            ViewBag.Client = Report.SingleOrDefault().Client;
            ViewBag.Employee = Report.SingleOrDefault().Employee;
            ViewBag.Feedback = Report.SingleOrDefault().Feedback;
            ViewBag.Query = Report.SingleOrDefault().Query;
            ViewBag.Product = Report.SingleOrDefault().Product;
            ViewBag.TotalResponse = Report.SingleOrDefault().TotalResponse;
            ViewBag.PurchasePackage = Report.SingleOrDefault().PurchasePackage;
            return View();
        }

        public ActionResult EmployeeTaskReport()
        {
            var Report = obj.SystemReport().ToList();
            ViewBag.AState = Report.SingleOrDefault().AState;
            ViewBag.City = Report.SingleOrDefault().City;
            ViewBag.Category = Report.SingleOrDefault().Category;
            ViewBag.Package = Report.SingleOrDefault().Package;
            ViewBag.Question = Report.SingleOrDefault().Question;
            ViewBag.AttemptedReviewQues = Report.SingleOrDefault().AttemptedReviewQues;
            ViewBag.AttemptedSurveyQues = Report.SingleOrDefault().AttemptedSurveyQues;
            ViewBag.ApprovedSurveyResponse = Report.SingleOrDefault().ApprovedSurveyResponse;
            ViewBag.NotApprovedSurveyResponse = Report.SingleOrDefault().NotApprovedSurveyResponse;
            return View();
        }
    }
}
