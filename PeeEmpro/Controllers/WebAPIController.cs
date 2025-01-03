using Golden_Terry_Towels.Models;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.DynamicData;
using System.Web.Http;
using static Golden_Terry_Towels.Controllers.LoginController;
using System.Web.Mvc;
using System.Web.Http.Controllers;

namespace Golden_Terry_Towels.Controllers
{
    public class WebAPIController : ApiController
    {
        HanaSQL Sqlhana = new HanaSQL();
        HanaConnection con = new HanaConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Hana"].ConnectionString);
        //start working hare

        #region InitData Costing TeryTowelInit

        [System.Web.Http.HttpGet]
        public IHttpActionResult TeryTowelInit()
        {
            try
            {
                string SCHEMA = con.ConnectionString.Split(';')[2].Split('=')[1];
                string command = "SELECT \"CardName\" AS \"CustomerName\",\"CardCode\" AS \"CustomerCode\" from " + "\"" + SCHEMA + "\"" + ". \"OCRD\" WHERE \"CardType\" = 'C'";

                DataTable DT = Sqlhana.GetHanaDataSQL(command);
                List<Terry_Towels> OCRD = new List<Terry_Towels>();
                OCRD = HanaSQL.ConvertDataTable<Terry_Towels>(DT);
                string comand2 = "SELECT \"SlpName\",\"SlpCode\" from " + "\"" + SCHEMA + "\"" + ". \"OSLP\"";
                DataTable DT2 = Sqlhana.GetHanaDataSQL(comand2);
                List<OSLP> SaleEmployee = new List<OSLP>();
                SaleEmployee = HanaSQL.ConvertDataTable<OSLP>(DT2);
                //string comand3 = "SELECT \"Name\",\"Code\" from " + "\"" + SCHEMA + "\"" + ". \"@INPUT\"";
                //DataTable DT3 = Sqlhana.GetHanaDataSQL(comand3);
                //List<OSLP> INPUTDATA = new List<OSLP>();
                //INPUTDATA = HanaSQL.ConvertDataTable<OSLP>(DT3);
                string comand4 = "SELECT \"Name\" AS \"Particulars\" from " + "\"" + SCHEMA + "\"" + ". \"@PARTICULARS\"";
                DataTable DT4 = Sqlhana.GetHanaDataSQL(comand4);
                List<OSLP> Particulars = new List<OSLP>();
                Particulars = HanaSQL.ConvertDataTable<OSLP>(DT4);


                string comandINP2 = "SELECT \"U_DYED_CODE\" AS \"DYED_CODE\" , \"U_DYED_NAME\" AS \"DYED_NAME\" from " + "\"" + SCHEMA + "\"" + ". \"@DYED_TYPE\"";
                DataTable DTINP2 = Sqlhana.GetHanaDataSQL(comandINP2);
                List<DYED_INPUTS> DYED = new List<DYED_INPUTS>();
                DYED = HanaSQL.ConvertDataTable<DYED_INPUTS>(DTINP2);

                string comandINP3 = "SELECT \"U_COLOR_CODE\" AS \"COLOR_CODE\" , \"U_COLOR_NAME\" AS \"COLOR_NAME\", \"U_COLOR_VALUE\" AS \"COLOR_VALUE\" from " + "\"" + SCHEMA + "\"" + ". \"@COLOR_TYPE\"";
                DataTable DTINP3 = Sqlhana.GetHanaDataSQL(comandINP3);
                List<COLOR_INPUTS> COLOR = new List<COLOR_INPUTS>();
                COLOR = HanaSQL.ConvertDataTable<COLOR_INPUTS>(DTINP3);

                string comandINP4 = "SELECT \"U_SHEARING_CODE\" AS \"SHEARING_CODE\" , \"U_SHEARING_NAME\" AS \"SHEARING_NAME\" from " + "\"" + SCHEMA + "\"" + ". \"@SHEARING\"";
                DataTable DTINP4 = Sqlhana.GetHanaDataSQL(comandINP4);
                List<SHEARING_INPUTS> SHEARING = new List<SHEARING_INPUTS>();
                SHEARING = HanaSQL.ConvertDataTable<SHEARING_INPUTS>(DTINP4);

                string comandINP5 = "SELECT  \"U_UNITS_NAME\" AS \"UNIT_NAME\" from " + "\"" + SCHEMA + "\"" + ". \"@UNITS_TYPE\"";
                DataTable DTINP5 = Sqlhana.GetHanaDataSQL(comandINP5);
                List<UNITS_INPUTS> UNITS = new List<UNITS_INPUTS>();
                UNITS = HanaSQL.ConvertDataTable<UNITS_INPUTS>(DTINP5);

                string comandINP6 = "SELECT  \"U_POLYESTER_CODE\" AS \"POLYESTER_CODE\" , \"U_POLYESTER\" AS \"POLYESTER_NAME\", \"U_POLYESTER_VALUE\" AS \"POLYESTER_VALUE\" from " + "\"" + SCHEMA + "\"" + ". \"@POLYESTER_TYPE\"";
                DataTable DTINP6 = Sqlhana.GetHanaDataSQL(comandINP6);
                List<POLYESTER_INPUTS> POLYESTER = new List<POLYESTER_INPUTS>();
                POLYESTER = HanaSQL.ConvertDataTable<POLYESTER_INPUTS>(DTINP6);

                string command5 = "select \"Descr\" AS \"Type\" from " + "\"" + SCHEMA + "\"" + ".\"UFD1\" where \"TableID\" = '@TOWEL_INFORMATION' and \"FieldID\" = '0' ";
                DataTable DT5 = Sqlhana.GetHanaDataSQL(command5);
                List<TOWEL_INFORMATION> Type = new List<TOWEL_INFORMATION>();
                Type = HanaSQL.ConvertDataTable<TOWEL_INFORMATION>(DT5);

                string command6 = "select \"Descr\" AS \"countdata\" from " + "\"" + SCHEMA + "\"" + ".\"UFD1\" where \"TableID\" = '@TOWEL_INFORMATION' and \"FieldID\" = '1' ";
                DataTable DT6 = Sqlhana.GetHanaDataSQL(command6);
                List<TOWEL_INFORMATION> countdata = new List<TOWEL_INFORMATION>();
                countdata = HanaSQL.ConvertDataTable<TOWEL_INFORMATION>(DT6);

                //card_cmbd_OE name as Yarn Type
                string command7 = "select \"Descr\" AS \"card_cmbd_OE\" from " + "\"" + SCHEMA + "\"" + ".\"UFD1\" where \"TableID\" = '@TOWEL_INFORMATION' and \"FieldID\" = '2' ";
                DataTable DT7 = Sqlhana.GetHanaDataSQL(command7);
                List<TOWEL_INFORMATION> card_cmbd_OE = new List<TOWEL_INFORMATION>();
                card_cmbd_OE = HanaSQL.ConvertDataTable<TOWEL_INFORMATION>(DT7);

                string command11 = "select \"Descr\" AS \"Percentage\" from " + "\"" + SCHEMA + "\"" + ".\"UFD1\" where \"TableID\" = '@TOWEL_INFORMATION' and \"FieldID\" = '3' ";
                DataTable DT11 = Sqlhana.GetHanaDataSQL(command11);
                List<TOWEL_INFORMATION> Percentage = new List<TOWEL_INFORMATION>();
                Percentage = HanaSQL.ConvertDataTable<TOWEL_INFORMATION>(DT11);

                //string comand3 = "SELECT \"Name\",\"Code\" from " + "\"" + SCHEMA + "\"" + ". \"@INPUT\"";

                string command8 = "select \"LineId\" AS \"LineId\" ,\"U_SlabName\" AS \"Slab_name\",\"U_SlabValue\" AS \"Slab_Value\" from " + "\"" + SCHEMA + "\"" + ".\"@COSTING_SLAB_R\" WHERE \"DocEntry\" = '1' AND \"LineId\" BETWEEN '1' AND '8' ORDER BY \"LineId\" ";
                //SELECT "U_SlabName","U_SlabValue" FROM "@COSTING_SLAB_R"
                DataTable DT8 = Sqlhana.GetHanaDataSQL(command8);
                List<Costing_slab> slabdetail = new List<Costing_slab>();
                slabdetail = HanaSQL.ConvertDataTable<Costing_slab>(DT8);

                string command10 = "select \"Name\" AS \"Material_name\", \"U_Value\" AS \"Material_Value\", \"U_AssumptionCode\" AS \"Material_Code\" , \"U_ItemsGrpCode\" AS \"ItemsGrpCode\" from " + "\"" + SCHEMA + "\"" + ".\"@SLAB_MATERIAL_NAME\"  WHERE \"U_Show_Status\"='Active'  ORDER BY TO_INTEGER(\"U_SrNo\")";
                DataTable DT10 = Sqlhana.GetHanaDataSQL(command10);
                List<Material_slab> Material_name = new List<Material_slab>();
                Material_name = HanaSQL.ConvertDataTable<Material_slab>(DT10);

                string command12 = "select \"Name\" AS \"Name\",\"U_Value\" AS \"Value\",\"U_Code_Value\" AS \"Code_Value\" from " + "\"" + SCHEMA + "\"" + ". \"@WEAVING_TYPE\"";
                DataTable DT12 = Sqlhana.GetHanaDataSQL(command12);
                List<Weaving_Type> Name = new List<Weaving_Type>();
                Name = HanaSQL.ConvertDataTable<Weaving_Type>(DT12);

                
                string command15 = "select \"CurrCode\",\"CurrName\"  from " + "\"" + SCHEMA + "\"" + ". \"OCRN\"";
                DataTable DT15 = Sqlhana.GetHanaDataSQL(command15);
                List<Current_Con> Curry_dtl = new List<Current_Con>();
                Curry_dtl = HanaSQL.ConvertDataTable<Current_Con>(DT15);

                string command16 = "select \"Code\",\"U_Process_Name\",\"U_Process_Code\" ,\"U_Process_Status\"  from " + "\"" + SCHEMA + "\"" + ". \"@PRODUCTION_PROCESS\" ORDER BY \"Code\"";
                DataTable DT16 = Sqlhana.GetHanaDataSQL(command16);
                List<Production_Process> Production_Process = new List<Production_Process>();
                Production_Process = HanaSQL.ConvertDataTable<Production_Process>(DT16);
                string command18 = string.Empty;
                string queryApproved = string.Empty;
                string queryRejected = string.Empty;
                string queryPending = string.Empty;
                if (GlobalVariables.UserBranch == "Audit")
                {
                    command18 = "select count(*)  from " + "\"" + SCHEMA + "\"" + ". \"@COSTING_NEW\" where \"U_Merch_Status\"='Completed'";
                    queryApproved = "select count(*) from \"" + SCHEMA + "\".\"@COSTING_NEW\" where \"U_Status\"='Approved'  and \"U_Merch_Status\"='Completed'";
                    queryRejected = "select count(*) from \"" + SCHEMA + "\".\"@COSTING_NEW\" where \"U_Status\"='Reject' and \"U_Merch_Status\"='Completed'";
                    queryPending = "select count(*) from \"" + SCHEMA + "\".\"@COSTING_NEW\" where \"U_Status\"='Pending' and \"U_Merch_Status\"='Completed'";
                }
                else if (GlobalVariables.UserBranch == "Merchant")
                {
                    command18 = "select count(*)  from " + "\"" + SCHEMA + "\"" + ". \"@COSTING_NEW\" where \"U_CostingCreatedBy\"='" + GlobalVariables.userid + "'";
                    queryApproved = "select count(*) from \"" + SCHEMA + "\".\"@COSTING_NEW\" where \"U_Status\"='Approved'  and \"U_CostingCreatedBy\"='" + GlobalVariables.userid + "'";
                    queryRejected = "select count(*) from \"" + SCHEMA + "\".\"@COSTING_NEW\" where \"U_Status\"='Reject' and \"U_CostingCreatedBy\"='" + GlobalVariables.userid + "'";
                    queryPending = "select count(*) from \"" + SCHEMA + "\".\"@COSTING_NEW\" where \"U_Status\"='Pending' and \"U_CostingCreatedBy\"='" + GlobalVariables.userid + "'";
                }
                else
                {
                     command18 = "select count(*)  from " + "\"" + SCHEMA + "\"" + ". \"@COSTING_NEW\" ";
                     queryApproved = "select count(*) from \"" + SCHEMA + "\".\"@COSTING_NEW\" where \"U_Status\"='Approved'";
                     queryRejected = "select count(*) from \"" + SCHEMA + "\".\"@COSTING_NEW\" where \"U_Status\"='Reject'";
                     queryPending = "select count(*) from \"" + SCHEMA + "\".\"@COSTING_NEW\" where \"U_Status\"='Pending'";
                }
                List<int> counts = new List<int>();
                counts.Add(GetCountFromQuery(command18));
                counts.Add(GetCountFromQuery(queryApproved));
                counts.Add(GetCountFromQuery(queryRejected));
                counts.Add(GetCountFromQuery(queryPending));
                // Function to execute query and return count
                int GetCountFromQuery(string sqlQuery)
                {
                    DataTable dataTable = Sqlhana.GetHanaDataSQL(sqlQuery);
                    if (dataTable.Rows.Count == 1 && dataTable.Columns.Count == 1)
                    {
                        return Convert.ToInt32(dataTable.Rows[0][0]);
                    }
                    else
                    {
                        // Handle error or unexpected result
                        throw new Exception("Query did not return expected result.");
                    }
                }
                //string command17 = "SELECT \"ItemCode\" AS \"ItemCode\",\"ItemName\" AS \"ItemName\",\"InvntryUom\" AS \"InvntryUom\" from " + "\"" + SCHEMA + "\"" + ". \"OITM\"  WHERE  \"ItmsGrpCod\"='102'";
                string command17 = "SELECT T2.\"Price\" AS \"Priceperpc\" ,T1.\"ItemCode\" AS \"ItemCode\", T1.\"ItemName\" AS \"ItemName\" \r\nfrom " + "\"" + SCHEMA + "\"" + ". \"OITM\" T1 Inner Join " + "\"" + SCHEMA + "\"" + ". \"ITM1\" T2 On T1.\"ItemCode\"=T2.\"ItemCode\" \r\nwhere T1.\"ItmsGrpCod\"='102' and T2.\"PriceList\"='1'";
                DataTable DT17 = Sqlhana.GetHanaDataSQL(command17);
                List<ItemDlt> Itm = new List<ItemDlt>();
                Itm = HanaSQL.ConvertDataTable<ItemDlt>(DT17);

                string command19 = "select \"U_Dpt\" AS \"CurrName\"   from " + "\"" + SCHEMA + "\"" + ". \"@VWAVEQCLOGIN\" where  \"U_Branch\"='Audit'";
                DataTable DT19 = Sqlhana.GetHanaDataSQL(command19);
                List<Current_Con> AuditDep = new List<Current_Con>();
                AuditDep = HanaSQL.ConvertDataTable<Current_Con>(DT19);

                var sessBranch = HttpContext.Current.Session["UserBranch"];



                var DATA = new { OCRD, SaleEmployee, Particulars, Type, Percentage, countdata, card_cmbd_OE, COLOR, slabdetail,/* SD, DOBBY, , DYED,*/ /*SHEARING, UNITS, POLYESTER,*/ Material_name, Name,/* lastentrydata,*/ Curry_dtl, Production_Process, counts, Itm, DYED, SHEARING , UNITS , POLYESTER, AuditDep };
                return Ok(DATA);
            }
            catch (Exception ex)
            {
                return InternalServerError(new Exception("An error occurred while processing your request: " + ex.Message));

            }

        }

