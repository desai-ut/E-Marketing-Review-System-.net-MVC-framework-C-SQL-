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

namespace ProjSurveyPro.Areas.Client.Controllers
{
    public class HomeController : Controller
    {
        dbEMRSEntities obj = new dbEMRSEntities();

        //
        // GET: /Client/Home/

        public ActionResult Index(Int16? Status)
        {
            if (Session["ClientID"] != null)
            {
                short ClientID = Convert.ToInt16(Session["ClientID"]);
                var Report = obj.GetClientReport(ClientID).ToList();
                ViewBag.AUser = Report.SingleOrDefault().AUser;
                ViewBag.AttemptedSurvey = Report.SingleOrDefault().AttemptedSurvey;
                ViewBag.AttemptedReview = Report.SingleOrDefault().AttemptedReview;
                ViewBag.PackageTitle = Report.SingleOrDefault().PackageTitle;
                ViewBag.NoofProduct = Report.SingleOrDefault().NoofProduct;
                ViewBag.PackageEndDate = Report.SingleOrDefault().PackageEndDate;
                ViewBag.LastLoginDate = Session["ClientLastLoginDt"];
                ViewBag.Question = Report.SingleOrDefault().Question;

                ViewBag.Feedback = Report.SingleOrDefault().Feedback;
                ViewBag.Product = Report.SingleOrDefault().Product;
                ViewBag.AProduct = Report.SingleOrDefault().AProduct;
                ViewBag.TotalResponse = Report.SingleOrDefault().TotalResponse;
                if (Status != null)
                {
                    string Designation = Session["Designation"].ToString();
                    obj.UpdateLastLoginDate(ClientID, Designation);
                }
                return View();
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult Product()
        {
            if (Session["ClientID"] != null)
            {
                short ClientID = Convert.ToInt16(Session["ClientID"]);
                return View(obj.GetProduct(ClientID));
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        [HttpGet]
        [ActionName("AddProduct")]
        public ActionResult AddProductGet(Int16? id)
        {
            if (Session["ClientID"] != null)
            {
                short ClientID = Convert.ToInt16(Session["ClientID"]);
                var Status = obj.CheckPackage(ClientID);
                short? PStatus = Status.SingleOrDefault().ClientID;
                if (PStatus == 1)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("BuyPackage");
                }
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult BuyPackage()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ActionName("AddProduct")]
        public ActionResult AddProductPost(ClsProductMst Product, HttpPostedFileBase ProductImage1, HttpPostedFileBase ProductImage2, HttpPostedFileBase ProductImage3, HttpPostedFileBase ProductImage4, HttpPostedFileBase Video)
        {
            DateTime date = DateTime.Now;
            if (Session["ClientID"] != null)
            {
                short ClientID = Convert.ToInt16(Session["ClientID"]);
                Product.ClientID = ClientID;
                if (ModelState.IsValid)
                {
                    if (ProductImage1 != null)
                    {
                        if ((ProductImage1.ContentType == "image/png" || ProductImage1.ContentType == "image/jpeg") && (ProductImage1.ContentLength <= 4194304))
                        {
                            if (Video != null)
                            {
                                if (Video.ContentType == "video/mp4" && Video.ContentLength <= 6553600)
                                {
                                    var extensionVideo = Path.GetExtension(Video.FileName);
                                    string fileNameVideo = "Video" + date.ToString("yyyyMMddhhmmssfff") + extensionVideo;
                                    Product.Video = "~/Contents/appimg/Product/Video/" + fileNameVideo;
                                    Video.SaveAs(Path.Combine(Server.MapPath("~/Contents/appimg/Product/Video/"), fileNameVideo));
                                }
                                else
                                {
                                    Session["Image"] = "1";
                                    if (Video.ContentLength <= 6553600)
                                    {
                                        ViewBag.VideoType = "Product Video Size Must Be lessthen 4MB";
                                    }
                                    else
                                    {
                                        ViewBag.VideoType = "You Can Only Put MP4 Product Video";
                                    }
                                    return View();
                                }
                            }

                            if (ProductImage2 != null)
                            {
                                if ((ProductImage2.ContentType == "image/png" || ProductImage2.ContentType == "image/jpeg") && (ProductImage2.ContentLength <= 4194304))
                                {
                                    var extension2 = Path.GetExtension(ProductImage1.FileName);
                                    string fileName2 = "IMG2" + date.ToString("yyyyMMddhhmmssfff") + extension2;
                                    Product.ProductImage2 = "~/Contents/appimg/Product/" + fileName2;
                                    ProductImage2.SaveAs(Path.Combine(Server.MapPath("~/Contents/appimg/Product/"), fileName2));
                                }
                                else
                                {
                                    Session["Image"] = "1";
                                    if (ProductImage2.ContentLength <= 4194304)
                                    {
                                        ViewBag.Image2Type = "Product Image Size Must Be lessthen 4MB";
                                    }
                                    else
                                    {
                                        ViewBag.Image2Type = "Product Image Must Be in jpg or png Format";
                                    }
                                    return View();
                                }
                            }

                            if (ProductImage3 != null)
                            {
                                if ((ProductImage3.ContentType == "image/png" || ProductImage3.ContentType == "image/jpeg") && (ProductImage3.ContentLength <= 4194304))
                                {
                                    var extension3 = Path.GetExtension(ProductImage1.FileName);
                                    string fileName3 = "IMG3" + date.ToString("yyyyMMddhhmmssfff") + extension3;
                                    Product.ProductImage3 = "~/Contents/appimg/Product/" + fileName3;
                                    ProductImage3.SaveAs(Path.Combine(Server.MapPath("~/Contents/appimg/Product/"), fileName3));
                                }
                                else
                                {
                                    Session["Image"] = "1";
                                    if (ProductImage3.ContentLength <= 4194304)
                                    {
                                        ViewBag.Image3Type = "Product Image Size Must Be lessthen 4MB";
                                    }
                                    else
                                    {
                                        ViewBag.Image3Type = "Product Image Must Be in jpg or png Format";
                                    }
                                    return View();
                                }
                            }

                            if (ProductImage4 != null)
                            {
                                if ((ProductImage4.ContentType == "image/png" || ProductImage4.ContentType == "image/jpeg") && (ProductImage4.ContentLength <= 4194304))
                                {
                                    var extension4 = Path.GetExtension(ProductImage1.FileName);
                                    string fileName4 = "IMG4" + date.ToString("yyyyMMddhhmmssfff") + extension4;
                                    Product.ProductImage4 = "~/Contents/appimg/Product/" + fileName4;
                                    ProductImage4.SaveAs(Path.Combine(Server.MapPath("~/Contents/appimg/Product/"), fileName4));
                                }
                                else
                                {
                                    Session["Image"] = "1";
                                    if (ProductImage4.ContentLength <= 4194304)
                                    {
                                        ViewBag.Image4Type = "Product Image Size Must Be lessthen 4MB";
                                    }
                                    else
                                    {
                                        ViewBag.Image4Type = "Product Image Must Be in jpg or png Format";
                                    }
                                    return View();
                                }
                            }

                            var extension1 = Path.GetExtension(ProductImage1.FileName);
                            string fileName1 = "IMG1" + date.ToString("yyyyMMddhhmmssfff") + extension1;
                            Product.ProductImage1 = "~/Contents/appimg/Product/" + fileName1;
                            ProductImage1.SaveAs(Path.Combine(Server.MapPath("~/Contents/appimg/Product/"), fileName1));

                            obj.uspSaveProductMst(Product.ClientID, Product.ProductID, Product.ProductName, Product.ProductCompany, Product.ModelName, Product.ShortDiscription, Product.BriefDiscription, Product.Facility, Product.Price, Product.ProductImage1, Product.ProductImage2, Product.ProductImage3, Product.ProductImage4, Product.Video, Product.TotalSurveyAmt, Product.SurveyPerAmount, Product.NoOfSurvey);
                            return RedirectToAction("Product");
                        }
                        else
                        {
                            Session["Image"] = "1";
                            if (ProductImage1.ContentLength <= 4194304)
                            {
                                ViewBag.Image1Type = "Product Image Size Must Be lessthen 4MB";
                            }
                            else
                            {
                                ViewBag.Image1Type = "Product Image Must Be in jpg or png Format";
                            }
                            return View();
                        }
                    }
                    else
                    {
                        obj.uspSaveProductMst(Product.ClientID, Product.ProductID, Product.ProductName, Product.ProductCompany, Product.ModelName, Product.ShortDiscription, Product.BriefDiscription, Product.Facility, Product.Price, Product.ProductImage1, Product.ProductImage2, Product.ProductImage3, Product.ProductImage4, Product.Video, Product.TotalSurveyAmt, Product.SurveyPerAmount, Product.NoOfSurvey);
                        return RedirectToAction("Product");
                    }
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult ViewPackages()
        {
            return View(obj.GetPackage(1).ToList());
        }

        public ActionResult PurchasePackage(byte PID, decimal Price)
        {
            DateTime date = DateTime.Now;
            if (Session["ClientID"] != null)
            {
                short ClientID = Convert.ToInt16(Session["ClientID"]);
                string RefNo = "Ref" + date.Day + date.Month + date.Year + ClientID;
                var Status = obj.SavePurchasePackage(PID, ClientID, 0, Price, RefNo, date.Date).ToList();
                byte? PStatus = Status.SingleOrDefault().PStatus;
                if (PStatus == 1)
                {
                    return RedirectToAction("PackageSucess");
                }
                else
                {
                    short? PurchaseID = Status.SingleOrDefault().PurchaseID;
                    PurchasePackage PP = obj.PurchasePackages.Single(P => P.PurchaseID == PurchaseID);
                    ViewBag.PackageTitle = PP.PackageMst.PackageTitle;
                    ViewBag.ProductCnt = PP.ProductCnt;
                    ViewBag.EndingDate = PP.EndingDate;
                    return View("PackageExist");
                }
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult PackageSucess()
        {
            return View();
        }

        public ActionResult PackageExist()
        {
            return View();
        }

        [HttpGet]
        [ActionName("AddSurveyQues")]
        public ActionResult AddSurveyQuesGet(short id, decimal TSA, string Status)
        {
            var Category = obj.GetCategory();
            ViewBag.Clist = new SelectList(Category, "CategoryID", "CategoryDisc");
            Session["ProductID"] = id;
            Session["TSA"] = TSA;
            if (Status == "First")
            {
                SurveyMst Survey = new SurveyMst();
                Survey.ClientID = Convert.ToInt16(Session["ClientID"]);
                Survey.ProductID = id;
                Survey.SurveyID = 0;
                Survey.SurveyCnt = 0;
                var SID = obj.uspSaveSurveyMst(Survey.ClientID, Survey.ProductID, Survey.SurveyID, Survey.SurveyCnt);
                DateTime date = DateTime.Now;
                SurveyPayment SP = new SurveyPayment();
                short CID = Convert.ToInt16(Session["ClientID"]);
                SP.SPID = 0;
                SP.TotalAmt = Convert.ToInt16(TSA);
                SP.SurveyID = SID.SingleOrDefault().Value;
                SP.RefNo = "Ref" + date.Day + date.Month + date.Year + CID;
                obj.uspSaveSurveyPayment(SP.SPID, date.Date, SP.SurveyID, SP.TotalAmt, SP.RefNo);
                Session["SurveyID"] = SP.SurveyID;
            }
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ActionName("AddSurveyQues")]
        public ActionResult AddSurveyQuesPost(ClsSurveyQue Ques)
        {
            var Category = obj.GetCategory();
            ViewBag.Clist = new SelectList(Category, "CategoryID", "CategoryDisc");
            Ques.SurveyID = Convert.ToInt16(Session["SurveyID"]);
            if (ModelState.IsValid)
            {
                if (Ques.CategoryID == 1)//Descriptive
                {
                    obj.uspSaveReviewQues(Ques.SurveyID, Ques.CategoryID, Ques.QuestionID, Ques.Question);
                }
                else
                {
                    obj.uspSaveSurveyQues(Ques.SurveyID, Ques.CategoryID, Ques.QuestionID, Ques.Question, Ques.Answer1, Ques.Answer2, Ques.Answer3, Ques.Answer4, Ques.Answer5, Ques.Answer6, Ques.Answer7);
                }
            }
            ModelState.Clear();
            return View();
        }

        [HttpGet]
        [ActionName("HelpingQuesList")]
        public ActionResult HelpingQuesListGet()
        {
            var Category = obj.GetCategory();
            ViewBag.Clist = new SelectList(Category, "CategoryID", "CategoryDisc");
            ViewBag.ProductID = Session["ProductID"];
            return View(obj.GetHelpingQues().ToList());
        }

        [HttpPost]
        [ActionName("HelpingQuesList")]
        public ActionResult HelpingQuesListPost()
        {
            try
            {
                int id = Convert.ToInt16(Request["ddlCategory"]);
                var Category = obj.GetCategory();
                ViewBag.Clist = new SelectList(Category, "CategoryID", "CategoryDisc", id);
                IEnumerable<HelpingQue> Ques = obj.HelpingQues.Where(s => s.CategoryID == id);
                Session["Cid"] = id;
                ViewBag.ProductID = Session["ProductID"];
                return View(Ques);
            }
            catch (Exception)
            {
                ViewBag.ProductID = Session["ProductID"];
                return RedirectToAction("HelpingQuesList", obj.GetHelpingQues().ToList());
            }
        }

        public ActionResult AddQuestion()
        {
            short QID = Convert.ToInt16(Request["QuestionID"]);
            byte CID = Convert.ToByte(Request["CategoryID"]);
            HelpingQue Ques = obj.HelpingQues.Single(Q => Q.QuestionID == QID && Q.CategoryID == CID);
            var Category = obj.GetCategory();
            ViewBag.Clist = new SelectList(Category, "CategoryID", "CategoryDisc", CID);
            short SID = Convert.ToInt16(Session["SurveyID"]);
            SurveyQue SQues = new SurveyQue();
            SQues.SurveyID = SID;
            Ques.QuestionID = 0;
            if (CID == 1)//Descriptive
            {
                obj.uspSaveReviewQues(SQues.SurveyID, Ques.CategoryID, Ques.QuestionID, Ques.Question);
            }
            else
            {
                obj.uspSaveSurveyQues(SQues.SurveyID, Ques.CategoryID, Ques.QuestionID, Ques.Question, Ques.Answer1, Ques.Answer2, Ques.Answer3, Ques.Answer4, Ques.Answer5, Ques.Answer6, Ques.Answer7);
            }
            ViewBag.ProductID = Session["ProductID"];
            return RedirectToAction("HelpingQuesList", obj.GetHelpingQues().ToList());
        }

        public ActionResult SurveyQuesList()
        {
            if (Session["ClientID"] != null)
            {
                short ClientID = Convert.ToInt16(Session["ClientID"]);
                short PID = Convert.ToInt16(Session["ProductID"]);
                ProductMst Product = obj.ProductMsts.Single(P => P.ClientID == ClientID & P.ProductID == PID);
                ClientMst Client = obj.ClientMsts.Single(C => C.ClientID == ClientID);
                ViewBag.ProductName = Product.ProductName;
                ViewBag.ModelName = Product.ModelName;
                ViewBag.CompName = Client.CompName;
                ViewBag.OwnerName = Client.OwnerName;
                ViewBag.ContactNo = Client.ContactNo;
                ViewBag.EmailID = Client.EmailID;
                ViewBag.CompanyLogo = Client.CompanyLogo;
                return View(obj.GetSurveyQues(ClientID, PID).ToList());
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult DeleteSurveyQues()
        {
            if (Session["ClientID"] != null)
            {
                short SID = Convert.ToInt16(Request["SurveyID"]);
                short QID = Convert.ToInt16(Request["QuestionID"]);
                byte CID = Convert.ToByte(Request["CategoryID"]);
                var Ques = obj.SurveyQues.Single(x => x.SurveyID == SID & x.CategoryID == CID & x.QuestionID == QID);
                if (Ques != null)
                {
                    obj.Entry(Ques).State = EntityState.Deleted;
                    obj.SaveChanges();
                }
                short ClientID = Convert.ToInt16(Session["ClientID"]);
                short ProductID = Convert.ToInt16(Session["ProductID"]);
                return RedirectToAction("SurveyQuesList", obj.GetSurveyQues(ClientID, ProductID).ToList());
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        [HttpGet]
        [ActionName("EditSurveyQues")]
        public ActionResult EditSurveyQuesGet()
        {
            short QID = Convert.ToInt16(Request["QuestionID"]);
            short SID = Convert.ToInt16(Request["SurveyID"]);
            byte CID = Convert.ToByte(Request["CategoryID"]);
            SurveyQue Ques = obj.SurveyQues.Single(Q => Q.SurveyID == SID & Q.CategoryID == CID & Q.QuestionID == QID);
            var Category = obj.GetCategory();
            ViewBag.CList = new SelectList(Category, "CategoryID", "CategoryDisc", CID);
            return View(Ques);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ActionName("EditSurveyQues")]
        public ActionResult EditSurveyQuesPost(ClsSurveyQue Ques)
        {
            if (Session["ClientID"] != null)
            {
                Ques.SurveyID = Convert.ToInt16(Session["SurveyID"]);
                short ClientID = Convert.ToInt16(Session["ClientID"]);
                short ProductID = Convert.ToInt16(Session["ProductID"]);
                if (ModelState.IsValid)
                {
                    if (Ques.CategoryID != 1)//Not Descriptive
                    {
                        obj.uspSaveSurveyQues(Ques.SurveyID, Ques.CategoryID, Ques.QuestionID, Ques.Question, Ques.Answer1, Ques.Answer2, Ques.Answer3, Ques.Answer4, Ques.Answer5, Ques.Answer6, Ques.Answer7);
                        return RedirectToAction("SurveyQuesList", obj.GetSurveyQues(ClientID, ProductID).ToList());
                    }
                    else
                    {
                        var Category = obj.GetCategory();
                        ViewBag.CList = new SelectList(Category, "CategoryID", "CategoryDisc", Ques.CategoryID);
                        Session["CError"] = "1";
                        ViewBag.Error = "You Can't Add Descriptive From This Panal...";
                        return View();
                    }
                }
                else
                {
                    var Category = obj.GetCategory();
                    ViewBag.CList = new SelectList(Category, "CategoryID", "CategoryDisc", Ques.CategoryID);
                    SurveyQue Quesobj = obj.SurveyQues.Single(Q => Q.SurveyID == Ques.SurveyID & Q.CategoryID == Ques.CategoryID & Q.QuestionID == Ques.QuestionID);
                    return View(Quesobj);
                }
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult ReviewQuesList()
        {
            if (Session["ClientID"] != null)
            {
                short ClientID = Convert.ToInt16(Session["ClientID"]);
                short PID = Convert.ToInt16(Session["ProductID"]);
                ProductMst Product = obj.ProductMsts.Single(P => P.ClientID == ClientID & P.ProductID == PID);
                ClientMst Client = obj.ClientMsts.Single(C => C.ClientID == ClientID);
                ViewBag.ProductName = Product.ProductName;
                ViewBag.ModelName = Product.ModelName;
                ViewBag.CompName = Client.CompName;
                ViewBag.OwnerName = Client.OwnerName;
                ViewBag.ContactNo = Client.ContactNo;
                ViewBag.EmailID = Client.EmailID;
                ViewBag.CompanyLogo = Client.CompanyLogo;
                return View(obj.GetReviewQues(ClientID, PID).ToList());
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult DeleteReviewQues()
        {
            if (Session["ClientID"] != null)
            {
                short SID = Convert.ToInt16(Request["SurveyID"]);
                short QID = Convert.ToInt16(Request["QuestionID"]);
                byte CID = Convert.ToByte(Request["CategoryID"]);
                var Ques = obj.ReviewQues.Single(x => x.SurveyID == SID & x.CategoryID == CID & x.QuestionID == QID);
                if (Ques != null)
                {
                    obj.Entry(Ques).State = EntityState.Deleted;
                    obj.SaveChanges();
                }
                short ClientID = Convert.ToInt16(Session["ClientID"]);
                short ProductID = Convert.ToInt16(Session["ProductID"]);
                return RedirectToAction("ReviewQuesList", obj.GetSurveyQues(ClientID, ProductID).ToList());
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        [HttpGet]
        [ActionName("EditReviewQues")]
        public ActionResult EditReviewQuesGet()
        {
            short QID = Convert.ToInt16(Request["QuestionID"]);
            short SID = Convert.ToInt16(Request["SurveyID"]);
            byte CID = Convert.ToByte(Request["CategoryID"]);
            ReviewQue Ques = obj.ReviewQues.Single(Q => Q.SurveyID == SID & Q.CategoryID == CID & Q.QuestionID == QID);
            var Category = obj.GetCategory();
            ViewBag.CList = new SelectList(Category, "CategoryID", "CategoryDisc", CID);
            return View(Ques);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ActionName("EditReviewQues")]
        public ActionResult EditReviewQuesPost(ClsReviewQue Ques)
        {
            if (Session["ClientID"] != null)
            {
                Ques.SurveyID = Convert.ToInt16(Session["SurveyID"]);
                short ClientID = Convert.ToInt16(Session["ClientID"]);
                short ProductID = Convert.ToInt16(Session["ProductID"]);
                if (ModelState.IsValid)
                {
                    if (Ques.CategoryID == 1)//Not Descriptive
                    {
                        obj.uspSaveReviewQues(Ques.SurveyID, Ques.CategoryID, Ques.QuestionID, Ques.Question);
                        return RedirectToAction("ReviewQuesList", obj.GetReviewQues(ClientID, ProductID).ToList());
                    }
                    else
                    {
                        var Category = obj.GetCategory();
                        ViewBag.CList = new SelectList(Category, "CategoryID", "CategoryDisc", Ques.CategoryID);
                        Session["CError"] = "1";
                        ViewBag.Error = "You Can only Add Descriptive From This Panal...";
                        ReviewQue Quesobj = obj.ReviewQues.Single(Q => Q.SurveyID == Ques.SurveyID & Q.CategoryID == Ques.CategoryID & Q.QuestionID == Ques.QuestionID);
                        return View(Quesobj);
                    }
                }
                else
                {
                    var Category = obj.GetCategory();
                    ViewBag.CList = new SelectList(Category, "CategoryID", "CategoryDisc", Ques.CategoryID);
                    ReviewQue Quesobj = obj.ReviewQues.Single(Q => Q.SurveyID == Ques.SurveyID & Q.CategoryID == Ques.CategoryID & Q.QuestionID == Ques.QuestionID);
                    return View();
                }
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult ProductPublishList()
        {
            if (Session["ClientID"] != null)
            {
                short ClientID = Convert.ToInt16(Session["ClientID"]);
                return View(obj.GetSurveyProduct(ClientID));
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult Publish(short SID, bool Status)
        {
            if (Session["ClientID"] != null)
            {
                short ClientID = Convert.ToInt16(Session["ClientID"]);
                bool? Cnt = obj.SavePackageProductCnt(ClientID, Status).SingleOrDefault().Cnt;
                if (Cnt == true)
                {
                    DateTime Date = DateTime.Now;
                    bool? AStatus = obj.SaveApprovedSurveyMst(SID, Date.Date, Status).SingleOrDefault().AStatus;
                    if (AStatus == true)
                    {
                        return RedirectToAction("ProductSuccess");
                    }
                    else
                    {
                        return RedirectToAction("ProductPublishList");
                    }
                }
                else
                {
                    return RedirectToAction("BuyPackage");
                }
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult ProductSuccess()
        {
            return View();
        }

        [HttpGet]
        [ActionName("UpdateProfile")]
        public ActionResult UpdateProfileGet()
        {
            if (Session["ClientID"] != null)
            {
                short ClientID = Convert.ToInt16(Session["ClientID"]);
                ClientMst Client = obj.ClientMsts.Single(C => C.ClientID == ClientID);
                return View(Client);
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        [HttpPost]
        [ActionName("UpdateProfile")]
        public ActionResult UpdateProfilePost(ClsClientMst Client)
        {
            if (Session["ClientID"] != null)
            {
                short ClientID = Convert.ToInt16(Session["ClientID"]);
                Client.ClientID = ClientID;
                if (ModelState.IsValid)
                {
                    try
                    {
                        obj.uspSaveClientMst(Client.ClientID, Client.CompName, Client.CompAddress, Client.OwnerName, Client.CompanyLogo, Client.ContactNo, Client.MobileNo, Client.GSTNo, Client.WebsiteName, Client.EmailID, Client.JoiningDate, Client.IsActive);
                        ClientMst objClient = obj.ClientMsts.Single(C => C.ClientID == ClientID);
                        return View("UpdateProfile", objClient);
                    }
                    catch (Exception e)
                    {
                        var sql = e.InnerException as System.Data.SqlClient.SqlException;
                        if (sql.Number == 2627)
                        {
                            Session["Email"] = "1";
                            ViewBag.Email = "EmailID Already Exist...";
                        }
                        ClientMst Clientobj = obj.ClientMsts.Single(C => C.ClientID == Client.ClientID);
                        return View(Clientobj);
                    }
                }
                else
                {
                    ClientMst Clientobj = obj.ClientMsts.Single(C => C.ClientID == ClientID);
                    return View(Clientobj);
                }
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        [HttpPost]
        public ActionResult UpdateImage(HttpPostedFileBase CompanyLogo)
        {
            if (Session["ClientID"] != null)
            {
                short ClientID = Convert.ToInt16(Session["ClientID"]);
                if (CompanyLogo != null)
                {
                    if ((CompanyLogo.ContentType == "image/png" || CompanyLogo.ContentType == "image/jpeg") && (CompanyLogo.ContentLength <= 4194304))
                    {
                        DateTime date = DateTime.Now;
                        var extension = Path.GetExtension(CompanyLogo.FileName);
                        string fileName = "Client" + date.ToString("yyyyMMddhhmmssfff") + extension;
                        ClientMst Client = new ClientMst();
                        Client.ClientID = ClientID;
                        Client.CompanyLogo = "~/Contents/appimg/Client/" + fileName;
                        CompanyLogo.SaveAs(Path.Combine(Server.MapPath("~/Contents/appimg/Client/"), fileName));
                        obj.uspSaveUpdateImageClient(Client.ClientID, Client.CompanyLogo);
                        ClientMst objClient = obj.ClientMsts.Single(C => C.ClientID == ClientID);
                        Session["ClientImage"] = objClient.CompanyLogo;
                        return View("UpdateProfile", objClient);
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
                        ClientMst Clientobj = obj.ClientMsts.Single(C => C.ClientID == ClientID);
                        return View("UpdateProfile", Clientobj);
                    }
                }
                else
                {
                    ClientMst Clientobj = obj.ClientMsts.Single(C => C.ClientID == ClientID);
                    return View("UpdateProfile", Clientobj);
                }
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        [HttpGet]
        [ActionName("UpdateProduct")]
        public ActionResult UpdateProductGet(short PID, short CID)
        {
            ProductMst Product = obj.ProductMsts.Single(C => C.ClientID == CID & C.ProductID == PID);
            return View(Product);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ActionName("UpdateProduct")]
        public ActionResult UpdateProductPost(ClsProductMst Product)
        {
            if (Session["ClientID"] != null)
            {
                short ClientID = Convert.ToInt16(Session["ClientID"]);
                Product.ClientID = ClientID;
                if (ModelState.IsValid)
                {
                    obj.uspSaveProductMst(Product.ClientID, Product.ProductID, Product.ProductName, Product.ProductCompany, Product.ModelName, Product.ShortDiscription, Product.BriefDiscription, Product.Facility, Product.Price, Product.ProductImage1, Product.ProductImage2, Product.ProductImage3, Product.ProductImage4, Product.Video, Product.TotalSurveyAmt, Product.SurveyPerAmount, Product.NoOfSurvey);
                    return View("Product", obj.GetProduct(ClientID));
                }
                else
                {
                    ProductMst Productobj = obj.ProductMsts.Single(C => C.ClientID == ClientID & C.ProductID == Product.ProductID);
                    return View(Productobj);
                }
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        [HttpPost]
        [ActionName("UpdateProductImage")]
        public ActionResult UpdateProductImage(ClsProductMst Product, HttpPostedFileBase ProductImage1, HttpPostedFileBase ProductImage2, HttpPostedFileBase ProductImage3, HttpPostedFileBase ProductImage4, HttpPostedFileBase Video)
        {
            if (Session["ClientID"] != null)
            {
                short ClientID = Convert.ToInt16(Session["ClientID"]);
                Product.ClientID = ClientID;
                DateTime date = DateTime.Now;
                if (ProductImage1 != null)
                {
                    if ((ProductImage1.ContentType == "image/png" || ProductImage1.ContentType == "image/jpeg") && (ProductImage1.ContentLength <= 4194304))
                    {
                        var extension1 = Path.GetExtension(ProductImage1.FileName);
                        string fileName1 = "IMG1" + date.ToString("yyyyMMddhhmmssfff") + extension1;
                        Product.ProductImage1 = "~/Contents/appimg/Product/" + fileName1;
                        ProductImage1.SaveAs(Path.Combine(Server.MapPath("~/Contents/appimg/Product/"), fileName1));
                    }
                    else
                    {
                        Session["Image"] = "1";
                        if (ProductImage1.ContentLength <= 4194304)
                        {
                            ViewBag.Image1Type = "Product Image Size Must Be lessthen 4MB";
                        }
                        else
                        {
                            ViewBag.Image1Type = "Product Image Must Be in jpg or png Format";
                        }
                    }
                }
                if (ProductImage2 != null)
                {
                    if ((ProductImage2.ContentType == "image/png" || ProductImage2.ContentType == "image/jpeg") && (ProductImage2.ContentLength <= 4194304))
                    {
                        var extension2 = Path.GetExtension(ProductImage2.FileName);
                        string fileName2 = "IMG2" + date.ToString("yyyyMMddhhmmssfff") + extension2;
                        Product.ProductImage2 = "~/Contents/appimg/Product/" + fileName2;
                        ProductImage2.SaveAs(Path.Combine(Server.MapPath("~/Contents/appimg/Product/"), fileName2));
                    }
                    else
                    {
                        Session["Image"] = "1";
                        if (ProductImage2.ContentLength <= 4194304)
                        {
                            ViewBag.Image2Type = "Product Image Size Must Be lessthen 4MB";
                        }
                        else
                        {
                            ViewBag.Image2Type = "Product Image Must Be in jpg or png Format";
                        }
                        ProductMst Productobj = obj.ProductMsts.Single(C => C.ClientID == ClientID & C.ProductID == Product.ProductID);
                        return View("UpdateProduct", Productobj);
                    }
                }
                if (ProductImage3 != null)
                {
                    if ((ProductImage3.ContentType == "image/png" || ProductImage3.ContentType == "image/jpeg") && (ProductImage3.ContentLength <= 4194304))
                    {
                        var extension3 = Path.GetExtension(ProductImage3.FileName);
                        string fileName3 = "IMG3" + date.ToString("yyyyMMddhhmmssfff") + extension3;
                        Product.ProductImage3 = "~/Contents/appimg/Product/" + fileName3;
                        ProductImage3.SaveAs(Path.Combine(Server.MapPath("~/Contents/appimg/Product/"), fileName3));
                    }
                    else
                    {
                        Session["Image"] = "1";
                        if (ProductImage3.ContentLength <= 4194304)
                        {
                            ViewBag.Image3Type = "Product Image Size Must Be lessthen 4MB";
                        }
                        else
                        {
                            ViewBag.Image3Type = "Product Image Must Be in jpg or png Format";
                        }
                        ProductMst Productobj = obj.ProductMsts.Single(C => C.ClientID == ClientID & C.ProductID == Product.ProductID);
                        return View("UpdateProduct", Productobj);
                    }
                }
                if (ProductImage4 != null)
                {
                    if ((ProductImage4.ContentType == "image/png" || ProductImage4.ContentType == "image/jpeg") && (ProductImage4.ContentLength <= 4194304))
                    {
                        var extension4 = Path.GetExtension(ProductImage1.FileName);
                        string fileName4 = "IMG4" + date.ToString("yyyyMMddhhmmssfff") + extension4;
                        Product.ProductImage4 = "~/Contents/appimg/Product/" + fileName4;
                        ProductImage4.SaveAs(Path.Combine(Server.MapPath("~/Contents/appimg/Product/"), fileName4));
                    }
                    else
                    {
                        Session["Image"] = "1";
                        if (ProductImage4.ContentLength <= 4194304)
                        {
                            ViewBag.Image4Type = "Product Image Size Must Be lessthen 4MB";
                        }
                        else
                        {
                            ViewBag.Image4Type = "Product Image Must Be in jpg or png Format";
                        }
                        ProductMst Productobj = obj.ProductMsts.Single(C => C.ClientID == ClientID & C.ProductID == Product.ProductID);
                        return View("UpdateProduct", Productobj);
                    }
                }
                if (Video != null)
                {
                    if (Video.ContentType == "video/mp4" && Video.ContentLength <= 6553600)
                    {
                        var extension = Path.GetExtension(Video.FileName);
                        string fileName = "Video" + date.ToString("yyyyMMddhhmmssfff") + extension;
                        Product.Video = "~/Contents/appimg/Product/Video/" + fileName;
                        Video.SaveAs(Path.Combine(Server.MapPath("~/Contents/appimg/Product/Video/"), fileName));
                    }
                    else
                    {
                        Session["Image"] = "1";
                        if (Video.ContentLength <= 4194304)
                        {
                            ViewBag.VideoType = "Product Video Size Must Be lessthen 4MB";
                        }
                        else
                        {
                            ViewBag.VideoType = "You Can Only Put MP4 Product Video";
                        }
                        ProductMst Productobj = obj.ProductMsts.Single(C => C.ClientID == ClientID & C.ProductID == Product.ProductID);
                        return View("UpdateProduct", Productobj);
                    }
                }

                obj.uspSaveUpdateImageProduct(Product.ClientID, Product.ProductID, Product.ProductImage1, Product.ProductImage2, Product.ProductImage3, Product.ProductImage4, Product.Video);
                return View("Product", obj.GetProduct(ClientID));
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
            if (Session["ClientID"] != null)
            {
                short ClientID = Convert.ToInt16(Session["ClientID"]);
                if (Password != "" && NewPassword != "" && ConfirmPassword != "")
                {
                    if (NewPassword.CompareTo(ConfirmPassword) == 0)
                    {
                        var Status = obj.uspSaveChangePasswdClient(ClientID, Password, NewPassword);
                        if (Convert.ToInt16(Status.SingleOrDefault().Value) == ClientID)
                        {
                            ClientMst Client = obj.ClientMsts.Single(C => C.ClientID == ClientID);
                            try
                            {
                                string EmailID = Client.EmailID;
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
            if (Session["ClientID"] != null)
            {
                Session["Area"] = "Client";
                short ClientID = Convert.ToInt16(Session["ClientID"]);
                return View(obj.GetProductResponse(ClientID));
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult ViewResponse(short SID)
        {
            if (Session["ClientID"] != null)
            {
                Session["Area"] = "Client";
                Session["SurveyID"] = SID;
                ViewBag.Grid = obj.GetResponse(SID).ToList();
                return View();
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
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

        public ActionResult ProductDetails(short PID)
        {
            if (Session["ClientID"] != null)
            {
                short ClientID = Convert.ToInt16(Session["ClientID"]);
                ProductMst Product = obj.ProductMsts.Single(P => P.ClientID == ClientID & P.ProductID == PID);
                Session["SRP"] = "Client";
                Session["Btn"] = "Client";
                return View(Product);
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        [HttpGet]
        [ActionName("FeedbackReport")]
        public ActionResult FeedbackReportGet()
        {
            if (Session["ClientID"] != null)
            {
                short ClientID = Convert.ToInt16(Session["ClientID"]);
                ViewBag.FeedbackGrid = obj.GetFeedbackReport(ClientID, 0);
                return View();
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        [HttpPost]
        [ActionName("FeedbackReport")]
        public ActionResult FeedbackReportPost()
        {
            if (Session["ClientID"] != null)
            {
                byte Star = Convert.ToByte(Request["ddlStar"]);
                short ClientID = Convert.ToInt16(Session["ClientID"]);
                ViewBag.FeedbackGrid = obj.GetFeedbackReport(ClientID, Star);
                return View();
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult SurveyReport()
        {
            if (Session["ClientID"] != null)
            {
                short ClientID = Convert.ToInt16(Session["ClientID"]);
                ViewBag.SurveyGrid = obj.GetSurveyReport(ClientID).ToList();
                return View();
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult SurveyQuesReport(short SID)
        {
            if (Session["ClientID"] != null)
            {
                SurveyMst Survey = obj.SurveyMsts.Single(S => S.SurveyID == SID);
                ViewBag.CompanyLogo = Survey.ClientMst.CompanyLogo;
                ViewBag.OwnerName = Survey.ClientMst.OwnerName;
                ViewBag.CompName = Survey.ClientMst.CompName;
                ViewBag.ProductName = Survey.ProductMst.ProductName;
                ViewBag.ModelName = Survey.ProductMst.ModelName;
                ViewBag.ContactNo = Survey.ClientMst.ContactNo;
                ViewBag.EmailID = Survey.ClientMst.EmailID;
                ViewBag.SurveyQuesGrid = obj.GetSurveyQuesReport(SID).ToList();
                return View();
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult ReviewQuesReport(short SID)
        {
            if (Session["ClientID"] != null)
            {
                SurveyMst Survey = obj.SurveyMsts.Single(S => S.SurveyID == SID);
                ViewBag.CompanyLogo = Survey.ClientMst.CompanyLogo;
                ViewBag.OwnerName = Survey.ClientMst.OwnerName;
                ViewBag.CompName = Survey.ClientMst.CompName;
                ViewBag.ProductName = Survey.ProductMst.ProductName;
                ViewBag.ModelName = Survey.ProductMst.ModelName;
                ViewBag.ContactNo = Survey.ClientMst.ContactNo;
                ViewBag.EmailID = Survey.ClientMst.EmailID;
                ViewBag.ReviewQuesGrid = obj.GetReviewQuesReport(SID).ToList();
                return View();
            }
            else
            {
                return RedirectToAction("SignIn", "Home", new { area = "" });
            }
        }

        public ActionResult SurveyPieChartReport(short SID)
        {
            SurveyMst Survey = obj.SurveyMsts.Single(S => S.SurveyID == SID);
            ViewBag.CompanyLogo = Survey.ClientMst.CompanyLogo;
            ViewBag.OwnerName = Survey.ClientMst.OwnerName;
            ViewBag.CompName = Survey.ClientMst.CompName;
            ViewBag.ProductName = Survey.ProductMst.ProductName;
            ViewBag.ModelName = Survey.ProductMst.ModelName;
            ViewBag.ContactNo = Survey.ClientMst.ContactNo;
            ViewBag.EmailID = Survey.ClientMst.EmailID;
            var Report = obj.SurveyPieChartReport(SID).ToList();
            ViewBag.ApprovedSurvey = Report.SingleOrDefault().ApprovedSurvey;
            ViewBag.NotApprovedSurvey = Report.SingleOrDefault().NotApprovedSurvey;
            return View();
        }

        public ActionResult ReviewQuesGridReport(short QID, short SID)
        {
            SurveyMst Survey = obj.SurveyMsts.Single(S => S.SurveyID == SID);
            ViewBag.CompanyLogo = Survey.ClientMst.CompanyLogo;
            ViewBag.OwnerName = Survey.ClientMst.OwnerName;
            ViewBag.CompName = Survey.ClientMst.CompName;
            ViewBag.ProductName = Survey.ProductMst.ProductName;
            ViewBag.ModelName = Survey.ProductMst.ModelName;
            ViewBag.ContactNo = Survey.ClientMst.ContactNo;
            ViewBag.EmailID = Survey.ClientMst.EmailID;
            ViewBag.SurveyID = SID;
            ReviewQue Ques = obj.ReviewQues.Single(Q => Q.SurveyID == SID & Q.QuestionID == QID);
            ViewBag.Question = Ques.Question;
            ViewBag.ReviewQuesGridReport = obj.GetReviewAnsReport(SID, QID);
            return View();
        }

        public ActionResult SurveyQuesGridReport(short QID, short SID, short CID)
        {
            SurveyMst Survey = obj.SurveyMsts.Single(S => S.SurveyID == SID);
            ViewBag.CompanyLogo = Survey.ClientMst.CompanyLogo;
            ViewBag.OwnerName = Survey.ClientMst.OwnerName;
            ViewBag.CompName = Survey.ClientMst.CompName;
            ViewBag.ProductName = Survey.ProductMst.ProductName;
            ViewBag.ModelName = Survey.ProductMst.ModelName;
            ViewBag.ContactNo = Survey.ClientMst.ContactNo;
            ViewBag.EmailID = Survey.ClientMst.EmailID;
            ViewBag.SurveyID = SID;
            SurveyQue Ques = obj.SurveyQues.Single(Q => Q.SurveyID == SID & Q.CategoryID == CID & Q.QuestionID == QID);
            ViewBag.Question = Ques.Question;
            var Report = obj.GetSurveyAnsReport(SID, QID, CID).ToList();
            ViewBag.SurveyID = Report.SingleOrDefault().SurveyID;
            ViewBag.TotalCount = Report.SingleOrDefault().TotalCount;
            short? CategoryID = Report.SingleOrDefault().CategoryID;
            ViewBag.CategoryID = CategoryID;
            if (CategoryID == 2)
            {
                ViewBag.Answer1 = Ques.Answer1;
                ViewBag.Answer2 = Ques.Answer2;
                ViewBag.Answer3 = Ques.Answer3;
                ViewBag.Answer4 = Ques.Answer4;
                ViewBag.Answer5 = Ques.Answer5;
                ViewBag.Answer6 = Ques.Answer6;
                ViewBag.Answer7 = Ques.Answer7;
            }
            ViewBag.AvgAns1 = Report.SingleOrDefault().AvgAns1;
            ViewBag.AvgAns2 = Report.SingleOrDefault().AvgAns2;
            ViewBag.AvgAns3 = Report.SingleOrDefault().AvgAns3;
            ViewBag.AvgAns4 = Report.SingleOrDefault().AvgAns4;
            ViewBag.AvgAns5 = Report.SingleOrDefault().AvgAns5;
            ViewBag.AvgAns6 = Report.SingleOrDefault().AvgAns6;
            ViewBag.AvgAns7 = Report.SingleOrDefault().AvgAns7;
            ViewBag.CntAns1 = Report.SingleOrDefault().CntAns1;
            ViewBag.CntAns2 = Report.SingleOrDefault().CntAns2;
            ViewBag.CntAns3 = Report.SingleOrDefault().CntAns3;
            ViewBag.CntAns4 = Report.SingleOrDefault().CntAns4;
            ViewBag.CntAns5 = Report.SingleOrDefault().CntAns5;
            ViewBag.CntAns6 = Report.SingleOrDefault().CntAns6;
            ViewBag.CntAns7 = Report.SingleOrDefault().CntAns7;
            return View();
        }

        public ActionResult SurveyQuesPieChartReport(short QID, short SID, short CID)
        {
            SurveyMst Survey = obj.SurveyMsts.Single(S => S.SurveyID == SID);
            ViewBag.CompanyLogo = Survey.ClientMst.CompanyLogo;
            ViewBag.OwnerName = Survey.ClientMst.OwnerName;
            ViewBag.CompName = Survey.ClientMst.CompName;
            ViewBag.ProductName = Survey.ProductMst.ProductName;
            ViewBag.ModelName = Survey.ProductMst.ModelName;
            ViewBag.ContactNo = Survey.ClientMst.ContactNo;
            ViewBag.EmailID = Survey.ClientMst.EmailID;
            ViewBag.SurveyID = SID;
            SurveyQue Ques = obj.SurveyQues.Single(Q => Q.SurveyID == SID & Q.CategoryID == CID & Q.QuestionID == QID);
            var Report = obj.GetSurveyAnsReport(SID, QID, CID).ToList();
            ViewBag.SurveyID = Report.SingleOrDefault().SurveyID;
            short? CategoryID = Report.SingleOrDefault().CategoryID;
            if (CategoryID == 2)
            {
                ViewBag.Answer1 = Ques.Answer1;
                ViewBag.Answer2 = Ques.Answer2;
                if (Ques.Answer3 != null)
                {
                    ViewBag.Answer3 = Ques.Answer3;
                }
                else
                {
                    ViewBag.Answer3 = "Option C";
                }
                if (Ques.Answer4 != null)
                {
                    ViewBag.Answer4 = Ques.Answer4;
                }
                else
                {
                    ViewBag.Answer4 = "Option D";
                }
                if (Ques.Answer5 != null)
                {
                    ViewBag.Answer5 = Ques.Answer5;
                }
                else
                {
                    ViewBag.Answer5 = "Option E";
                }
                if (Ques.Answer6 != null)
                {
                    ViewBag.Answer6 = Ques.Answer6;
                }
                else
                {
                    ViewBag.Answer6 = "Option F";
                }
                if (Ques.Answer7 != null)
                {
                    ViewBag.Answer7 = Ques.Answer7;
                }
                else
                {
                    ViewBag.Answer7 = "Option G";
                }
            }
            else if (CategoryID == 4)
            {
                ViewBag.Answer1 = "1 Star";
                ViewBag.Answer2 = "2 Star";
                ViewBag.Answer3 = "3 Star";
                ViewBag.Answer4 = "4 Star";
                ViewBag.Answer5 = "5 Star";
            }
            else
            {
                ViewBag.Answer1 = "Like";
                ViewBag.Answer2 = "Dislike";
            }
            ViewBag.CntAns1 = Report.SingleOrDefault().CntAns1;
            ViewBag.CntAns2 = Report.SingleOrDefault().CntAns2;
            ViewBag.CntAns3 = Report.SingleOrDefault().CntAns3;
            ViewBag.CntAns4 = Report.SingleOrDefault().CntAns4;
            ViewBag.CntAns5 = Report.SingleOrDefault().CntAns5;
            ViewBag.CntAns6 = Report.SingleOrDefault().CntAns6;
            ViewBag.CntAns7 = Report.SingleOrDefault().CntAns7;
            return View();
        }
    }
}
