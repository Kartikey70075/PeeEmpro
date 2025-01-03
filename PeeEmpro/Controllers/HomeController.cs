using Golden_Terry_Towels.Models;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using SAPbobsCOM;
using System.Web.Mvc;
using System.IO;
using System.Globalization;
using CRM_Sahib.Models;
using Newtonsoft.Json.Linq;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Configuration;
using OfficeOpenXml;
using System.Data.SqlClient;
using CrystalDecisions;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using PeeEmpro.Controllers;
using OfficeOpenXml.Export.HtmlExport.StyleCollectors.StyleContracts;
using System.Net.Mail;
using System.Web.Configuration;
using System.Net;
using static Golden_Terry_Towels.Controllers.LoginController;
using System.Web.WebSockets;


//using System.Web.Http;

namespace Golden_Terry_Towels.Controllers
{
    public class HomeController : Controller
    {
        int ErrorCode = 0;
        string msg = "";
        HanaConnection con;
        HanaSQL Sqlhana;
        string _errorMessage = "";
        int HeaderrecordsAffected = 0;
        HanaSQL Sqlhana2 = new HanaSQL();
        HanaConnection con2 = new HanaConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Hana"].ConnectionString);
        SapConnection db = new SapConnection();
        Company _company;
        SAPbobsCOM.Items Items = null;
        SAPbobsCOM.ProductTrees productTree = null;
        SAPbobsCOM.Documents Documents = null;
        private dynamic doc;
        public ActionResult Index()
        {
            if (Session.Count != 0)
            {
                return View();
            }
            else
            {

                return RedirectToAction("login", "login");
            }
        }
        public ActionResult Contact()
        {
            if (Session.Count != 0)
            {
                return View();
            }
            else
            {

                return RedirectToAction("login", "login");
            }
        }
        public ActionResult XLProcess()
        {
            return View();
        }
        public ActionResult RepDownLoad()
        {
            if (Session.Count != 0)
            {
                return View();
            }
            else
            {

                return RedirectToAction("login", "login");
            }
        }
        [HttpPost]
        public ActionResult XLProcess(HttpPostedFileBase file)
        {
            // Retrieve the action button name that was clicked
            string action = Request.Form["action"];
            string _path = string.Empty;
            // Check if a file was uploaded
            if (file != null && file.ContentLength > 0)
            {
                string FileUploadPath = ConfigurationManager.AppSettings["Path"]; // Ensure this path is correctly set in web.config
                string _FileName = Path.GetFileName(file.FileName);
                _path = Path.Combine(FileUploadPath, _FileName);

                // Save the file
                file.SaveAs(_path);
                TempData["Path"] = _path;
                Session["Path"] = _path;

                // Perform actions based on which button was clicked
                if (action == "Download")
                {
                    // Ensure DownloadFile is implemented and handles file download logic
                    //DownloadExcelFile(FileUploadPath, _FileName);
                    return File(_path, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", _FileName);

                }
                else if (action == "Upload")
                {
                    // Ensure PunchData is implemented and processes the file as needed
                    PunchData(_path);
                    ViewBag.Message = action == "Download" ? "File Downloaded Successfully!!" : "File Uploaded Successfully!!";
                }

                // Set a message based on the action

            }
            else
            {
                // Set a message if no file was uploaded
                ViewBag.Message = "Please Upload File!!";
                TempData["Path"] = "empty";
            }

            // Return the view with the appropriate message
            return View();
        }
        [System.Web.Http.HttpGet]
        public JsonResult PunchData(string _path)
        {
            try
            {
                string UserId = Session["Userid"].ToString();
                string UserName = Session["Name"].ToString();
                string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];
                string Query = "select top 1 \"DocEntry\" from" + "\"" + SCHEMA + "\"" + ".\"@COSTING_ATTACHMENT\" order by \"DocEntry\" desc ";
                DataTable DT = new DataTable();
                int docentry = 0;
                DT = Sqlhana2.GetHanaDataSQL(Query);
                if (DT.Rows.Count > 0)
                {
                    docentry = Convert.ToInt32(DT.Rows[0]["DocEntry"]);
                }
                if (UserName == "CRR1")
                {
                    con2.Open();
                    string updqury = "UPDATE " + "\"" + SCHEMA + "\"" + ".\"@COSTING_ATTACHMENT\" " + "SET" + "\"U_AM\" ='" + _path + "' , \"U_UN\"='" + UserName + "' where \"DocEntry\"='" + docentry + "' ";
                    HanaCommand insertHeader = new HanaCommand(updqury, con2);
                    HeaderrecordsAffected = insertHeader.ExecuteNonQuery();
                    con2.Close();
                }

                if (UserName == "PPC")
                {
                    con2.Open();
                    string updqury1 = "UPDATE " + "\"" + SCHEMA + "\"" + ".\"@COSTING_ATTACHMENT\" " + "SET" + "\"U_APPC\" ='" + _path + "' , \"U_UN\"='" + UserName + "' where \"DocEntry\"='" + docentry + "' ";
                    HanaCommand insertHeader1 = new HanaCommand(updqury1, con2);
                    HeaderrecordsAffected = insertHeader1.ExecuteNonQuery();
                    con2.Close();
                }

                return Json("Data Saved Sucesfully", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(ex);
            }

        }
        [System.Web.Http.HttpPost]
        public JsonResult insert(Alltowelinfo1 obj)
        {
            try
            {
                UDOPunch O = new UDOPunch();
                var info = O.INSERT(obj);
                if (info.Data != null)
                {
                    string msg = info.Data.ToString();
                    if (msg.Contains("Error:"))
                    {
                        var error = new { Message = info.Data };
                        return Json(error, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(info, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var error = new { Message = "Error: Somethings Went Wrong !!" };
                    return Json(error, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var error = new { Message = "Error: " + ex.Message };
                return Json(error, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult FindCustomer(string SessiouserDep)
        {
            try
            {
                string comand = string.Empty;
                string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];
                if (GlobalVariables.UserBranch == "Audit")
                {
                    comand = "SELECT \"DocEntry\" AS \"docentry\" ,\"U_CustomerName\" AS \"Customername\" ,\"U_DateofCosting\" AS \"CostingDate\",\"U_Merchandiser\" AS \"merchandiser\" ,\"U_Costing_Num\" AS \"CostingNum\",\"U_WeavingType\" AS \"Weaving_VAL\" ,\"U_DyedType\" AS \"DYED_VAL\",\"U_Status\" AS \"Status\",\"U_Customer_reference_no\" AS \"CRefNo\",\"U_SN\" AS \"SaleNum2\" from " + "\"" + SCHEMA + "\"" + ". \"@COSTING_NEW\" where \"U_Merch_Status\"='Completed' order by \"DocEntry\" ";
                }
                else if (GlobalVariables.UserBranch == "Merchant")
                {
                    comand = "SELECT \"DocEntry\" AS \"docentry\" ,\"U_CustomerName\" AS \"Customername\" ,\"U_DateofCosting\" AS \"CostingDate\",\"U_Merchandiser\" AS \"merchandiser\" ,\"U_Costing_Num\" AS \"CostingNum\",\"U_WeavingType\" AS \"Weaving_VAL\" ,\"U_DyedType\" AS \"DYED_VAL\",\"U_Status\" AS \"Status\",\"U_Customer_reference_no\" AS \"CRefNo\",\"U_SN\" AS \"SaleNum2\" from " + "\"" + SCHEMA + "\"" + ". \"@COSTING_NEW\" where \"U_CostingCreatedBy\"='" + GlobalVariables.userid + "' order by \"DocEntry\" ";
                }
                else
                {
                    comand = "SELECT \"DocEntry\" AS \"docentry\" ,\"U_CustomerName\" AS \"Customername\" ,\"U_DateofCosting\" AS \"CostingDate\",\"U_Merchandiser\" AS \"merchandiser\" ,\"U_Costing_Num\" AS \"CostingNum\",\"U_WeavingType\" AS \"Weaving_VAL\" ,\"U_DyedType\" AS \"DYED_VAL\",\"U_Status\" AS \"Status\",\"U_Customer_reference_no\" AS \"CRefNo\",\"U_SN\" AS \"SaleNum2\" from " + "\"" + SCHEMA + "\"" + ". \"@COSTING_NEW\" order by \"DocEntry\" ";
                }

                DataTable DT10 = Sqlhana2.GetHanaDataSQL(comand);
                List<Alltowelinfo1> abc = new List<Alltowelinfo1>();
                abc = HanaSQL.ConvertDataTable<Alltowelinfo1>(DT10);
                var list = new { abc };
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }

        public JsonResult FindApprovedCosting(string Status)
        {
            try
            {
                string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];
                string comand = "SELECT \"DocEntry\" AS \"docentry\" ,\"U_CustomerName\" AS \"Customername\" ,\"U_DateofCosting\" AS \"CostingDate\",\"U_Merchandiser\" AS \"merchandiser\" ,\"U_Costing_Num\" AS \"CostingNum\",\"U_WeavingType\" AS \"Weaving_VAL\" ,\"U_DyedType\" AS \"DYED_VAL\",\"U_Status\" AS \"Status\",\"U_Customer_reference_no\" AS \"CRefNo\",\"U_SN\" AS \"SaleNum2\" from " + "\"" + SCHEMA + "\"" + ". \"@COSTING_NEW\" where \"U_Status\"='" + Status + "' order by \"DocEntry\" ";
                DataTable DT10 = Sqlhana2.GetHanaDataSQL(comand);
                List<Alltowelinfo1> abc = new List<Alltowelinfo1>();
                abc = HanaSQL.ConvertDataTable<Alltowelinfo1>(DT10);
                var list = new { abc };
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }
        public JsonResult EditAll(int docentry)
        {
            try
            {
                UDOPunch O = new UDOPunch();
                return O.EditAll(docentry);
            }
            catch (Exception ex)
            {
                var error = new { Message = "Error: " + ex.Message };
                return Json(error, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult UpdateAll(Alltowelinfo2 obj)
        {
            try
            {
                UDOPunch O = new UDOPunch();
                var info = O.UpdateAll(obj);
                if (info.Data != null)
                {
                    string msg = info.Data.ToString();
                    if (msg.Contains("Error:"))
                    {
                        var error = new { Message = info.Data };
                        return Json(error, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(info, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var error = new { Message = "Error: Somethings Went Wrong !!" };
                    return Json(error, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var error = new { Message = "Error: " + ex.Message };
                return Json(error, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult CostingNum_Validation()
        {
            string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];
            con2.Open();
            string comand9 = "SELECT \"U_Costing_Num\" AS \"CostingNum\" from " + "\"" + SCHEMA + "\"" + ". \"@COSTING\"";
            DataTable DT9 = Sqlhana2.GetHanaDataSQL(comand9);
            List<Alltowelinfo1> CostingNum = new List<Alltowelinfo1>();
            CostingNum = HanaSQL.ConvertDataTable<Alltowelinfo1>(DT9);
            con2.Close();
            var list = new { CostingNum };
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Material_Name()
        {
            string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];
            con2.Open();
            string command10 = "select \"Name\" AS \"Material_name\" from " + "\"" + SCHEMA + "\"" + ".\"@SLAB_MATERIAL_NAME\" ORDER BY TO_INTEGER(\"Code\")  ";
            DataTable DT10 = Sqlhana2.GetHanaDataSQL(command10);
            List<Material_slab> Material_name = new List<Material_slab>();
            Material_name = HanaSQL.ConvertDataTable<Material_slab>(DT10);
            con2.Close();
            var list = new { Material_name };
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Current_Exchange_Rate(string a)
        {
            string d = DateTime.Now.ToString("yyyyMMdd");
            //string d = "20240303";
            List<Current_Exchange_Rate> C_Rate_val = new List<Current_Exchange_Rate>();
            if (a == "INR")
            {
                C_Rate_val.Add(new Current_Exchange_Rate { Rate = "1" });
                //C_Rate_val[0].Rate = "1";
                return Json(C_Rate_val, JsonRequestBehavior.AllowGet);
            }
            string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];
            string comand = "SELECT \"Rate\" AS \"Rate\"  from  " + "\"" + SCHEMA + "\"" + ". \"ORTT\" where \"RateDate\" ='" + d + "' and \"Currency\" ='" + a + "'";
            DataTable DT10 = Sqlhana2.GetHanaDataSQL(comand);
            C_Rate_val = HanaSQL.ConvertDataTable<Current_Exchange_Rate>(DT10);
            return Json(C_Rate_val, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Administrator()
        {
            if (Session.Count > 0)
                return View();
            else
                return RedirectToAction("Login", "Login");
        }
        public ActionResult Costing_List()
        {
            if (Session.Count != 0)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login", "login");
            }
        }
        public JsonResult Costing_List2()
        {
            try
            {
                string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];
                //string comand = "SELECT \"DocEntry\" AS \"docentry\" ,\"U_CustomerName\" AS \"Customername\",\"U_Code\" AS \"CustomerCode\" ,\"U_DateofCosting\" AS \"CostingDate\",\"U_Merchandiser\" AS \"merchandiser\",\"U_Costing_Num\" AS \"CostingNum\",\"U_SN\" AS \"SaleNum2\" from " + "\"" + SCHEMA + "\"" + ". \"@COSTING_NEW\" where \"U_Status\" ='" + "Pending" + "' and \"U_Code\"!= ''";

                string comand = "SELECT   DISTINCT  \r\n    t0.\"DocEntry\" AS \"docentry\",\r\n    t0.\"U_CustomerName\" AS \"Customername\",\r\n    t0.\"U_Code\" AS \"CustomerCode\",\r\n    t0.\"U_DateofCosting\" AS \"CostingDate\",\r\n    t0.\"U_Merchandiser\" AS \"merchandiser\",\r\n    t0.\"U_Costing_Num\" AS \"CostingNum\",\r\n    t0.\"U_SN\" AS \"SaleNum2\"\r\nfrom " + "\"" + SCHEMA + "\"" + ".\"@COSTING_NEW\" t0\r\nJOIN \r\n    " + "\"" + SCHEMA + "\"" + ".\"@AUDIT\" t1 \r\n    ON t0.\"DocEntry\" = t1.\"DocEntry\"\r\nWHERE \r\n    t0.\"U_Status\" = 'Pending'\r\n    AND t1.\"U_AUDITOR_NAME\" = 'DESIGNING' \r\n    AND t1.\"U_AUDITOR_ACTION\" = 'Approve' \r\nOrder By \r\nCAST(t0.\"DocEntry\" AS INT) DESC";
                DataTable DT10 = Sqlhana2.GetHanaDataSQL(comand);
                List<Alltowelinfo1> abc = new List<Alltowelinfo1>();
                abc = HanaSQL.ConvertDataTable<Alltowelinfo1>(DT10);
                var list = new { abc };
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public string Costing_List_Reject(string DocEntry)
        {
            try
            {

                string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];

                con2.Open();
                string updqury = "UPDATE " + "\"" + SCHEMA + "\"" + ".\"@COSTING_NEW\" " + "SET" + "\"U_Status\"='" + "Reject" + "'  where \"DocEntry\"='" + DocEntry + "' ";
                HanaCommand insertHeader = new HanaCommand(updqury, con2);
                HeaderrecordsAffected = insertHeader.ExecuteNonQuery();
                con2.Close();
                return "YES";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public ActionResult Administrator2()
        {
            try
            {
                string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];
                string comand = "SELECT \"DocEntry\" AS \"DocEntry\" ,\"U_CAD_NO\" AS \"CAD_NO\",\"U_SAM_FOR_BUYER\" AS \"SAM_FOR_BUYER\" ,\"U_DATE\" AS \"DATE\",\"U_USERFROM\" AS \"UserNameFrom\",\"U_SAM_MERC\" AS \"SAM_MERC\"  from " + "\"" + SCHEMA + "\"" + ". \"@SAMPLING_H\" Where \"U_Approve\"='" + "N" + "' and  \"Canceled\"='" + "N" + "'";
                //string comand = "SELECT * from "\"" + SCHEMA + "\"" + ". \"@COSTING\"";
                DataTable DT10 = Sqlhana2.GetHanaDataSQL(comand);
                List<SAMPLING> abcd = new List<SAMPLING>();
                //Convert.ToInt32(abc.docentry);

                abcd = HanaSQL.ConvertDataTable<SAMPLING>(DT10);

                foreach (var item in abcd)
                {
                    if (!string.IsNullOrEmpty(item.DATE))
                    {
                        item.DATE = Convert.ToDateTime(item.DATE).ToString("dd MMM yyy");
                    }

                }

                var list = new { abcd };
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string Administrator2_Approv(string DocEntry)
        {
            try
            {
                string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];

                con2.Open();
                string updqury = "UPDATE " + "\"" + SCHEMA + "\"" + ".\"@SAMPLING_H\" " + "SET" + "\"U_Approve\"='" + "Y" + "'  where \"DocEntry\"='" + DocEntry + "' ";
                HanaCommand insertHeader = new HanaCommand(updqury, con2);
                HeaderrecordsAffected = insertHeader.ExecuteNonQuery();
                con2.Close();
                return "YES";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public string Administrator2_Reject(string DocEntry, string remarks)
        {
            try
            {

                string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];

                con2.Open();
                string updqury = "UPDATE " + "\"" + SCHEMA + "\"" + ".\"@SAMPLING_H\" " + "SET" + "\"U_Approve\"='" + "R" + "' , \"Remark\"='" + remarks + "'  where \"DocEntry\"='" + DocEntry + "' ";
                HanaCommand insertHeader = new HanaCommand(updqury, con2);
                HeaderrecordsAffected = insertHeader.ExecuteNonQuery();
                con2.Close();
                return "YES";
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public JsonResult Administrator2_View(string DocEntry)
        {
            try
            {

                string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];
                string comand = "SELECT \"DocEntry\" AS \"Sm_Doc\" ,\"Remark\" AS \"Sm_Remarks\",\"U_THEME_ART_WORK\" AS \"Sm_Art\" ,\"U_CAD_NO\" AS \"Sm_CAD_DEV_By\",\"U_SAM_MERC\" AS \"Sm_Marchent_Name\",\"U_SAM_TECH\" AS \"Sm_Sample_tech\",\"U_PRODUCT\" AS \"Sm_Product_pr\" ,\"U_USERFROM\" AS \"Sm_User_Name\",\"U_DATE\" AS \"Sm_Date\",\"U_Qualityrefer\" AS \"Sm_Quality_Refrance\" ,\"U_Protocol\" AS \"Sm_Protocol\" ,\"U_SAM_FOR_BUYER\" AS \"Sm_Samplte_For\" ,\"CreateDate\" AS \"Sm_Create_Date\" ,\"UpdateDate\" AS \"Sm_UpdateDate\" ,\"Creator\" AS \"Sm_Creator\" ,\"U_SAM_BASE\" AS \"Sm_SAMPLE_BASED_ON\" ,\"U_CAD_APP_BY\" AS \"Sm_CARD_APPROVED_BY\" ,\"U_CAD_NO\" AS \"Sm_CARD_NO\" ,\"U_SEASON\" AS \"Sm_SEASON\" ,\"U_OPPURTUNITY\" AS \"Sm_OPPURTUNITY\" ,\"U_USERTO\" AS \"Sm_USERNAME_TO\" ,\"U_Qualitytype\" AS \"Sm_Quality_Type\" ,\"U_Markettype\" AS \"Sm_Market_Type\",\"U_Approve\" AS \"Sm_Aproved\"  from " + "\"" + SCHEMA + "\"" + ". \"@SAMPLING_H\" Where \"DocEntry\"='" + DocEntry + "' ";
                DataTable DT10 = Sqlhana2.GetHanaDataSQL(comand);
                List<Sample> Samplte_view = new List<Sample>();
                Samplte_view = HanaSQL.ConvertDataTable<Sample>(DT10);
                foreach (var item in Samplte_view)
                {
                    if (!string.IsNullOrEmpty(item.Sm_Date))
                    {
                        item.Sm_Date = Convert.ToDateTime(item.Sm_Date).ToString("dd MMM yyy");
                        item.Sm_Create_Date = Convert.ToDateTime(item.Sm_Create_Date).ToString("dd MMM yyy");
                        item.Sm_UpdateDate = Convert.ToDateTime(item.Sm_UpdateDate).ToString("dd MMM yyy");
                    }
                }
                string comand_R = "SELECT * from " + "\"" + SCHEMA + "\"" + ". \"@SAMPLING_R\" Where \"DocEntry\"='" + DocEntry + "' ";
                DataTable DTR = Sqlhana2.GetHanaDataSQL(comand_R);
                List<Semple_R> Samplte_R = new List<Semple_R>();
                Samplte_R = HanaSQL.ConvertDataTable<Semple_R>(DTR);
                foreach (var item in Samplte_R)
                {
                    if (!string.IsNullOrEmpty(item.U_APP_DATE))
                        item.U_APP_DATE = Convert.ToDateTime(item.U_APP_DATE).ToString("dd MMM yyy");

                }

                var list = new { Samplte_view, Samplte_R };
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        public ActionResult DownloadFile(string filePath, string fileName)
        {
            // Construct the absolute path to the file
            var fullFilePath = Path.Combine(filePath, fileName);

            // Check if the file exists
            if (!System.IO.File.Exists(fullFilePath))
            {
                // File does not exist, so create a new file with dummy content
                // You can modify this part to generate or copy the appropriate content
                CreateDummyFile(fullFilePath);
            }

            // Determine the content type based on the file extension
            string contentType;
            switch (Path.GetExtension(fileName).ToLower())
            {
                case ".pdf":
                    contentType = "application/pdf";
                    break;
                case ".xlsx":
                case ".xls":
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case ".jpg":
                case ".jpeg":
                    contentType = "image/jpeg";
                    break;
                case ".png":
                    contentType = "image/png";
                    break;
                case ".gif":
                    contentType = "image/gif";
                    break;
                default:
                    // If the file type is not supported, return a 404 Not Found status
                    return HttpNotFound();
            }

            // Return the file for download with the appropriate content t ype
            return File(fullFilePath, contentType, fileName);
        }

        private void CreateDummyFile(string fullFilePath)
        {
            // Create the directory if it does not exist
            var directory = Path.GetDirectoryName(fullFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a dummy file with example content
            var dummyContent = "This is a dummy file content.";
            System.IO.File.WriteAllText(fullFilePath, dummyContent);
        }

        [HttpGet]
        public string CheckFolder(string attachedFile)
        {
            try
            {
                string folderPath = string.Empty;
                if (GlobalVariables.UserDep == "DESIGNING")
                {
                    folderPath = @"E:\Costing_Data\DESIGNING";
                    string fileName = attachedFile;
                    string filePath = Path.Combine(folderPath, fileName);
                    if (System.IO.File.Exists(filePath))
                        return folderPath;
                    else
                        return "N";
                }
                else if (GlobalVariables.UserDep == "Dyeing")
                {
                    folderPath = @"E:\Costing_Data\DYEING";
                    string fileName = attachedFile;
                    string filePath = Path.Combine(folderPath, fileName);
                    if (System.IO.File.Exists(filePath))
                        return folderPath;
                    else
                        return "N";
                }else if (GlobalVariables.UserDep == "Merchant")
                {
                    folderPath = @"E:\Costing_Data\MERCHANT";
                    string fileName = attachedFile;
                    string filePath = Path.Combine(folderPath, fileName);
                    if (System.IO.File.Exists(filePath))
                        return folderPath;
                    else
                        return "N";
                }else if (GlobalVariables.UserDep == "Admin")
                {
                    folderPath = @"E:\Costing_Data\ADMIN";
                    string fileName = attachedFile;
                    string filePath = Path.Combine(folderPath, fileName);
                    if (System.IO.File.Exists(filePath))
                        return folderPath;
                    else
                        return "N";
                }else if (GlobalVariables.UserDep == "PPC")
                {
                    folderPath = @"E:\Costing_Data\PPC";
                    string fileName = attachedFile;
                    string filePath = Path.Combine(folderPath, fileName);
                    if (System.IO.File.Exists(filePath))
                        return folderPath;
                    else
                        return "N";
                }else if (GlobalVariables.UserDep == "Purchase")
                {
                    folderPath = @"E:\Costing_Data\PURCHASE";
                    string fileName = attachedFile;
                    string filePath = Path.Combine(folderPath, fileName);
                    if (System.IO.File.Exists(filePath))
                        return folderPath;
                    else
                        return "N";
                }else
                {
                    folderPath = @"E:\Costing_Data\OTHERS";
                    string fileName = attachedFile;
                    string filePath = Path.Combine(folderPath, fileName);
                    if (System.IO.File.Exists(filePath))
                        return folderPath;
                    else
                        return "N";
                }
            }
            catch (Exception ex)
            {
                return ex.Message; // Return the exception message
            }
        }

        public ActionResult Sale_Order()
        {
            if (Session.Count != 0)
            {
                return View();
            }
            else
            {

                return RedirectToAction("login", "login");
            }
        }

        public JsonResult Bill_to_Ship_to(Terry_Towels obj)
        {
            try
            {
                string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];
                string command = "SELECT T1.\"Address\" AS \"Address\" from " + "\"" + SCHEMA + "\"" + ".\"OCRD\" T0 INNER JOIN " + "\"" + SCHEMA + "\"" + ".\"CRD1\"  T1 ON T0.\"CardCode\" = T1.\"CardCode\" WHERE T1.\"CardCode\" = '" + obj.Code + "' and T1.\"AdresType\" = '" + 'S' + "'";
                DataTable DT = Sqlhana2.GetHanaDataSQL(command);
                List<B_to_S_to> OCRD = new List<B_to_S_to>();
                OCRD = HanaSQL.ConvertDataTable<B_to_S_to>(DT);

                string command2 = "SELECT T1.\"Address\" AS \"Address2\",T1.\"State\" from " + "\"" + SCHEMA + "\"" + ".\"OCRD\" T0 INNER JOIN " + "\"" + SCHEMA + "\"" + ".\"CRD1\"  T1 ON T0.\"CardCode\" = T1.\"CardCode\" WHERE T1.\"CardCode\" = '" + obj.Code + "' and T1.\"AdresType\" = '" + 'B' + "'";
                DataTable DT2 = Sqlhana2.GetHanaDataSQL(command2);
                List<B_to_S_to> OCRD2 = new List<B_to_S_to>();
                OCRD2 = HanaSQL.ConvertDataTable<B_to_S_to>(DT2);

                //string command3 = "SELECT \"U_Costing_Num\" AS \"Costing_Num\",\"DocEntry\" AS \"DocEntry\" from " + "\"" + SCHEMA + "\"" + ". \"@COSTING\" WHERE \"U_Code\" = '" + obj.Code + "' and \"U_Status\" = 'Approved' ";
                //DataTable DT3 = Sqlhana2.GetHanaDataSQL(command3);
                //List<B_to_S_to> OCRD3 = new List<B_to_S_to>();
                //OCRD3 = HanaSQL.ConvertDataTable<B_to_S_to>(DT3);

                var list = new { OCRD, OCRD2 };
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult Costing_dtl(string Costing_num, string created_costing_DocEntry)
        {
            int FGGrpCode = Convert.ToInt32(WebConfigurationManager.AppSettings["FGGrpCode"]);
            string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];
            string command = "SELECT \"ItemCode\" AS \"ItemCode\",\"ItemName\" AS \"ItemName\",\"InvntryUom\" AS \"UOM\" from " + "\"" + SCHEMA + "\"" + ". \"OITM\"  WHERE \"U_Costing_No\"='" + Costing_num + "' and \"ItmsGrpCod\"='" + FGGrpCode + "'";
            DataTable DT = Sqlhana2.GetHanaDataSQL(command);
            List<ItemDlt> Itm = new List<ItemDlt>();
            Itm = HanaSQL.ConvertDataTable<ItemDlt>(DT);

            string command2 = "SELECT \r\n    T0.\"U_Costing_Num\" AS \"CostingNum\",\r\n    T0.\"DocEntry\" AS \"DocEntry\",\r\n    T0.\"U_merchandiserCode\", \r\n    T2.\"U_ExchangeRate\" AS \"U_Exchangerate\", \r\n    T2.\"U_ExchangeCode\" AS \"U_ExchangeCode\", \r\n    T1.\"U_OrderQty\" AS \"OrderQty\",\r\n    T1.\"U_SalePricePerPc\" AS \"Priceperpc\", \r\n    T0.\"U_SN\" AS \"SaleOrd_No\", \r\n    T0.\"U_Customer_reference_no\" AS \"CRN\", \r\n    TotalOrderQty.\"TotalOrderQty\",\r\n    Totalkg.\"Totalkg\"from " + "\"" + SCHEMA + "\"" + ".\"@COSTING_NEW\" T0\r\nINNER JOIN \r\n    " + "\"" + SCHEMA + "\"" + ".\"@TOWEL_DETAILS\" T1 ON T0.\"DocEntry\" = T1.\"DocEntry\"\r\nINNER JOIN \r\n    " + "\"" + SCHEMA + "\"" + ".\"@GENERAL_INPUTS\" T2 ON T0.\"DocEntry\" = T2.\"DocEntry\"\r\nCROSS JOIN \r\n    (SELECT \r\n        SUM(T1.\"U_OrderQty\") AS \"TotalOrderQty\"\r\n    from " + "\"" + SCHEMA + "\"" + ".\"@COSTING_NEW\" T0\r\n    INNER JOIN \r\n    " + "\"" + SCHEMA + "\"" + ".\"@TOWEL_DETAILS\" T1 ON T0.\"DocEntry\" = T1.\"DocEntry\"\r\n    WHERE \r\n        T0.\"DocEntry\" = '" + created_costing_DocEntry + "') AS TotalOrderQty\r\nCROSS JOIN \r\n    (SELECT \r\n        SUM(T1.\"U_OrderTotalKg\") AS \"Totalkg\" -- Subquery to sum \"U_OrderTotalKg\"\r\n    from " + "\"" + SCHEMA + "\"" + ".\"@COSTING_NEW\" T0\r\n    INNER JOIN \r\n    " + "\"" + SCHEMA + "\"" + ".\"@TOWEL_DETAILS\" T1 ON T0.\"DocEntry\" = T1.\"DocEntry\"\r\n    WHERE \r\n        T0.\"DocEntry\" = '" + created_costing_DocEntry + "') AS Totalkg\r\nWHERE \r\n    T0.\"DocEntry\" = '" + created_costing_DocEntry + "'\r\n    AND T1.\"LineId\" = '1'";
            DataTable DT2 = Sqlhana2.GetHanaDataSQL(command2);
            List<ItemDlt> Itm2 = new List<ItemDlt>();
            Itm2 = HanaSQL.ConvertDataTable<ItemDlt>(DT2);
            var ls = new { Itm, Itm2 };
            return Json(ls, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaleOrderInit()
        {
            try
            {
                string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];
                string command = "SELECT \"CardName\" AS \"CustomerName\",\"CardCode\" AS \"CustomerCode\" from " + "\"" + SCHEMA + "\"" + ". \"OCRD\" WHERE \"CardType\" = 'C'";
                DataTable DT = Sqlhana2.GetHanaDataSQL(command);
                List<ocrd> OCRD = new List<ocrd>();
                OCRD = HanaSQL.ConvertDataTable<ocrd>(DT);
                string command3 = "SELECT \"U_Costing_Num\" AS \"Costing_Num\",\"DocEntry\" AS \"DocEntry\",\"U_CustomerName\" AS \"CustomerName\",\"U_Code\" AS \"Code\",\"U_SN\"  from " + "\"" + SCHEMA + "\"" + ". \"@COSTING_NEW\" WHERE \"U_Status\" = 'Approved' and \"U_CSP\" = 'No'";
                DataTable DT3 = Sqlhana2.GetHanaDataSQL(command3);
                List<B_to_S_to> OCRD3 = new List<B_to_S_to>();
                OCRD3 = HanaSQL.ConvertDataTable<B_to_S_to>(DT3);
                string command4 = "SELECT \"WhsCode\", \"WhsName\"  from " + "\"" + SCHEMA + "\"" + ". \"OWHS\" ";
                DataTable DT4 = Sqlhana2.GetHanaDataSQL(command4);
                List<WHS> WareHouse = new List<WHS>();
                WareHouse = HanaSQL.ConvertDataTable<WHS>(DT4);
                string command5 = "SELECT \"Code\" AS \"TaxCode\", \"Name\" AS \"TaxName\"  from " + "\"" + SCHEMA + "\"" + ". \"OSTC\" ";
                DataTable DT5 = Sqlhana2.GetHanaDataSQL(command5);
                List<OSTC> TaxName = new List<OSTC>();
                TaxName = HanaSQL.ConvertDataTable<OSTC>(DT5);
                var Data = new { OCRD, OCRD3, WareHouse, TaxName };
                return Json(Data, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        public string SL_Statefind(string StCode)
        {
            try
            {
                string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];
                string command = " select \"Name\" from " + "\"" + SCHEMA + "\"" + ".\"OCST\" where \"Code\"='" + StCode + "'";
                DataTable DT = Sqlhana2.GetHanaDataSQL(command);
                string name = string.Empty;
                if (DT.Rows.Count > 0)
                {
                    // Assuming there is only one row
                    DataRow row = DT.Rows[0];

                    // Extract the value from the "Name" column
                    name = row["Name"].ToString();
                }

                return name;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region Item Selection According to the Selected Assumption 
        [System.Web.Http.HttpGet]
        public JsonResult Dyes_Chem_Items(string a)
        {
            string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];
            string[] codes = a.Split(',');
            if (codes.Length == 1)
            {
                string command = "SELECT \"ItemCode\" AS \"ItemCode\",\"ItemName\" AS \"ItemName\",\"InvntryUom\" AS \"InvntryUom\" from " + "\"" + SCHEMA + "\"" + ". \"OITM\"  WHERE  \"ItmsGrpCod\"='" + codes[0] + "'";
                DataTable DT = Sqlhana2.GetHanaDataSQL(command);
                List<ItemDlt> Itm = new List<ItemDlt>();
                Itm = HanaSQL.ConvertDataTable<ItemDlt>(DT);

                var ls = new { Itm };
                return Json(ls, JsonRequestBehavior.AllowGet);
            }
            else if (codes.Length == 2)
            {
                string command = "SELECT \"ItemCode\" AS \"ItemCode\",\"ItemName\" AS \"ItemName\",\"InvntryUom\" AS \"InvntryUom\" from " + "\"" + SCHEMA + "\"" + ". \"OITM\" WHERE \"ItmsGrpCod\" IN ('" + codes[0] + "','" + codes[1] + "')";
                DataTable DT = Sqlhana2.GetHanaDataSQL(command);
                List<ItemDlt> Itm = new List<ItemDlt>();
                Itm = HanaSQL.ConvertDataTable<ItemDlt>(DT);

                var ls = new { Itm };
                return Json(ls, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Three Item Group are Not Allowed !!");
            }
        }
        #endregion
        public string Costing_List_App(string DocEntry)
        {
            SapPunch sp = new SapPunch();
            string res = sp.Costing_List_Approv(DocEntry);
            return res;
        }
        public string ReportCtrl()
        {
            try
            {
                string pass = WebConfigurationManager.AppSettings["Gmailpass"];
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("kartikey.pandey@itssquad.com");
                msg.To.Add("kartikey9794724044@gmail.com");
                msg.Subject = "Test Mail Subject !!";
                msg.Body = "Test Mail Body !!";
                msg.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.rediffmailpro.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("kartikey.pandey@itssquad.com", pass); // Consider using an App Password
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;

                smtp.Send(msg);
                return "Send";
            }
            catch (SmtpException smtpEx)
            {
                // Log the SMTP exception for debugging
                return $"SMTP Error: {smtpEx.Message}";
            }
            catch (Exception ex)
            {
                // Log the general exception for debugging
                return $"Error: {ex.Message}";
            }
        }

        [HttpGet]
        public string UniqSaleOrdercheck(int Salenum)
        {
            try
            {
                if (Salenum == 0)
                    return "NotExits";
                string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];
                string command = "select * from" + "\"" + SCHEMA + "\"" + ".\"@COSTING_NEW\" where  \"U_SN\"=" + "'" + Salenum + "' ";
                DataTable DT = new DataTable();
                DT = Sqlhana2.GetHanaDataSQL(command);
                if (DT.Rows.Count > 0)
                {
                    return "Exist";
                }
                else
                {
                    return "NotExits";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public ActionResult Report()
        {
            if (Session.Count != 0)
            {
                return View();
            }
            else
            {

                return RedirectToAction("login", "login");
            }
        }

        public JsonResult GetAllRecordForReport(int docentry)
        {
            string oth_user = Session["U_UserID"] as string;
            string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];

            string comand2 = "SELECT \"U_TypeOfTowel\" AS \"Type\" , \"U_Single_Double\" AS \"SD_VAL\" , \"U_Counts\" AS \"countdata\",\"U_CARDCMDBOE\" AS \"card_cmbd_OE\",\"U_CostOfTowel\" AS \"costdata\",\"U_Percentage\" AS \"Percentage\",\"U_Percentage_val\" AS \"Percentage_val\" FROM  " + "\"" + SCHEMA + "\"" + ". \"@YARN_INFORMATION\" WHERE \"DocEntry\"= '" + docentry + "' ";
            DataTable DT11 = Sqlhana2.GetHanaDataSQL(comand2);
            List<TOWEL_INFORMATION> abc2 = new List<TOWEL_INFORMATION>();
            abc2 = HanaSQL.ConvertDataTable<TOWEL_INFORMATION>(DT11);

            string comand3 = "SELECT \"DocEntry\" AS \"docentry\" ,\"U_CustomerName\" AS \"Customername\",\"U_Code\" AS \"CustomerCode\" ,\"U_DateofCosting\" AS \"CostingDate\",\"U_Merchandiser\" AS \"merchandiser\",\"U_merchandiserCode\" AS \"merchandiserCode\" ,\"U_Costing_Num\" AS \"CostingNum\" ,\"U_WeavingType\" AS \"Weaving_VAL\" ,\"U_DyedType\" AS \"DYED_VAL\",\"U_Customer_reference_no\" AS \"CRefNo\" ,\"U_SN\" AS \"SaleNum\" FROM  " + "\"" + SCHEMA + "\"" + ". \"@COSTING_NEW\" WHERE \"DocEntry\"= '" + docentry + "' ";
            DataTable DT12 = Sqlhana2.GetHanaDataSQL(comand3);
            List<Alltowelinfo2> abc3 = new List<Alltowelinfo2>();
            abc3 = HanaSQL.ConvertDataTable<Alltowelinfo2>(DT12);
            foreach (var item in abc3)
            {
                if (!string.IsNullOrEmpty(item.CostingDate))
                {
                    //item.CostingDate = DateTime.ParseExact(item.CostingDate, "d/M/yyyy h:mm:ss tt", CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");
                    string[] dp = item.CostingDate.Split('-');
                    item.CostingDate = dp[0] + "-" + dp[1] + "-" + dp[2];
                }

            }
            string comand4 = "SELECT \"U_Polyester\"AS\"POLYESTER_CODE\",\"U_ExchangeRate\" AS \"Current_Exchange_Rate_val\",\"U_Shearing\" AS \"SHEARING_VAL\", \"U_Units\" AS \"UNITS_VAL\", \r\n\"U_WashingFinish\" AS \"Wahing_Finish_CODE\", \"U_LoomStates\" AS \"Loom_States_VAL\",\"U_UnSheared\" AS \"Un_Sheared_VAL\", \"U_Enzyme\" AS \"Enzyme_VAL\", \"U_Tumbeld\" AS \"Tumbeld_res\",\r\n \"U_Stitching\" AS \"Stitching_res\", \"U_Washing\" AS \"Washing_res\", \"U_Commission_per\" AS \"Commission_per\", \"U_ExchangeCode\" AS \"Currency_Code\" FROM " + "\"" + SCHEMA + "\"" + ". \"@GENERAL_INPUTS\" WHERE \"DocEntry\" = '" + docentry + "' ";
            DataTable DT4 = Sqlhana2.GetHanaDataSQL(comand4);
            List<General_Inputs> GENRAL_INPUTS = new List<General_Inputs>();
            GENRAL_INPUTS = HanaSQL.ConvertDataTable<General_Inputs>(DT4);


            string comand15 = "SELECT \"U_Particulars_Towel\" AS \"Particulars\", \"U_Color\" AS \"COLOR_VAL\", \"U_ColorShades\" AS \"Color_Shades\",\"U_Length\" AS \"Length\", \"U_Width\" AS \"Width\",\"U_GSMIbsdoz\" AS \"GSMlbsdoz\",\"U_PcWeight\" AS \"PcWeight\", \"U_Dyngwtlossper\" AS \"DyWgt\", \"U_GrayWeight\" AS \"GryWgt\",\"U_OrderQty\" AS \"Qty\", \"U_OrderTotalKg\" AS \"TotalKg\", \"U_WeaveLossPer\" AS \"WaveLossper\", \"U_PlanQty\" AS \"planQty\", \"U_PlanTotalKg\" AS \"planTotalKg\",\"U_OrderPlanDiffPer\" AS \"OrderplanTotalper\", \"U_SalePricePerPc\" AS \"Pricepc\", \"U_SalePricePerKgs\" AS \"PriceKgs\", \"U_TotWeaveLoss\" AS \"TotalWaveLoss\" FROM " + "\"" + SCHEMA + "\"" + ".\"@TOWEL_DETAILS\" WHERE \"DocEntry\" = '" + docentry + "' ";
            DataTable DT15 = Sqlhana2.GetHanaDataSQL(comand15);
            List<BathSheetArry> BATHSHEETARRY = new List<BathSheetArry>();
            BATHSHEETARRY = HanaSQL.ConvertDataTable<BathSheetArry>(DT15);

            string comand6 = " select t1.\"U_Material\"  AS \"Material_name\", t1.\"U_UnitPrice\" AS \"Unit_Price\", t1.\"U_Quantity\" AS \"Quantity\", t1.\"U_Material_Value\" AS \"Material_Value\",\r\nt1.\"U_Material_Remarks\" AS \"Remarks\" ,t0.\"U_AssumptionCode\" AS \"Material_Code\"\r\n  FROM " + "\"" + SCHEMA + "\"" + ". \"@SLAB_MATERIAL_NAME\" t0 INNER JOIN " + "\"" + SCHEMA + "\"" + ".\"@ASSUMPTION_DETAILS\" t1 on t1.\"U_Material\"=t0.\"Name\" WHERE t1.\"DocEntry\"= '" + docentry + "' ";
            DataTable DT6 = Sqlhana2.GetHanaDataSQL(comand6);
            List<Material_slab> ASSUMPTION_DETAILS = new List<Material_slab>();
            ASSUMPTION_DETAILS = HanaSQL.ConvertDataTable<Material_slab>(DT6);

            string comand7 = "SELECT \"U_TowelType\" AS \"TowelType\",\"U_Process_Name\" AS \"U_Process_Name\", \"U_Process_Code\" AS \"U_Process_Code\" FROM  " + "\"" + SCHEMA + "\"" + ".\"@SFG_PROCESS_NEW\" WHERE \"DocEntry\" = '" + docentry + "' ";
            DataTable DT7 = Sqlhana2.GetHanaDataSQL(comand7);
            List<Production_Process> Production_Process = new List<Production_Process>();
            Production_Process = HanaSQL.ConvertDataTable<Production_Process>(DT7);

            string comand9 = "Select \"U_UserActivity\",\"U_Dpt\" FROM  " + "\"" + SCHEMA + "\"" + ".\"@VWAVEQCLOGIN\" where \"U_UserName\"='" + oth_user + "'  ";
            DataTable DT9 = Sqlhana2.GetHanaDataSQL(comand9);
            List<User_Oth> User_Oth = new List<User_Oth>();
            User_Oth = HanaSQL.ConvertDataTable<User_Oth>(DT9);

            string comand10 = "select \"U_Cost_Attach\" as \"file\" FROM  " + "\"" + SCHEMA + "\"" + ". \"@COST_ATTACHMENT_R\"  where \"DocEntry\"='" + docentry + "'   ";
            DataTable DT10 = Sqlhana2.GetHanaDataSQL(comand10);
            List<Filearr> FileDAta = new List<Filearr>();
            FileDAta = HanaSQL.ConvertDataTable<Filearr>(DT10);


            var list = new { abc2, abc3, GENRAL_INPUTS, BATHSHEETARRY, ASSUMPTION_DETAILS, Production_Process, User_Oth, FileDAta };
            int docentry_val = docentry;
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        public string SaleOrderPunch(sale_order_data obj)
        {
            SapPunch sp = new SapPunch();
            return sp.SaleOrderPunch(obj);
        }

        public JsonResult ShowStatus(string docentry)
        {
            try
            {
                string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];
                string comand13 = "select Distinct \"U_AUDITOR_NAME\" ,\"U_AUDITOR_ACTION\" FROM  " + "\"" + SCHEMA + "\"" + ". \"@AUDIT\"  where \"DocEntry\"='" + docentry + "'";
                DataTable DT13 = Sqlhana2.GetHanaDataSQL(comand13);
                List<Current_Con> AuditStatus = new List<Current_Con>();
                AuditStatus = HanaSQL.ConvertDataTable<Current_Con>(DT13);
                var lst = new { AuditStatus };      
                return Json(lst, JsonRequestBehavior.AllowGet); 
            }
            catch (Exception ex)
            {

                return Json(ex.Message);
            }
        }
    }
}