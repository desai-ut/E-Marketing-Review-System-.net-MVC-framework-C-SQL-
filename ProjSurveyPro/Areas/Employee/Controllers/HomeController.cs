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

namespace ProjSurveyPro.Areas.Employee.Controllers
{
    public class HomeController : Controller
    {
        dbEMRSEntities obj = new dbEMRSEntities();

        //
        // GET: /Employee/Home/

        public ActionResult Index(Int16? Status)
        {
            if (Session["EmpID"] != null)
            {
                Byte EmpID = Convert.ToByte(Session["EmpID"]);
                var Report = obj.GetEmployeeReport(EmpID).ToList();
                ViewBag.NewClient = Report.SingleOrDefault().NewClient;
                ViewBag.NewUser = Report.SingleOrDefault().NewUser;
                ViewBag.Product = Report.SingleOrDefault().Product;
                ViewBag.ApproveResponse = Report.SingleOrDefault().ApproveResponse;
                ViewBag.Category = Report.SingleOrDefault().Category;
                ViewBag.LastLoginDate = Session["EmpLastLoginDt"];
                ViewBag.Employee = Report.SingleOrDefault().Employee;

                ViewBag.Package = Report.SingleOrDefault().Package;
                ViewBag.Question = Report.SingleOrDefault().Question;
                ViewBag.AUser = Report.SingleOrDefault().AUser;
                ViewBag.AClient = Report.SingleOrDefault().AClient;
                if (Status != null)
                {
                    string Designation = Session["Designation"].ToString();
                    obj.UpdateLastLoginDate(EmpID, Designation);
                }
                return View();
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult State()
        {
            return View(obj.GetState());
        }

        [HttpGet]
        [ActionName("AddState")]
        public ActionResult AddStateGet(Int16? id)
        {
            if (id == null)
            {
                Session["Status"] = "Create";
                return View();
            }
            else
            {
                Session["Status"] = "Edit";
                StateMst State = obj.StateMsts.Find(id);
                return View(State);
            }
        }

        [HttpPost]
        [ActionName("AddState")]
        public ActionResult AddStatePost(ClsStateMst objState)
        {
            String Status = Session["Status"].ToString();
            if ((Status.CompareTo("Create")) == 0)
            {
                if (ModelState.IsValid)
                {
                    StateMst State = new StateMst();
                    State.StateID = objState.StateID;
                    State.StateName = objState.StateName;
                    obj.StateMsts.Add(State);
                    obj.SaveChanges();
                    return RedirectToAction("State");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    StateMst State = new StateMst();
                    State.StateID = objState.StateID;
                    State.StateName = objState.StateName;
                    obj.Entry(State).State = EntityState.Modified;
                    obj.SaveChanges();
                    return RedirectToAction("State");
                }
                else
                {
                    return View();
                }
            }
        }

        public ActionResult Category()
        {
            return View(obj.GetCategory());
        }

        [HttpGet]
        [ActionName("AddCategory")]
        public ActionResult AddCategoryGet(Int16? id)
        {
            if (id == null)
            {
                Session["Status"] = "Create";
                return View();
            }
            else
            {
                Session["Status"] = "Edit";
                CategoryMst Category = obj.CategoryMsts.Find(id);
                return View(Category);
            }
        }

        [HttpPost]
        [ActionName("AddCategory")]
        public ActionResult AddCategoryPost(ClsCategoryMst objCategory)
        {
            String Status = Session["Status"].ToString();
            if ((Status.CompareTo("Create")) == 0)
            {
                if (ModelState.IsValid)
                {
                    CategoryMst Category = new CategoryMst();
                    Category.CategoryID = objCategory.CategoryID;
                    Category.CategoryDisc = objCategory.CategoryDisc;
                    obj.CategoryMsts.Add(Category);
                    obj.SaveChanges();
                    return RedirectToAction("Category");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    CategoryMst Category = new CategoryMst();
                    Category.CategoryID = objCategory.CategoryID;
                    Category.CategoryDisc = objCategory.CategoryDisc;
                    obj.Entry(Category).State = EntityState.Modified;
                    obj.SaveChanges();
                    return RedirectToAction("Category");
                }
                else
                {
                    return View();
                }
            }
        }

        [HttpGet]
        [ActionName("City")]
        public ActionResult CityGet()
        {
            var State = obj.GetState();
            ViewBag.Slist = new SelectList(State, "StateID", "StateName");
            ViewBag.Vis = "First";
            return View(obj.GetCity());
        }

        [HttpPost]
        [ActionName("City")]
        public ActionResult CityPost()
        {
            try
            {
                int id = Convert.ToInt16(Request["ddlState"]);
                var State = obj.uspGetState();
                ViewBag.Slist = new SelectList(State, "StateID", "StateName", id);
                IEnumerable<CityMst> City = obj.CityMsts.Where(s => s.StateID == id);
                Session["Sid"] = id;
                return View(City);
            }
            catch (Exception)
            {
                return RedirectToAction("City");
            }
        }

        [HttpGet]
        [ActionName("AddCity")]
        public ActionResult AddCityGet(Int16? id)
        {
            if (id == null)
            {
                Session["Status"] = "Create";
                return View();
            }
            else
            {
                Session["Status"] = "Edit";
                int Sid = Convert.ToInt16(Session["Sid"]);
                CityMst city = obj.CityMsts.Single(s => s.StateID == Sid && s.CityID == id);
                return View(city);
            }
        }

        [HttpPost]
        [ActionName("AddCity")]
        public ActionResult AddCityPost(ClsCityMst Cityobj)
        {
            Cityobj.StateID = Convert.ToByte(Session["Sid"]);
            String Status = Session["Status"].ToString();
            if ((Status.CompareTo("Create")) == 0)
            {
                if (ModelState.IsValid)
                {
                    obj.City(Cityobj.StateID, Cityobj.CityID, Cityobj.CityName);
                    return RedirectToAction("City");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    obj.City(Cityobj.StateID, Cityobj.CityID, Cityobj.CityName);
                    return RedirectToAction("City");
                }
                else
                {
                    return View();
                }
            }
        }

        public ActionResult Employee()
        {
            return View(obj.GetInActiveEmployee());
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

        [HttpGet]
        [ActionName("AddEmployee")]
        public ActionResult AddEmployeeGet(byte? EID)
        {
            if (EID == null)
            {
                Session["Image"] = "0";
                Session["Status"] = "Create";
                return View();
            }
            else
            {
                Session["Status"] = "Edit";
                Session["Image"] = "0";
                EmployeeMst Emp = obj.EmployeeMsts.Single(e => e.EmpID == EID);
                return View(Emp);
            }
        }

        [HttpPost]
        [ActionName("AddEmployee")]
        public ActionResult AddEmployeePost(ClsEmployeeMst Emp, HttpPostedFileBase EmpImage)
        {
            DateTime date = DateTime.Now;
            DateTime dt = date.AddYears(-20);
            Emp.JoiningDate = date.Date;
            Emp.IsActive = false;
            if (Emp.Designation == "0")
            {
                Emp.Designation = "Admin";
            }
            else
            {
                Emp.Designation = "Employee";
            }
            String Status = Session["Status"].ToString();
            if ((Status.CompareTo("Create")) == 0)
            {
                if (ModelState.IsValid)
                {
                    if (EmpImage != null)
                    {
                        if (Emp.DtBirth != date.Date & Emp.DtBirth <= dt)
                        {
                            if ((EmpImage.ContentType == "image/png" || EmpImage.ContentType == "image/jpeg") && (EmpImage.ContentLength <= 4194304))
                            {
                                var extension = Path.GetExtension(EmpImage.FileName);
                                string fileName = "Emp" + date.ToString("yyyyMMddhhmmssfff") + extension;
                                Emp.EmpImage = "~/Contents/appimg/Employee/" + fileName;
                                EmpImage.SaveAs(Path.Combine(Server.MapPath("~/Contents/appimg/Employee/"), fileName));
                                try
                                {
                                    obj.uspSaveEmployeeMst(Emp.EmpID, Emp.EmpName, Emp.Gender, Emp.EmpAddress, Emp.MobileNo, Emp.EmailID, Emp.DtBirth, Emp.JoiningDate, Emp.Designation, Emp.EmpImage, Emp.IsActive);
                                    return RedirectToAction("Index");
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
                                if (EmpImage.ContentLength <= 4194304)
                                {
                                    ViewBag.ImageType = "Employee Image Size Must Be less then 4MB";
                                }
                                else
                                {
                                    ViewBag.ImageType = "Employee Image Must Be in jpg or png Format";
                                }
                                return View();
                            }
                        }
                        else
                        {
                            Session["Date"] = "1";
                            ViewBag.Date = "Enter Valid Date (Your Age Must be 20+)...";
                            return View();
                        }
                    }
                    else
                    {
                        Emp.EmpImage = "~/Contents/appimg/user.png";
                        try
                        {
                            obj.uspSaveEmployeeMst(Emp.EmpID, Emp.EmpName, Emp.Gender, Emp.EmpAddress, Emp.MobileNo, Emp.EmailID, Emp.DtBirth, Emp.JoiningDate, Emp.Designation, Emp.EmpImage, Emp.IsActive);
                            return RedirectToAction("Index");
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
            else
            {
                if (ModelState.IsValid)
                {
                    if (EmpImage != null)
                    {
                        if (Emp.DtBirth != date.Date & Emp.DtBirth <= dt)
                        {
                            if ((EmpImage.ContentType == "image/png" || EmpImage.ContentType == "image/jpeg") && (EmpImage.ContentLength <= 4194304))
                            {
                                var extension = Path.GetExtension(EmpImage.FileName);
                                string fileName = "Emp" + date.ToString("yyyyMMddhhmmssfff") + extension;
                                Emp.EmpImage = "~/Contents/appimg/Employee/" + fileName;
                                EmpImage.SaveAs(Path.Combine(Server.MapPath("~/Contents/appimg/Employee/"), fileName));
                                try
                                {
                                    obj.uspSaveEmployeeMst(Emp.EmpID, Emp.EmpName, Emp.Gender, Emp.EmpAddress, Emp.MobileNo, Emp.EmailID, Emp.DtBirth, Emp.JoiningDate, Emp.Designation, Emp.EmpImage, Emp.IsActive);
                                    return RedirectToAction("Index");
                                }
                                catch (Exception e)
                                {
                                    var sql = e.InnerException as System.Data.SqlClient.SqlException;
                                    if (sql.Number == 2627)
                                    {
                                        Session["Email"] = "1";
                                        ViewBag.Email = "EmailID Already Exist...";
                                    }
                                    EmployeeMst Employee = obj.EmployeeMsts.Single(E => E.EmpID == Emp.EmpID);
                                    return View(Employee);
                                }
                            }
                            else
                            {
                                Session["Image"] = "1";
                                if (EmpImage.ContentLength <= 4194304)
                                {
                                    ViewBag.ImageType = "Employee Image Size Must Be less then 4MB";
                                }
                                else
                                {
                                    ViewBag.ImageType = "Employee Image Must Be in jpg or png Format";
                                }
                                EmployeeMst Employee = obj.EmployeeMsts.Single(E => E.EmpID == Emp.EmpID);
                                return View(Employee);
                            }
                        }
                        else
                        {
                            EmployeeMst Employee = obj.EmployeeMsts.Single(E => E.EmpID == Emp.EmpID);
                            try
                            {
                                obj.uspSaveEmployeeMst(Emp.EmpID, Emp.EmpName, Emp.Gender, Emp.EmpAddress, Emp.MobileNo, Emp.EmailID, Emp.DtBirth, Emp.JoiningDate, Emp.Designation, Employee.EmpImage, Emp.IsActive);
                                return RedirectToAction("Employee", obj.GetInActiveEmployee());
                            }
                            catch (Exception e)
                            {
                                var sql = e.InnerException as System.Data.SqlClient.SqlException;
                                if (sql.Number == 2627)
                                {
                                    Session["Email"] = "1";
                                    ViewBag.Email = "EmailID Already Exist...";
                                }
                                return View(Employee);
                            }
                        }
                    }
                    else
                    {
                        Session["Date"] = "1";
                        ViewBag.Date = "Enter Valid Date (Your Age Must be 20+)...";
                        EmployeeMst Employee = obj.EmployeeMsts.Single(E => E.EmpID == Emp.EmpID);
                        return View(Employee);
                    }
                }
                else
                {
                    EmployeeMst Employee = obj.EmployeeMsts.Single(E => E.EmpID == Emp.EmpID);
                    return View(Employee);
                }
            }
        }

        public ActionResult Package()
        {
            if (Session["EmpID"] != null)
            {
                byte EmpID = Convert.ToByte(Session["EmpID"]);
                return View(obj.GetEmployeePackage(EmpID));
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        [HttpGet]
        [ActionName("AddPackage")]
        public ActionResult AddPackageGet(Int16? id)
        {
            if (id == null)
            {
                Session["Status"] = "Create";
                return View();
            }
            else
            {
                Session["Status"] = "Edit";
                PackageMst Package = obj.PackageMsts.Find(id);
                return View(Package);
            }
        }

        [HttpPost]
        [ActionName("AddPackage")]
        public ActionResult AddPackagePost(ClsPackageMst Package)
        {
            if (Session["EmpID"] != null)
            {
                String Status = Session["Status"].ToString();
                if ((Status.CompareTo("Create")) == 0)
                {
                    Package.EmpID = Convert.ToByte(Session["EmpID"]);
                    if (ModelState.IsValid)
                    {
                        obj.uspSavePackageMst(Package.PackageID, Package.PackageTitle, Package.Discription, Package.Price, Package.Duration, Package.NoOfProduct, Package.NoOfSurveyQues, Package.NoOfReviewQues, Package.NoOfResponse, Package.EmpID);
                        return RedirectToAction("Package");
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        obj.uspSavePackageMst(Package.PackageID, Package.PackageTitle, Package.Discription, Package.Price, Package.Duration, Package.NoOfProduct, Package.NoOfSurveyQues, Package.NoOfReviewQues, Package.NoOfResponse, Package.EmpID);
                        return RedirectToAction("Package");
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        [HttpGet]
        [ActionName("HelpingQue")]
        public ActionResult HelpingQueGet()
        {
            var Category = obj.GetCategory();
            ViewBag.Clist = new SelectList(Category, "CategoryID", "CategoryDisc");
            return View(obj.GetHelpingQues());
        }

        [HttpPost]
        [ActionName("HelpingQue")]
        public ActionResult HelpingQuePost()
        {
            try
            {
                int id = Convert.ToInt16(Request["ddlCategory"]);
                var Category = obj.GetCategory();
                ViewBag.Clist = new SelectList(Category, "CategoryID", "CategoryDisc", id);
                IEnumerable<HelpingQue> Ques = obj.HelpingQues.Where(s => s.CategoryID == id);
                return View(Ques);
            }
            catch (Exception)
            {
                return RedirectToAction("HelpingQue", obj.GetHelpingQues());
            }
        }

        [HttpGet]
        [ActionName("AddHelpingQue")]
        public ActionResult AddHelpingQueGet()
        {
            short QID = Convert.ToInt16(Request["QuestionID"]);
            if (QID == 0)
            {
                var CategoryList = obj.GetCategory();
                ViewBag.Clist = new SelectList(CategoryList, "CategoryID", "CategoryDisc");
                Session["Status"] = "Create";
                return View();
            }
            else
            {
                byte CID = Convert.ToByte(Request["CategoryID"]);
                HelpingQue Ques = obj.HelpingQues.Single(q => q.CategoryID == CID && q.QuestionID == QID);
                var CategoryList = obj.GetCategory();
                ViewBag.Clist = new SelectList(CategoryList, "CategoryID", "CategoryDisc");
                Session["CID"] = CID;
                Session["Status"] = "Edit";
                return View(Ques);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        [ActionName("AddHelpingQue")]
        public ActionResult AddHelpingQuePost(ClsHelpingQue Ques)
        {
            String Status = Session["Status"].ToString();
            if ((Status.CompareTo("Create")) == 0)
            {
                if (ModelState.IsValid)
                {
                    obj.uspSaveHelpingQues(Ques.CategoryID, Ques.QuestionID, Ques.Question, Ques.Answer1, Ques.Answer2, Ques.Answer3, Ques.Answer4, Ques.Answer5, Ques.Answer6, Ques.Answer7);
                    var Category = obj.GetCategory();
                    ViewBag.Clist = new SelectList(Category, "CategoryID", "CategoryDisc");
                    return RedirectToAction("HelpingQue", obj.GetHelpingQues());
                }
                else
                {
                    var Category = obj.GetCategory();
                    ViewBag.CList = new SelectList(Category, "CategoryID", "CategoryDisc", Ques.CategoryID);
                    return View();
                }
            }
            else
            {
                if (ModelState.IsValid)
                {
                    obj.uspSaveHelpingQues(Ques.CategoryID, Ques.QuestionID, Ques.Question, Ques.Answer1, Ques.Answer2, Ques.Answer3, Ques.Answer4, Ques.Answer5, Ques.Answer6, Ques.Answer7);
                    var Category = obj.GetCategory();
                    ViewBag.Clist = new SelectList(Category, "CategoryID", "CategoryDisc");
                    return View("HelpingQue", obj.GetHelpingQues());
                }
                else
                {
                    byte CID = Convert.ToByte(Session["CID"]);
                    var CategoryList = obj.GetCategory();
                    ViewBag.CList = new SelectList(CategoryList, "CategoryID", "CategoryDisc", CID);
                    HelpingQue Quesobj = obj.HelpingQues.Single(q => q.QuestionID == Ques.QuestionID & q.CategoryID == CID);
                    return View(Quesobj);
                }
            }
        }

        public ActionResult DeleteHelpingQues()
        {
            short QID = Convert.ToInt16(Request["QuestionID"]);
            byte CID = Convert.ToByte(Request["CategoryID"]);
            var Ques = obj.HelpingQues.Single(q => q.CategoryID == CID && q.QuestionID == QID);
            if (Ques != null)
            {
                obj.Entry(Ques).State = EntityState.Deleted;
                obj.SaveChanges();
            }
            return View("HelpingQue", obj.GetHelpingQues());
        }

        [HttpGet]
        [ActionName("ApproveClient")]
        public ActionResult ApproveClientGet()
        {
            return View(obj.GetClient(0));
        }

        [HttpGet]
        [ActionName("ApproveUser")]
        public ActionResult ApproveUserGet()
        {
            return View(obj.GetUser(0));
        }

        public ActionResult SetApproveClient(short CID, bool Status)
        {
            DateTime date = DateTime.Now;
            if (Session["EmpID"] != null)
            {
                byte EmpID = Convert.ToByte(Session["EmpID"]);
                obj.uspSaveApprovedClient(CID, Status, EmpID, date.Date);
                try
                {
                    var Emp = obj.GetClientEIDAndPWD(CID).ToList();
                    string EmailID = Emp.SingleOrDefault().EmailID;
                    string Passwd = Emp.SingleOrDefault().Passwd;
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    mail.From = new MailAddress("prosurvey63@gmail.com");
                    mail.To.Add(EmailID);
                    mail.Subject = "Confirmation Mail";
                    mail.Body = "Welcome to Survey Pro..." + Environment.NewLine + "Your Login Password is : " + Passwd;
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("prosurvey63@gmail.com", "surveypro07102147");
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
                }
                catch
                {
                }
                return RedirectToAction("ApproveClient", obj.GetClient(0));
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult SetApproveUser(short UID, bool Status)
        {
            DateTime date = DateTime.Now;
            if (Session["EmpID"] != null)
            {
                byte EmpID = Convert.ToByte(Session["EmpID"]);
                obj.uspSaveApprovedUser(UID, Status, date.Date, EmpID);
                try
                {
                    var Emp = obj.GetUserEIDAndPWD(UID).ToList();
                    string EmailID = Emp.SingleOrDefault().EmailID;
                    string Passwd = Emp.SingleOrDefault().Passwd;
                    MailMessage mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                    mail.From = new MailAddress("prosurvey63@gmail.com");
                    mail.To.Add(EmailID);
                    mail.Subject = "Confirmation Mail";
                    mail.Body = "Welcome to Survey Pro..." + Environment.NewLine + "Your Login Password is : " + Passwd;
                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("prosurvey63@gmail.com", "surveypro07102147");
                    SmtpServer.EnableSsl = true;
                    SmtpServer.Send(mail);
                }
                catch
                {
                }
                return RedirectToAction("ApproveUser", obj.GetUser(0));
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult ClientDetails(Int16 id)
        {
            ClientMst Client = obj.ClientMsts.Find(id);
            return View(Client);
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

        [HttpGet]
        [ActionName("UpdateProfile")]
        public ActionResult UpdateProfileGet()
        {
            if (Session["EmpID"] != null)
            {
                byte EmpID = Convert.ToByte(Session["EmpID"]);
                EmployeeMst Emp = obj.EmployeeMsts.Single(E => E.EmpID == EmpID);
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
            if (Session["EmpID"] != null)
            {
                byte EmpID = Convert.ToByte(Session["EmpID"]);
                DateTime date = DateTime.Now;
                DateTime dt = date.AddYears(-20);
                Emp.EmpID = Convert.ToByte(Session["EmpID"]);
                if (ModelState.IsValid)
                {
                    if (Emp.DtBirth != date.Date & Emp.DtBirth <= dt)
                    {
                        try
                        {
                            obj.uspSaveEmployeeMst(Emp.EmpID, Emp.EmpName, Emp.Gender, Emp.EmpAddress, Emp.MobileNo, Emp.EmailID, Emp.DtBirth, Emp.JoiningDate, Emp.Designation, Emp.EmpImage, Emp.IsActive);
                            EmployeeMst Employee = obj.EmployeeMsts.Single(E => E.EmpID == EmpID);
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
                            EmployeeMst Empobj = obj.EmployeeMsts.Single(E => E.EmpID == EmpID);
                            return View(Empobj);
                        }
                    }
                    else
                    {
                        Session["Date"] = "1";
                        ViewBag.Date = "Enter Valid Date (Your Age Must be 20+)...";
                        EmployeeMst Empobj = obj.EmployeeMsts.Single(E => E.EmpID == EmpID);
                        return View(Empobj);
                    }
                }
                else
                {
                    EmployeeMst Empobj = obj.EmployeeMsts.Single(E => E.EmpID == EmpID);
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
            if (Session["EmpID"] != null)
            {
                byte EmpID = Convert.ToByte(Session["EmpID"]);
                if (EmpImage != null)
                {
                    if ((EmpImage.ContentType == "image/png" || EmpImage.ContentType == "image/jpeg") && (EmpImage.ContentLength <= 4194304))
                    {
                        DateTime date = DateTime.Now;
                        var extension = Path.GetExtension(EmpImage.FileName);
                        string fileName = "Emp" + date.ToString("yyyyMMddhhmmssfff") + extension;
                        EmployeeMst Emp = new EmployeeMst();
                        Emp.EmpID = EmpID;
                        Emp.EmpImage = "~/Contents/appimg/Employee/" + fileName;
                        EmpImage.SaveAs(Path.Combine(Server.MapPath("~/Contents/appimg/Employee/"), fileName));
                        obj.uspSaveUpdateImageEmployee(Emp.EmpID, Emp.EmpImage);
                        EmployeeMst Employee = obj.EmployeeMsts.Single(E => E.EmpID == EmpID);
                        Session["EmpImage"] = Employee.EmpImage;
                        return View("UpdateProfile", Employee);
                    }
                    else
                    {
                        Session["Image"] = "1";
                        if (EmpImage.ContentLength <= 4194304)
                        {
                            ViewBag.ImageType = "Employee Image Size Must Be less then 4MB";
                        }
                        else
                        {
                            ViewBag.ImageType = "Employee Image Must Be in jpg or png Format";
                        }
                        EmployeeMst Empobj = obj.EmployeeMsts.Single(E => E.EmpID == EmpID);
                        return View("UpdateProfile", Empobj);
                    }
                }
                else
                {
                    EmployeeMst Empobj = obj.EmployeeMsts.Single(E => E.EmpID == EmpID);
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
        public ActionResult ChangePasswordPost(string Password, string NewPassword, string ConfirmPassword)
        {
            if (Session["EmpID"] != null)
            {
                byte EmpID = Convert.ToByte(Session["EmpID"]);
                if (Password != "" && NewPassword != "" && ConfirmPassword != "")
                {
                    if (NewPassword.CompareTo(ConfirmPassword) == 0)
                    {
                        var Status = obj.uspSaveChangePasswdEmployee(EmpID, Password, NewPassword);
                        if (Convert.ToInt16(Status.SingleOrDefault().Value) == EmpID)
                        {
                            EmployeeMst Emp = obj.EmployeeMsts.Single(E => E.EmpID == EmpID);
                            try
                            {
                                string EmailID = Emp.EmailID;
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

        public ActionResult ViewProductResponse()
        {
            Session["Area"] = "Employee";
            return View(obj.GetProductResponseForEmployee());
        }

        public ActionResult ViewResponse(short SID)
        {
            Session["Area"] = "Employee";
            Session["SurveyID"] = SID;
            ViewBag.Grid = obj.GetResponse(SID).ToList();
            return View();
        }

        public ActionResult ApproveSurvey(short UID, short SID, bool Status)
        {
            obj.uspSaveApprovedEwallet(SID, UID, Status);
            ViewBag.Grid = obj.GetResponse(SID).ToList();
            EwalletMst User = obj.EwalletMsts.Single(U => U.SurveyID == SID & U.UserID == UID);
            try
            {
                string EmailID = User.UserMst.EmailID;
                string ProductName = User.SurveyMst.ProductMst.ProductName;
                short? Reward = User.SurveyMst.ProductMst.SurveyRewardPoint;
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("prosurvey63@gmail.com");
                mail.To.Add(EmailID);
                mail.Subject = "Confirmation Mail";
                mail.Body = "Welcome to Survey Pro... " + Environment.NewLine + "Your Survey on Product" + ProductName + "is Approve." + Environment.NewLine +
                    "Your Reward Point" + Reward + "is Add into Your Total Reward Point";
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("prosurvey63@gmail.com", "surveypro07102147");
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            catch
            {
            }
            return View("ViewResponse");
        }

        public ActionResult ShowSurvey(short UID, short SID)
        {
            UserMst User = obj.UserMsts.Single(U => U.UserID == UID);
            ViewBag.UserName = User.UserName;
            ViewBag.UserImage = User.UserImage;
            return View(obj.GetSurveyAnsForUser(UID, SID).ToList());
        }

        public ActionResult ShowReview(short UID, short SID)
        {
            UserMst User = obj.UserMsts.Single(U => U.UserID == UID);
            ViewBag.UserName = User.UserName;
            ViewBag.UserImage = User.UserImage;
            return View(obj.GetReviewAnsForUser(UID, SID).ToList());
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

        public ActionResult SurveyStatusReport()
        {
            var Report = obj.SurveyPieChartReport(0).ToList();
            ViewBag.ApprovedSurvey = Report.SingleOrDefault().ApprovedSurvey;
            ViewBag.NotApprovedSurvey = Report.SingleOrDefault().NotApprovedSurvey;
            return View();
        }

        [HttpGet]
        [ActionName("PackageReport")]
        public ActionResult PackageReportGet()
        {
            var Client = obj.GetClient(0);
            ViewBag.ClientList = new SelectList(Client, "ClientID", "CompName");
            ViewBag.PackageReport = obj.ClientPackageReport(0).ToList();
            return View();
        }

        [HttpPost]
        [ActionName("PackageReport")]
        public ActionResult PackageReportPost()
        {
            try
            {
                short id = Convert.ToInt16(Request["ddlClient"]);
                var Client = obj.GetClient(0);
                ViewBag.ClientList = new SelectList(Client, "ClientID", "CompName", id);
                ViewBag.PackageReport = obj.ClientPackageReport(id).ToList();
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("PackageReport");
            }
        }

        [HttpGet]
        [ActionName("SurveyPaymentReport")]
        public ActionResult SurveyPaymentReportGet()
        {
            var Client = obj.GetClient(0);
            ViewBag.ClientList = new SelectList(Client, "ClientID", "CompName");
            ViewBag.SurveyReport = obj.ClientSurveyReport(0).ToList();
            return View();
        }

        [HttpPost]
        [ActionName("SurveyPaymentReport")]
        public ActionResult SurveyPaymentReportPost()
        {
            try
            {
                short id = Convert.ToInt16(Request["ddlClient"]);
                var Client = obj.GetClient(0);
                ViewBag.ClientList = new SelectList(Client, "ClientID", "CompName", id);
                ViewBag.SurveyReport = obj.ClientSurveyReport(id).ToList();
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("SurveyPaymentReport");
            }
        }

        [HttpGet]
        [ActionName("DateWiseProductReport")]
        public ActionResult DateWiseProductReportGet()
        {
            ViewBag.ProductReport = obj.DateWiseProductReport("").ToList();
            return View();
        }

        [HttpPost]
        [ActionName("DateWiseProductReport")]
        public ActionResult DateWiseProductReportPost()
        {
            try
            {
                string Date = (Request["Date"]).ToString();
                ViewBag.ProductReport = obj.DateWiseProductReport(Date).ToList();
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("DateWiseProductReport");
            }
        }

        [HttpGet]
        [ActionName("DateWiseSurveyPaymentReport")]
        public ActionResult DateWiseSurveyPaymentReportGet()
        {
            ViewBag.SurveyPaymentReport = obj.DateWiseSurveyPaymentReport("").ToList();
            return View();
        }

        [HttpPost]
        [ActionName("DateWiseSurveyPaymentReport")]
        public ActionResult DateWiseSurveyPaymentReportPost()
        {
            try
            {
                string Date = (Request["Date"]).ToString();
                ViewBag.SurveyPaymentReport = obj.DateWiseSurveyPaymentReport(Date).ToList();
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("DateWiseSurveyPaymentReport");
            }
        }

        [HttpGet]
        [ActionName("DateWiseTransferAmountReport")]
        public ActionResult DateWiseTransferAmountReportGet()
        {
            ViewBag.TransferAmountReport = obj.DateWiseTransferAmountReport("").ToList();
            return View();
        }

        [HttpPost]
        [ActionName("DateWiseTransferAmountReport")]
        public ActionResult DateWiseTransferAmountReportPost()
        {
            try
            {
                string Date = (Request["Date"]).ToString();
                ViewBag.TransferAmountReport = obj.DateWiseTransferAmountReport(Date).ToList();
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("DateWiseSurveyPaymentReport");
            }
        }

        [HttpGet]
        [ActionName("StateWiseUserReport")]
        public ActionResult StateWiseUserReportGet()
        {
            var State = obj.GetState(0);
            ViewBag.StateList = new SelectList(State, "StateID", "StateName");
            ViewBag.StateWiseUserReport = obj.StateWiseUserReport(0).ToList();
            return View();
        }

        [HttpPost]
        [ActionName("StateWiseUserReport")]
        public ActionResult StateWiseUserReportPost()
        {
            try
            {
                short StateID = Convert.ToInt16(Request["ddlState"]);
                var State = obj.GetState(0);
                ViewBag.StateList = new SelectList(State, "StateID", "StateName", StateID);
                ViewBag.StateWiseUserReport = obj.StateWiseUserReport(StateID).ToList();
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("StateWiseUserReport");
            }
        }

        [HttpGet]
        [ActionName("CityWiseUserReport")]
        public ActionResult CityWiseUserReportGet()
        {
            var State = obj.GetState(0);
            ViewBag.StateList = new SelectList(State, "StateID", "StateName");
            ViewBag.CityWiseUserReport = obj.CityWiseUserReport(0, 0).ToList();
            return View();
        }

        [HttpPost]
        [ActionName("CityWiseUserReport")]
        public ActionResult CityWiseUserReportPost()
        {
            try
            {
                short StateID = Convert.ToInt16(Session["StateIDReport"]);
                short CityID = Convert.ToInt16(Request["ddlCity"]);
                var State = obj.GetState(0);
                ViewBag.StateList = new SelectList(State, "StateID", "StateName", StateID);
                List<CityMst> City = new List<CityMst>();
                City = obj.CityMsts.Where(C => C.StateID == StateID).ToList();
                ViewBag.CityList = new SelectList(City, "CityID", "CityName", CityID);
                ViewBag.CityWiseUserReport = obj.CityWiseUserReport(StateID, CityID).ToList();
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("CityWiseUserReport");
            }
        }

        [HttpPost]
        public ActionResult FillCity(int StateID)
        {
            List<CityMst> CList = new List<CityMst>();
            CList = obj.CityMsts.Where(m => m.StateID == StateID).ToList();
            SelectList CityList = new SelectList(CList, "CityID", "CityName", "StateID");
            Session["StateIDReport"] = StateID;
            return Json(CityList);
        }
    }
}
