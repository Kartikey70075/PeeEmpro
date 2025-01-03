using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace CRM_Sahib.Models
{
    public class SapConnection
    {

        //public static string gConnStr;
        //public static string gHANASERVER;
        //public static string gHANAUSER_NAME;
        //public static string gHANAUSER_PASSWORD;
        //public static string gHANADATABASE;
        //public static string gSAPSERVER;
        //public static string gSAPUSER_NAME;
        //public static string gSAPUSER_PASSWORD;
        //public static string gSAPDATABASE;
        //public static string strConnectionString = string.Empty;
        //public static DbConnection oDbConnection;
        //string Is_serverType = string.Empty;
        public Company connecttocompany(ref int ErrorCode, ref string _errorMessage)
        {
            try
            {
                Company _company = new Company();
                if (!_company.Connected)
                {
                    // _company.DbServerType = (BoDataServerTypes)Convert.ToInt32(ConfigurationManager.AppSettings["databaseserver"]);
                    _company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB;
                    //_company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2019;
                    _company.Server = ConfigurationManager.AppSettings["SAPSERVER"];
                    _company.DbUserName = ConfigurationManager.AppSettings["SAPDatabaseUser"];
                    _company.DbPassword = ConfigurationManager.AppSettings["SAPDatabasePassword"];
                    _company.LicenseServer = ConfigurationManager.AppSettings["SAPLicenseServer"];
                    _company.CompanyDB = ConfigurationManager.AppSettings["SAPDATABASE"];
                    _company.UserName = ConfigurationManager.AppSettings["SAPUSER"];
                    _company.Password = ConfigurationManager.AppSettings["SAPPASSWORD"];
                    ErrorCode = _company.Connect();
                    if (ErrorCode != 0)
                    {
                        _company.GetLastError(out ErrorCode, out _errorMessage);
                    }
                }
                return _company;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}                                          