        #endregion end code costing 
        public IHttpActionResult GetCodeValue(string Type)
        {
            try
            {
                string SCHEMA = con.ConnectionString.Split(';')[2].Split('=')[1];
                string command = "SELECT \"Code\" from " + "\"" + SCHEMA + "\"" + ". \"@INPUT\" where \"Name\"=" + "'" + Type + "'";
                DataTable DT = Sqlhana.GetHanaDataSQL(command);
                List<OSLP> CodeValue = new List<OSLP>();
                CodeValue = HanaSQL.ConvertDataTable<OSLP>(DT);
                return Ok(CodeValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string GenerateNewCostingNumber()
        {
            string costingNumber = string.Empty;
            int DocEntry = 0;
            string SCHEMA = con.ConnectionString.Split(';')[2].Split('=')[1];
            string command13 = $"SELECT TOP 1 \"DocEntry\" FROM \"{SCHEMA}\".\"@COSTING_NEW\" ORDER BY \"DocEntry\" DESC";
            DataTable DT4 = Sqlhana.GetHanaDataSQL(command13);
            if (DT4.Rows.Count > 0)
            {
                DocEntry = DocEntry + Convert.ToInt32(DT4.Rows[0]["DocEntry"]);
            }
            if (DocEntry == 0)
            {
                costingNumber = "COS" + "0000" + 1;
            }
            else
            {
                if (DocEntry < 10)
                {
                    costingNumber = "COS" + "0000" + (DocEntry + 1).ToString();
                }
                if (DocEntry >= 10 && DocEntry < 100)
                {
                    costingNumber = "COS" + "000" + (DocEntry + 1).ToString();
                }
                if (DocEntry >= 100)
                {
                    costingNumber = "COS" + "00" + (DocEntry + 1).ToString();
                }
            }
            return costingNumber;
        }
        
    }
}




