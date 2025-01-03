using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Golden_Terry_Towels.Models;
using Sap.Data.Hana;

namespace Golden_Terry_Towels.Controllers
{
    public class LoginController : Controller
    {
        public  class Globals
        {
            public String AuditUserType = string.Empty;
        }

        public static class GlobalVariables
        {
            public static String userid { get; set; }
            public static String UserDep = string.Empty;
            public static String UserBranch = string.Empty;
        }  
        // GET: Login
        HanaSQL Sqlhana = new HanaSQL();
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginTbl user)
        {
            HanaConnection con = new HanaConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Hana"].ConnectionString);
            string SCHEMA = con.ConnectionString.Split(';')[2].Split('=')[1];
            string command = "select * from" + "\"" + SCHEMA + "\"" + ".\"@VWAVEQCLOGIN\" where  \"U_UserID\"=" + "'" + user.U_UserName + "'" + " and \"U_Password\"=" + "'" + user.U_Password + "'";
            DataTable DT = new DataTable();
            DT = Sqlhana.GetHanaDataSQL(command);
            if (DT.Rows.Count > 0)
            {
                
                String brnch= DT.Rows[0]["U_Branch"].ToString();
                GlobalVariables.UserBranch = brnch;
                Session["UserBranch"] = brnch;
                Session["Name"] = DT.Rows[0]["Name"];
                Session["U_UserID"] = DT.Rows[0]["U_UserID"];
                GlobalVariables.userid = DT.Rows[0]["U_UserID"].ToString();
                string udepp= DT.Rows[0]["U_Dpt"].ToString();
                GlobalVariables.UserDep = DT.Rows[0]["U_Dpt"].ToString();
                Session["U_UserDepartment"] = udepp;
                if (brnch == "Audit")
                {
                    string command2 = "select \"U_Dpt\" from" + "\"" + SCHEMA + "\"" + ".\"@VWAVEQCLOGIN\" where  \"U_UserID\"=" + "'" + user.U_UserName + "'" + "";
                    DataTable DT2 = new DataTable();
                    DT2 = Sqlhana.GetHanaDataSQL(command2);
                    if (DT2.Rows.Count > 0)
                    {
                        string udep = DT2.Rows[0]["U_Dpt"].ToString();
                        Session["UserDep"] = udep;
                    }
                }
                return RedirectToAction("Contact","Home");
            }
            else
            {
                ViewBag.message = "UserID or Password is wrong !!";
                return View();
            }
        }
        public ActionResult Logout()
        {            
            Session.Abandon();
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("login", "login");
        }
     
    }
    
}
    
