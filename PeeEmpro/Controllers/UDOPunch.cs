using Golden_Terry_Towels.Models;
using Sap.Data.Hana;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Golden_Terry_Towels.Controllers.LoginController;


namespace PeeEmpro.Controllers
{
    public class UDOPunch : Controller
    {
        HanaConnection con2 = new HanaConnection(System.Configuration.ConfigurationManager.ConnectionStrings["Hana"].ConnectionString);
        HanaSQL Sqlhana2 = new HanaSQL();
        int HeaderrecordsAffected = 0;
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult INSERT(Alltowelinfo1 obj)
        {
            try
            {
                string UsrId = GlobalVariables.userid;
                string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];
                string Query = "select top 1 \"DocEntry\" from" + "\"" + SCHEMA + "\"" + ".\"@COSTING_NEW\" order by \"DocEntry\" desc ";
                DataTable DT = new DataTable();
                int docentry = 1;
                //int docNum = 0;
                DT = Sqlhana2.GetHanaDataSQL(Query);
                if (DT.Rows.Count > 0)
                {
                    docentry = Convert.ToInt32(DT.Rows[0]["DocEntry"]);
                    docentry = docentry + 1;
                }
                if (System.Web.HttpContext.Current.Session["U_UserID"] != null)
                {
                    UsrId = System.Web.HttpContext.Current.Session["U_UserID"].ToString();
                }
                con2.Open();
                string insertqurey = "INSERT INTO " + "\"" + SCHEMA + "\"" + ".\"@COSTING_NEW\" " +
                                       "(\"DocEntry\", \"U_CustomerName\", \"U_merchandiserCode\", \"U_Code\",\"U_Merchandiser\",\"DocNum\",\"U_DateofCosting\",\"U_Costing_Num\",\"U_DyedType\",\"U_WeavingType\",\"U_Customer_reference_no\",\"U_AttachedFile\",\"U_UpdateDate\",\"U_SN\",\"CreateDate\",\"U_CostingCreatedBy\",\"U_Merch_Status\",\"U_HR\") " +
                                       "VALUES('" + docentry + "' , '" + obj.Customername + "', '" + obj.merchandiserCode + "','" + obj.CustomerCode + "','" + obj.merchandiser + "','" + docentry + "','" + Convert.ToDateTime(obj.CostingDate).ToString("yyyyMMdd") + "','" + obj.CostingNum + "','" + obj.DYED_VAL + "','" + obj.Weaving_VAL + "','" + obj.CRefNo + "','" + obj.AttachedFile + "','" + Convert.ToDateTime(obj.CostingDate).ToString("yyyyMMdd") + "','" + obj.SaleNum + "','" + DateTime.Now.ToString("yyyyMMdd") + "','" + UsrId + "','" + obj.Merch_Status + "','" + obj.HRem + "')";
                HanaCommand insertHeader = new HanaCommand(insertqurey, con2);
                HeaderrecordsAffected = insertHeader.ExecuteNonQuery();
                if (obj.Allvalues != null)
                {
                    for (int i = 0; i < obj.Allvalues.Count; i++)
                    {
                        int lineid = 0;
                        lineid = i + 1;

                        HanaCommand insertTINFOarray = new HanaCommand("INSERT INTO " + "\"" + SCHEMA + "\"" + ".\"@YARN_INFORMATION\" " +
                            "(\"DocEntry\" , \"LineId\" ,\"U_TypeOfTowel\",\"U_Single_Double\" ,\"U_Counts\",\"U_CostOfTowel\",\"U_Percentage\",\"U_Percentage_val\",\"U_YR\")" +
                            "VALUES('" + docentry + "' , '" + lineid + "' , '" + obj.Allvalues[i].Type + "', '" + obj.Allvalues[i].SD_VAL + "' , '" + obj.Allvalues[i].countdata + "' , '" + obj.Allvalues[i].costdata + "', '" + obj.Allvalues[i].Percentage + "', '" + obj.Allvalues[i].Percentage_val + "', '" + obj.Allvalues[i].YarnRem + "')", con2);
                        int recordsAffectedTINFO = insertTINFOarray.ExecuteNonQuery();
                    }
                }
                if (obj.General_Inputs[0] != null)
                {
                    for (int i = 0; i < obj.General_Inputs.Count; i++)
                    {
                        int lineid = 0;
                        lineid = i + 1;

                        HanaCommand insertBSarray = new HanaCommand("INSERT INTO " + "\"" + SCHEMA + "\"" + ".\"@GENERAL_INPUTS\" " +
               "(\"DocEntry\", \"LineId\", \"U_Polyester\", \"U_ExchangeRate\", \"U_ExchangeCode\", \"U_Shearing\", \"U_Units\", \"U_WashingFinish\", \"U_LoomStates\", \"U_UnSheared\", \"U_Enzyme\", \"U_Tumbeld\", \"U_Stitching\", \"U_Washing\", \"U_Commission_per\") " +
               "VALUES  ('" + docentry + "' , '" + lineid + "' , '" + obj.General_Inputs[i].POLYESTER_CODE + "', '" + obj.General_Inputs[i].Current_Exchange_Rate_val + "', '" + obj.General_Inputs[i].Currency_Code + "', '" + obj.General_Inputs[i].SHEARING_VAL + "', '" + obj.General_Inputs[i].UNITS_VAL + "', '" + obj.General_Inputs[i].Wahing_Finish_CODE + "', '" + obj.General_Inputs[i].Loom_States_VAL + "', '" + obj.General_Inputs[i].Un_Sheared_VAL + "', '" + obj.General_Inputs[i].Enzyme_VAL + "', '" + obj.General_Inputs[i].Tumbeld_res + "', '" + obj.General_Inputs[i].Stitching_res + "', '" + obj.General_Inputs[i].Washing_res + "', '" + obj.General_Inputs[i].Commission_per + "')", con2);

                        int recordsAffected = insertBSarray.ExecuteNonQuery();
                    }
                }
                if (obj.BathSheetArry != null)
                {
                    for (int i = 0; i < obj.BathSheetArry.Count; i++)
                    {
                        int lineid = 0;
                        lineid = i + 1;

                        HanaCommand insertBSarray = new HanaCommand("INSERT INTO \"" + SCHEMA + "\".\"@TOWEL_DETAILS\" " +
                                                          "(\"DocEntry\", \"LineId\" , \"U_Particulars_Towel\", \"U_Color\", \"U_ColorShades\", \"U_Length\", \"U_Width\", \"U_GSMIbsdoz\", \"U_PcWeight\", \"U_Dyngwtlossper\", \"U_GrayWeight\", \"U_OrderQty\", \"U_OrderTotalKg\", \"U_WeaveLossPer\", \"U_PlanQty\", \"U_PlanTotalKg\", \"U_OrderPlanDiffPer\", \"U_SalePricePerPc\", \"U_SalePricePerKgs\", \"U_TotWeaveLoss\", \"U_Shearingloss\", \"U_Design\") " +
                                                         "VALUES ('" + docentry + "' , '" + lineid + "' , '" + obj.BathSheetArry[i].Particulars + "' , '" + obj.BathSheetArry[i].COLOR_VAL + "' , '" + obj.BathSheetArry[i].Color_Shades + "','" + obj.BathSheetArry[i].Length + "','" + obj.BathSheetArry[i].Width + "','" + obj.BathSheetArry[i].GSMlbsdoz + "' , '" + obj.BathSheetArry[i].PcWeight + "', '" + obj.BathSheetArry[i].DyWgt + "', '" + obj.BathSheetArry[i].GryWgt + "', '" + obj.BathSheetArry[i].Qty + "', '" + obj.BathSheetArry[i].TotalKg + "', '" + obj.BathSheetArry[i].WaveLossper + "', '" + obj.BathSheetArry[i].planQty + "', '" + obj.BathSheetArry[i].planTotalKg + "', '" + obj.BathSheetArry[i].OrderplanTotalper + "', '" + obj.BathSheetArry[i].Pricepc + "', '" + obj.BathSheetArry[i].PriceKgs + "', '" + obj.BathSheetArry[i].TotalWaveLoss + "', '" + obj.BathSheetArry[i].Shearingloss + "', '" + obj.BathSheetArry[i].Design + "')", con2);

                        int recordsAffected = insertBSarray.ExecuteNonQuery();
                    }
                }
                if (obj.Production_Process != null)
                {
                    for (int i = 0; i < obj.Production_Process.Count; i++)
                    {
                        int lineid = 0;
                        lineid = i + 1;

                        HanaCommand insertTINFOarray = new HanaCommand("INSERT INTO " + "\"" + SCHEMA + "\"" + ".\"@SFG_PROCESS_NEW\" " +
                            "(\"DocEntry\" , \"LineId\" ,\"U_TowelType\" ,\"U_Process_Name\",\"U_Process_Code\")" +
                            "VALUES('" + docentry + "' , '" + lineid + "' , '" + obj.Production_Process[i].TowelType + "' , '" + obj.Production_Process[i].U_Process_Name + "' , '" + obj.Production_Process[i].U_Process_Code + "')", con2);
                        int recordsAffectedTINFO = insertTINFOarray.ExecuteNonQuery();
                    }
                }
                if (obj.SAL_Detail != null)
                {
                    for (int i = 0; i < obj.SAL_Detail.Count; i++)
                    {
                        int lineid = 0;
                        lineid = i + 1;


                        HanaCommand insertBSarray = new HanaCommand("INSERT INTO " + "\"" + SCHEMA + "\"" + ".\"@SALES_INVO\" " +
    "(\"DocEntry\", \"LineId\", \"U_SALPiece_set\", \"U_SALPcWeight\", \"U_SALKGSSET\", \"U_SALPriceindoll\", \"U_SALExchnge\", \"U_SALTotalprice\") " +
    "VALUES ('" + docentry + "', '" + lineid + "', '" + obj.SAL_Detail[i].SALPiece_set + "', '" + obj.SAL_Detail[i].SALPcWeight + "', '" + obj.SAL_Detail[i].SALKGSSET + "', '" + obj.SAL_Detail[i].SALPriceindoll + "', '" + obj.SAL_Detail[i].SALExchnge + "', '" + obj.SAL_Detail[i].SALTotalprice + "' )", con2);
                        int recordsAffected = insertBSarray.ExecuteNonQuery();
                    }
                }
                if (obj.Material_slab != null)
                {
                    for (int i = 0; i < obj.Material_slab.Count; i++)
                    {
                        int lineid = i + 1;
                        // Get the material value and format it
                        string materialValue = obj.Material_slab[i].Material_Value;
                        string materialValueSql = FormatSqlValue(materialValue);

                        // Format other fields
                        string materialName = FormatSqlValue(obj.Material_slab[i].Material_name);
                        string unitPrice = FormatSqlValue(obj.Material_slab[i].Unit_Price);
                        string quantity = FormatSqlValue(obj.Material_slab[i].Quantity);
                        string remarks = FormatSqlValue(obj.Material_slab[i].Remarks);

                        // Construct the SQL command string
                        string sqlCommandText = "INSERT INTO " + "\"" + SCHEMA + "\"" + ".\"@ASSUMPTION_DETAILS\" " +
                            "(\"DocEntry\", \"LineId\", \"U_Material\", \"U_UnitPrice\", \"U_Quantity\", \"U_Material_Value\", \"U_Material_Remarks\") " +
                            "VALUES('" + docentry + "', '" + lineid + "', " + materialName + ", " + unitPrice + ", " + quantity + ", " + materialValueSql + ", " + remarks + ")";

                        // Create the command with the SQL string
                        HanaCommand insertTINFOarray = new HanaCommand(sqlCommandText, con2);

                        // Execute the command
                        int recordsAffectedTINFO = insertTINFOarray.ExecuteNonQuery();
                    }
                }
                if (obj.Filearr != null)
                {
                    for (int i = 0; i < obj.Filearr.Count; i++)
                    {
                        int lineid = 0;
                        lineid = i + 1;
                        HanaCommand insertBSarray = new HanaCommand("INSERT INTO " + "\"" + SCHEMA + "\"" + ".\"@COST_ATTACHMENT_R\" " +
    "(\"DocEntry\", \"LineId\", \"U_Cost_Attach\", \"U_UserId\", \"U_userDepartment\") " +
    "VALUES ('" + docentry + "', '" + lineid + "', '" + obj.Filearr[i].file + "', '" + obj.Filearr[i].UserId + "', '" + obj.Filearr[i].userDepartment + "')", con2);
                        int recordsAffected = insertBSarray.ExecuteNonQuery();
                    }
                }

                string AuditQurey = "SELECT \"U_Dpt\" AS \"CurrName\" from " + "\"" + SCHEMA + "\"" + ". \"@VWAVEQCLOGIN\" WHERE \"U_Branch\"='Audit'";
                DataTable DT11 = Sqlhana2.GetHanaDataSQL(AuditQurey);
                List<Current_Con> AuditArray = new List<Current_Con>();
                AuditArray = HanaSQL.ConvertDataTable<Current_Con>(DT11);

                if (AuditArray.Count > 0)
                {
                    for (int i = 0; i < AuditArray.Count; i++)
                    {
                        int lineid = 0;
                        lineid = i + 1;
                        HanaCommand insertBSarray = new HanaCommand("INSERT INTO " + "\"" + SCHEMA + "\"" + ".\"@AUDIT\" " +
    "(\"DocEntry\", \"LineId\", \"U_AUDITOR_NAME\")" +
    "VALUES ('" + docentry + "', '" + lineid + "', '" + AuditArray[i].CurrName + "')", con2);
                        int recordsAffected = insertBSarray.ExecuteNonQuery();
                    }
                }
                string lastentry = "SELECT TOP 1 \"DocEntry\" AS \"DocEntry\" ,\"U_Costing_Num\" AS \"Costing_Numntdata\" from " + "\"" + SCHEMA + "\"" + ". \"@COSTING_NEW\" ORDER BY \"DocEntry\" DESC  ";
                DataTable DT10 = Sqlhana2.GetHanaDataSQL(lastentry);
                List<last_Entry> lastentrydata = new List<last_Entry>();
                lastentrydata = HanaSQL.ConvertDataTable<last_Entry>(DT10);
                con2.Close();
                string a = string.Empty;
                a = GenerateNewCostingNumber();
                if (a != "")
                {
                    con2.Open();
                    string CosUpQure = "Update " + "\"" + SCHEMA + "\"" + ".\"@COSTING_NEW\" Set \"U_Costing_Num\"='" + a + "' where \"DocEntry\"='" + lastentrydata[0].DocEntry + "'";
                    HanaCommand insertHeader2 = new HanaCommand(CosUpQure, con2);
                    HeaderrecordsAffected = insertHeader2.ExecuteNonQuery();
                    con2.Close();
                }
                var info = new { lastentrydata, a };
                return Json(info, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var errorMessage = "Error: " + ex.Message; 
                var error = new { Message = errorMessage };
                return Json(error, JsonRequestBehavior.AllowGet);
            }
        }
        private string FormatSqlValue(string value)
        {
            if (decimal.TryParse(value, out decimal materialDecimalValue))
            {
                // If the value is numeric, do not use quotes around it
                return materialDecimalValue.ToString(CultureInfo.InvariantCulture);
            }
            else if (string.IsNullOrEmpty(value))
            {
                // If the value is null or empty, use SQL NULL
                return "NULL";
            }
            else
            {
                // Otherwise, treat it as a string and escape single quotes
                return "'" + value.Replace("'", "''") + "'";
            }
        }
        public string GenerateNewCostingNumber()
        {
            string costingNumber = string.Empty;
            int DocEntry = 1;
            string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];
            string command13 = $"SELECT TOP 1 \"DocEntry\" FROM \"{SCHEMA}\".\"@COSTING_NEW\" ORDER BY \"DocEntry\" DESC";
            DataTable DT4 = Sqlhana2.GetHanaDataSQL(command13);
            if (DT4.Rows.Count > 0)
            {
                DocEntry = Convert.ToInt32(DT4.Rows[0]["DocEntry"]);
            }
            if (DocEntry == 1)
            {
                costingNumber = "COS" + "0000" + 1;
            }
            else
            {
                if (DocEntry < 10)
                {
                    costingNumber = "COS" + "0000" + (DocEntry).ToString();
                }
                if (DocEntry >= 10 && DocEntry < 100)
                {
                    costingNumber = "COS" + "000" + (DocEntry).ToString();
                }
                if (DocEntry >= 100)
                {
                    costingNumber = "COS" + "00" + (DocEntry).ToString();
                }
            }
            return costingNumber;
        }
        public JsonResult UpdateAll(Alltowelinfo2 obj)
        {
            try
            {
                string Audit_Dep = "";
                //var UpdateDate = DateTime.Now.ToString("dd/MM/yyyy");
                string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];
                con2.Open();
                string updqury = "UPDATE " + "\"" + SCHEMA + "\"" + ".\"@COSTING_NEW\" " + "SET" + "\"U_CustomerName\" ='" + obj.Customername + "' , \"U_Merchandiser\"='" + obj.merchandiser + "', \"U_merchandiserCode\"='" + obj.merchandiserCode + "',\"U_WeavingType\"='" + obj.Weaving_VAL + "',\"U_DyedType\"='" + obj.DYED_VAL + "',\"U_UpdateDate\"='" + DateTime.Now.ToString("yyyyMMdd") + "',\"U_Status\"='" + "Pending" + "',\"U_Customer_reference_no\"='" + obj.CRefNo + "',\"U_SN\"='" + obj.SaleNum + "',\"U_Merch_Status\"='" + obj.Merch_Status + "',\"U_HR\"='" + obj.HRem + "' where \"DocEntry\"='" + obj.DocEntry_val + "'";
                HanaCommand insertHeader = new HanaCommand(updqury, con2);
                HeaderrecordsAffected = insertHeader.ExecuteNonQuery();
                con2.Close();
                con2.Open();
                string userDep = System.Web.HttpContext.Current.Session["UserDep"]?.ToString();
                if (userDep == "DESIGNING")
                {
                    if (obj.Edtbydesign == true)
                    {
                        string dltqry = "DELETE FROM \"" + SCHEMA + "\"" + ". \"@DSG\" WHERE \"DocEntry\"='" + obj.DocEntry_val + "'";
                        HanaCommand dltcmd = new HanaCommand(dltqry, con2);
                        int ress = dltcmd.ExecuteNonQuery();

                        if (obj.Allvalues != null)
                        {
                            string dltqry3 = "DELETE FROM \"" + SCHEMA + "\"" + ". \"@YARN_INFORMATION\" WHERE \"DocEntry\"='" + obj.DocEntry_val + "' ";
                            HanaCommand dltcmd3 = new HanaCommand(dltqry3, con2);
                            int ress3 = dltcmd3.ExecuteNonQuery();
                            for (int i = 0; i < obj.Allvalues.Count; i++)
                            {
                                int lineid = 0;
                                lineid = i + 1;

                                HanaCommand insertTINFOarray = new HanaCommand("INSERT INTO " + "\"" + SCHEMA + "\"" + ".\"@YARN_INFORMATION\" " +
            "(\"DocEntry\" , \"LineId\" ,\"U_TypeOfTowel\",\"U_Single_Double\" ,\"U_Counts\",\"U_CostOfTowel\",\"U_Percentage\",\"U_Percentage_val\",\"U_YR\")" +
            "VALUES('" + obj.DocEntry_val + "' , '" + lineid + "' , '" + obj.Allvalues[i].Type + "', '" + obj.Allvalues[i].SD_VAL + "' , '" + obj.Allvalues[i].countdata + "' , '" + obj.Allvalues[i].costdata + "', '" + obj.Allvalues[i].Percentage + "', '" + obj.Allvalues[i].Percentage_val + "', '" + obj.Allvalues[i].YarnRem + "')", con2);
                                int recordsAffectedTINFO = insertTINFOarray.ExecuteNonQuery();

                            }
                        }
                        else
                        {
                            string dltqry4 = "DELETE FROM \"" + SCHEMA + "\"" + ". \"@YARN_INFORMATION\" WHERE \"DocEntry\"='" + obj.DocEntry_val + "' ";
                            HanaCommand dltcmd4 = new HanaCommand(dltqry4, con2);
                            int ress4 = dltcmd4.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        if (obj.DesignYarnInformation != null)
                        {
                            string dltqryDsgn = "DELETE FROM \"" + SCHEMA + "\"" + ". \"@DSG\" WHERE \"DocEntry\"='" + obj.DocEntry_val + "' ";
                            HanaCommand Dsgndltcmd = new HanaCommand(dltqryDsgn, con2);
                            int ress = Dsgndltcmd.ExecuteNonQuery();
                            for (int i = 0; i < obj.DesignYarnInformation.Count; i++)
                            {
                                string totYarnConsumedInKg = obj.DesignYarnInformation[i].Tot_Yarn_Consumed_in_Kg ?? "0";
                                string perPcYarnConsumedInMtr = obj.DesignYarnInformation[i].PerPc_Yarn_Consumed_in_Mtr ?? "0";
                                string perPcYarnConsumedInKg = obj.DesignYarnInformation[i].PerPc_Yarn_Consumed_in_Kg ?? "0";
                                string totYarnConsumedInMtr = obj.DesignYarnInformation[i].Tot_Yarn_Consumed_in_Mtr ?? "0";

                                int lineid = 0;
                                lineid = i + 1;
                                string InsertqryDsgn = "INSERT INTO " + "\"" + SCHEMA + "\"" + ".\"@DSG\" " +
                   "(\"DocEntry\", \"LineId\", \"U_D_L\", \"U_D_W\", \"U_YC\", \"U_YN\", \"U_YCPER\", \"U_YCINPKG\", \"U_tyc\", \"U_TYCKG\", \"U_TypeOfTowel\", \"U_D_N\") " +
                   "VALUES  ('" + obj.DocEntry_val + "' , '" + lineid + "' , '" + obj.DesignYarnInformation[i].U_Length + "', '" + obj.DesignYarnInformation[i].U_Width + "', '" + obj.DesignYarnInformation[i].U_Counts + "', '" + obj.DesignYarnInformation[i].U_Single_Double + "', '" + perPcYarnConsumedInMtr + "', '" + perPcYarnConsumedInKg + "', '" + totYarnConsumedInMtr + "', '" + totYarnConsumedInKg + "', '" + obj.DesignYarnInformation[i].U_TypeOfTowel + "', '" + obj.DesignYarnInformation[i].U_Design + "')";
                                HanaCommand insertTINFOarray = new HanaCommand(InsertqryDsgn, con2);
                                int recordsAffectedTINFO = insertTINFOarray.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            string dltqry = "DELETE FROM \"" + SCHEMA + "\"" + ". \"@DSG\" WHERE \"DocEntry\"='" + obj.DocEntry_val + "'";
                            HanaCommand dltcmd = new HanaCommand(dltqry, con2);
                            int ress = dltcmd.ExecuteNonQuery();
                        }

                    }

                }
                else
                {
                    if (obj.Allvalues != null)
                    {
                        string dltqry = "DELETE FROM \"" + SCHEMA + "\"" + ". \"@YARN_INFORMATION\" WHERE \"DocEntry\"='" + obj.DocEntry_val + "' ";
                        HanaCommand dltcmd = new HanaCommand(dltqry, con2);
                        int ress = dltcmd.ExecuteNonQuery();
                        for (int i = 0; i < obj.Allvalues.Count; i++)
                        {
                            int lineid = 0;
                            lineid = i + 1;

                            HanaCommand insertTINFOarray = new HanaCommand("INSERT INTO " + "\"" + SCHEMA + "\"" + ".\"@YARN_INFORMATION\" " +
        "(\"DocEntry\" , \"LineId\" ,\"U_TypeOfTowel\",\"U_Single_Double\" ,\"U_Counts\",\"U_CostOfTowel\",\"U_Percentage\",\"U_Percentage_val\",\"U_YR\")" +
        "VALUES('" + obj.DocEntry_val + "' , '" + lineid + "' , '" + obj.Allvalues[i].Type + "', '" + obj.Allvalues[i].SD_VAL + "' , '" + obj.Allvalues[i].countdata + "' , '" + obj.Allvalues[i].costdata + "', '" + obj.Allvalues[i].Percentage + "', '" + obj.Allvalues[i].Percentage_val + "', '" + obj.Allvalues[i].YarnRem + "')", con2);
                            int recordsAffectedTINFO = insertTINFOarray.ExecuteNonQuery();

                        }
                    }
                    else
                    {
                        string dltqry = "DELETE FROM \"" + SCHEMA + "\"" + ". \"@YARN_INFORMATION\" WHERE \"DocEntry\"='" + obj.DocEntry_val + "' ";
                        HanaCommand dltcmd = new HanaCommand(dltqry, con2);
                        int ress = dltcmd.ExecuteNonQuery();
                    }
                }
                con2.Close();
                con2.Open();
                if (obj.General_Inputs[0] != null)
                {
                    string dltqry = "DELETE FROM \"" + SCHEMA + "\"" + ". \"@GENERAL_INPUTS\" WHERE \"DocEntry\"='" + obj.DocEntry_val + "' ";
                    HanaCommand dltcmd = new HanaCommand(dltqry, con2);
                    int ress = dltcmd.ExecuteNonQuery();

                    for (int i = 0; i < obj.General_Inputs.Count; i++)
                    {
                        int lineid = 0;
                        lineid = i + 1;

                        HanaCommand insertBSarray = new HanaCommand("INSERT INTO " + "\"" + SCHEMA + "\"" + ".\"@GENERAL_INPUTS\" " +
               "(\"DocEntry\", \"LineId\", \"U_Polyester\", \"U_ExchangeRate\", \"U_ExchangeCode\", \"U_Shearing\", \"U_Units\", \"U_WashingFinish\", \"U_LoomStates\", \"U_UnSheared\", \"U_Enzyme\", \"U_Tumbeld\", \"U_Stitching\", \"U_Washing\", \"U_Commission_per\") " +
               "VALUES  ('" + obj.DocEntry_val + "' , '" + lineid + "' , '" + obj.General_Inputs[i].POLYESTER_CODE + "', '" + obj.General_Inputs[i].Current_Exchange_Rate_val + "', '" + obj.General_Inputs[i].Currency_Code + "', '" + obj.General_Inputs[i].SHEARING_VAL + "', '" + obj.General_Inputs[i].UNITS_VAL + "', '" + obj.General_Inputs[i].Wahing_Finish_CODE + "', '" + obj.General_Inputs[i].Loom_States_VAL + "', '" + obj.General_Inputs[i].Un_Sheared_VAL + "', '" + obj.General_Inputs[i].Enzyme_VAL + "', '" + obj.General_Inputs[i].Tumbeld_res + "', '" + obj.General_Inputs[i].Stitching_res + "', '" + obj.General_Inputs[i].Washing_res + "', '" + obj.General_Inputs[i].Commission_per + "')", con2);

                        int recordsAffected = insertBSarray.ExecuteNonQuery();
                    }
                }
                con2.Close();
                con2.Open();
                if (obj.Production_Process != null)
                {
                    string dltqry = "DELETE FROM \"" + SCHEMA + "\"" + ". \"@SFG_PROCESS_NEW\" WHERE \"DocEntry\"='" + obj.DocEntry_val + "' ";
                    HanaCommand dltcmd = new HanaCommand(dltqry, con2);
                    int ress = dltcmd.ExecuteNonQuery();

                    for (int i = 0; i < obj.Production_Process.Count; i++)
                    {
                        int lineid = 0;
                        lineid = i + 1;

                        HanaCommand insertTINFOarray = new HanaCommand("INSERT INTO " + "\"" + SCHEMA + "\"" + ".\"@SFG_PROCESS_NEW\" " +
                            "(\"DocEntry\" , \"LineId\" ,\"U_TowelType\" ,\"U_Process_Name\",\"U_Process_Code\")" +
                            "VALUES('" + obj.DocEntry_val + "' , '" + lineid + "' , '" + obj.Production_Process[i].TowelType + "' , '" + obj.Production_Process[i].U_Process_Name + "' , '" + obj.Production_Process[i].U_Process_Code + "')", con2);
                        int recordsAffectedTINFO = insertTINFOarray.ExecuteNonQuery();
                    }
                }
                else
                {
                    string dltqry = "DELETE FROM \"" + SCHEMA + "\"" + ". \"@SFG_PROCESS_NEW\" WHERE \"DocEntry\"='" + obj.DocEntry_val + "' ";
                    HanaCommand dltcmd = new HanaCommand(dltqry, con2);
                    int ress = dltcmd.ExecuteNonQuery();
                }
                con2.Close();

                if (obj.BathSheetArry != null)
                {
                    con2.Open();
                    string dltqry = "DELETE FROM \"" + SCHEMA + "\"" + ". \"@TOWEL_DETAILS\" WHERE \"DocEntry\"='" + obj.DocEntry_val + "' ";
                    HanaCommand dltcmd = new HanaCommand(dltqry, con2);
                    //string Query = "select top 1 \"DocEntry\" from" + "\"" + SCHEMA + "\"" + ".\"@COSTING\" order by \"DocEntry\" desc ";
                    int ress = dltcmd.ExecuteNonQuery();

                    for (int i = 0; i < obj.BathSheetArry.Count; i++)
                    {
                        int lineid = 0;
                        lineid = i + 1;

                        HanaCommand insertBSarray = new HanaCommand("INSERT INTO \"" + SCHEMA + "\".\"@TOWEL_DETAILS\" " +
                                                          "(\"DocEntry\", \"LineId\" , \"U_Particulars_Towel\", \"U_Color\", \"U_ColorShades\", \"U_Length\", \"U_Width\", \"U_GSMIbsdoz\", \"U_PcWeight\", \"U_Dyngwtlossper\", \"U_GrayWeight\", \"U_OrderQty\", \"U_OrderTotalKg\", \"U_WeaveLossPer\", \"U_PlanQty\", \"U_PlanTotalKg\", \"U_OrderPlanDiffPer\", \"U_SalePricePerPc\", \"U_SalePricePerKgs\", \"U_TotWeaveLoss\", \"U_Shearingloss\", \"U_Design\") " +
                                                         "VALUES ('" + obj.DocEntry_val + "' , '" + lineid + "' , '" + obj.BathSheetArry[i].Particulars + "' , '" + obj.BathSheetArry[i].COLOR_VAL + "' , '" + obj.BathSheetArry[i].Color_Shades + "','" + obj.BathSheetArry[i].Length + "','" + obj.BathSheetArry[i].Width + "','" + obj.BathSheetArry[i].GSMlbsdoz + "' , '" + obj.BathSheetArry[i].PcWeight + "', '" + obj.BathSheetArry[i].DyWgt + "', '" + obj.BathSheetArry[i].GryWgt + "', '" + obj.BathSheetArry[i].Qty + "', '" + obj.BathSheetArry[i].TotalKg + "', '" + obj.BathSheetArry[i].WaveLossper + "', '" + obj.BathSheetArry[i].planQty + "', '" + obj.BathSheetArry[i].planTotalKg + "', '" + obj.BathSheetArry[i].OrderplanTotalper + "', '" + obj.BathSheetArry[i].Pricepc + "', '" + obj.BathSheetArry[i].PriceKgs + "', '" + obj.BathSheetArry[i].TotalWaveLoss + "', '" + obj.BathSheetArry[i].Shearingloss + "', '" + obj.BathSheetArry[i].Design + "')", con2);

                        int recordsAffected = insertBSarray.ExecuteNonQuery();
                    }
                    con2.Close();
                }
                else
                {
                    con2.Open();
                    string dltqry = "DELETE FROM \"" + SCHEMA + "\"" + ". \"@TOWEL_DETAILS\" WHERE \"DocEntry\"='" + obj.DocEntry_val + "' ";
                    HanaCommand dltcmd = new HanaCommand(dltqry, con2);
                    //string Query = "select top 1 \"DocEntry\" from" + "\"" + SCHEMA + "\"" + ".\"@COSTING\" order by \"DocEntry\" desc ";
                    int ress = dltcmd.ExecuteNonQuery();
                    con2.Close();
                }
                con2.Open();
                string dltqry2 = "DELETE FROM \"" + SCHEMA + "\"" + ". \"@ASSUMPTION_DETAILS\" WHERE \"DocEntry\"='" + obj.DocEntry_val + "' ";
                HanaCommand dltcmd2 = new HanaCommand(dltqry2, con2);
                int ress2 = dltcmd2.ExecuteNonQuery();
                for (int i = 0; i < obj.Material_slab.Count; i++)
                {
                    int lineid = i + 1;
                    string materialValue = obj.Material_slab[i].Material_Value;
                    string materialValueSql = FormatSqlValue(materialValue);
                    string materialName = FormatSqlValue(obj.Material_slab[i].Material_name);
                    string unitPrice = FormatSqlValue(obj.Material_slab[i].Unit_Price);
                    string quantity = FormatSqlValue(obj.Material_slab[i].Quantity);
                    string remarks = FormatSqlValue(obj.Material_slab[i].Remarks);
                    string sqlCommandText = "INSERT INTO " + "\"" + SCHEMA + "\"" + ".\"@ASSUMPTION_DETAILS\" " +
                        "(\"DocEntry\", \"LineId\", \"U_Material\", \"U_UnitPrice\", \"U_Quantity\", \"U_Material_Value\", \"U_Material_Remarks\") " +
                        "VALUES('" + obj.DocEntry_val + "', '" + lineid + "', " + materialName + ", " + unitPrice + ", " + quantity + ", " + materialValueSql + ", " + remarks + ")";
                    HanaCommand insertTINFOarray = new HanaCommand(sqlCommandText, con2);
                    int recordsAffectedTINFO = insertTINFOarray.ExecuteNonQuery();
                }
                con2.Close();
                if (obj.Filearr != null)
                {
                    con2.Open();
                    string dltqry3 = "DELETE FROM \"" + SCHEMA + "\"" + ". \"@COST_ATTACHMENT_R\" WHERE \"DocEntry\"='" + obj.DocEntry_val + "' ";
                    HanaCommand dltcmd3 = new HanaCommand(dltqry3, con2);
                    int ress3 = dltcmd3.ExecuteNonQuery();
                    for (int i = 0; i < obj.Filearr.Count; i++)
                    {
                        int lineid = 0;
                        lineid = i + 1;
                        HanaCommand insertBSarray = new HanaCommand("INSERT INTO " + "\"" + SCHEMA + "\"" + ".\"@COST_ATTACHMENT_R\" " +
    "(\"DocEntry\", \"LineId\", \"U_Cost_Attach\", \"U_UserId\", \"U_userDepartment\") " +
    "VALUES ('" + obj.DocEntry_val + "', '" + lineid + "', '" + obj.Filearr[i].file + "', '" + obj.Filearr[i].UserId + "', '" + obj.Filearr[i].userDepartment + "')", con2);
                        int recordsAffected = insertBSarray.ExecuteNonQuery();
                    }
                    con2.Close();
                }


                if (obj.Audit_Status != null)
                {

                    Audit_Dep = System.Web.HttpContext.Current.Session["UserDep"].ToString();
                    if (Audit_Dep != "")
                    {
                        con2.Open();
                        string AuditStatus = "Update " + "\"" + SCHEMA + "\"" + ".\"@AUDIT\" set \"U_AUDITOR_ACTION\"='" + obj.Audit_Status + "' where \"DocEntry\"='" + obj.DocEntry_val + "' and \"U_AUDITOR_NAME\"='" + Audit_Dep + "'";
                        HanaCommand AuditStatuscmd = new HanaCommand(AuditStatus, con2);
                        HeaderrecordsAffected = AuditStatuscmd.ExecuteNonQuery();
                        con2.Close();
                    }

                }
                else
                {
                    con2.Open();
                    string dltqry3 = "DELETE FROM \"" + SCHEMA + "\"" + ". \"@AUDIT\" WHERE \"DocEntry\"='" + obj.DocEntry_val + "' ";
                    HanaCommand dltcmd3 = new HanaCommand(dltqry3, con2);
                    int ress3 = dltcmd3.ExecuteNonQuery();

                    string AuditQurey = "SELECT \"U_Dpt\" AS \"CurrName\" from " + "\"" + SCHEMA + "\"" + ". \"@VWAVEQCLOGIN\" WHERE \"U_Branch\"='Audit'";
                    DataTable DT11 = Sqlhana2.GetHanaDataSQL(AuditQurey);
                    List<Current_Con> AuditArray = new List<Current_Con>();
                    AuditArray = HanaSQL.ConvertDataTable<Current_Con>(DT11);

                    if (AuditArray.Count > 0)
                    {
                        for (int i = 0; i < AuditArray.Count; i++)
                        {
                            int lineid = 0;
                            lineid = i + 1;
                            HanaCommand insertBSarray = new HanaCommand("INSERT INTO " + "\"" + SCHEMA + "\"" + ".\"@AUDIT\" " +
        "(\"DocEntry\", \"LineId\", \"U_AUDITOR_NAME\")" +
        "VALUES ('" + obj.DocEntry_val + "', '" + lineid + "', '" + AuditArray[i].CurrName + "')", con2);
                            int recordsAffected = insertBSarray.ExecuteNonQuery();
                        }
                    }
                    con2.Close();
                }
                return Json("OK");
            }
            catch (Exception ex)
            {
                var err = new Exception(ex.Message);
                return Json("Error:"+ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult EditAll(int docentry)
        {
            try
            {
                string oth_user = System.Web.HttpContext.Current.Session["U_UserID"] as string;
                string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];

                string comand2 = "SELECT \"U_TypeOfTowel\" AS \"Type\" , \"U_Single_Double\" AS \"SD_VAL\" , \"U_Counts\" AS \"countdata\",\"U_CARDCMDBOE\" AS \"card_cmbd_OE\",\"U_CostOfTowel\" AS \"costdata\",\"U_Percentage\" AS \"Percentage\",\"U_Percentage_val\" AS \"Percentage_val\",\"U_YR\" AS \"YarnRem\" FROM  " + "\"" + SCHEMA + "\"" + ". \"@YARN_INFORMATION\" WHERE \"DocEntry\"= '" + docentry + "' ";
                DataTable DT11 = Sqlhana2.GetHanaDataSQL(comand2);
                List<TOWEL_INFORMATION> abc2 = new List<TOWEL_INFORMATION>();
                abc2 = HanaSQL.ConvertDataTable<TOWEL_INFORMATION>(DT11);

                string comand3 = "SELECT \"DocEntry\" AS \"docentry\" ,\"U_CustomerName\" AS \"Customername\",\"U_Code\" AS \"CustomerCode\" ,\"U_DateofCosting\" AS \"CostingDate\",\"U_Merchandiser\" AS \"merchandiser\",\"U_merchandiserCode\" AS \"merchandiserCode\" ,\"U_Costing_Num\" AS \"CostingNum\" ,\"U_WeavingType\" AS \"Weaving_VAL\" ,\"U_DyedType\" AS \"DYED_VAL\",\"U_Customer_reference_no\" AS \"CRefNo\" ,\"U_SN\" AS \"SaleNum\",\"U_Merch_Status\" AS \"Merch_Status\",\"U_HR\" AS \"HRem\" FROM  " + "\"" + SCHEMA + "\"" + ". \"@COSTING_NEW\" WHERE \"DocEntry\"= '" + docentry + "' ";
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


                string comand15 = "SELECT \"U_Particulars_Towel\" AS \"Particulars\", \"U_Color\" AS \"COLOR_VAL\", \"U_ColorShades\" AS \"Color_Shades\",\"U_Length\" AS \"Length\", \"U_Width\" AS \"Width\",\"U_GSMIbsdoz\" AS \"GSMlbsdoz\",\"U_PcWeight\" AS \"PcWeight\", \"U_Dyngwtlossper\" AS \"DyWgt\", \"U_GrayWeight\" AS \"GryWgt\",\"U_OrderQty\" AS \"Qty\", \"U_OrderTotalKg\" AS \"TotalKg\", \"U_WeaveLossPer\" AS \"WaveLossper\", \"U_PlanQty\" AS \"planQty\", \"U_PlanTotalKg\" AS \"planTotalKg\",\"U_OrderPlanDiffPer\" AS \"OrderplanTotalper\", \"U_SalePricePerPc\" AS \"Pricepc\", \"U_SalePricePerKgs\" AS \"PriceKgs\", \"U_TotWeaveLoss\" AS \"TotalWaveLoss\", \"U_Shearingloss\" AS \"Shearingloss\", \"U_Design\" AS \"Design\" FROM " + "\"" + SCHEMA + "\"" + ".\"@TOWEL_DETAILS\" WHERE \"DocEntry\" = '" + docentry + "' ";
                DataTable DT15 = Sqlhana2.GetHanaDataSQL(comand15);
                List<BathSheetArry> BATHSHEETARRY = new List<BathSheetArry>();
                BATHSHEETARRY = HanaSQL.ConvertDataTable<BathSheetArry>(DT15);

                string comand6 = " select t1.\"U_Material\"  AS \"Material_name\", t1.\"U_UnitPrice\" AS \"Unit_Price\", t1.\"U_Quantity\" AS \"Quantity\", t1.\"U_Material_Value\" AS \"Material_Value\",\r\nt1.\"U_Material_Remarks\" AS \"Remarks\" ,t0.\"U_AssumptionCode\" AS \"Material_Code\"\r\n  FROM " + "\"" + SCHEMA + "\"" + ". \"@SLAB_MATERIAL_NAME\" t0 INNER JOIN " + "\"" + SCHEMA + "\"" + ".\"@ASSUMPTION_DETAILS\" t1 on t1.\"U_Material\"=t0.\"Name\" WHERE t1.\"DocEntry\"= '" + docentry + "' ORDER BY \r\n    CAST(t0.\"U_SrNo\" AS INTEGER)";
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

                string comand10 = "select \"U_Cost_Attach\" as \"file\",\"U_UserId\" as \"UserId\" ,\"U_userDepartment\" as \"userDepartment\" FROM  " + "\"" + SCHEMA + "\"" + ". \"@COST_ATTACHMENT_R\"  where \"DocEntry\"='" + docentry + "'   ";
                DataTable DT10 = Sqlhana2.GetHanaDataSQL(comand10);
                List<Filearr> FileDAta = new List<Filearr>();
                FileDAta = HanaSQL.ConvertDataTable<Filearr>(DT10);

                string comand13 = "select Distinct \"U_AUDITOR_NAME\" ,\"U_AUDITOR_ACTION\" FROM  " + "\"" + SCHEMA + "\"" + ". \"@AUDIT\"  where \"DocEntry\"='" + docentry + "'   ";
                DataTable DT13 = Sqlhana2.GetHanaDataSQL(comand13);
                List<Current_Con> AuditStatus = new List<Current_Con>();
                AuditStatus = HanaSQL.ConvertDataTable<Current_Con>(DT13);
                List<DesignYarnInformation> DesignYarnInformation = new List<DesignYarnInformation>();

                if (System.Web.HttpContext.Current.Session["UserDep"] != null && System.Web.HttpContext.Current.Session["UserDep"].ToString() == "DESIGNING")
                {
                    string DSGSelcetQurey = "SELECT \r\n \"U_D_L\" AS \"U_Length\", \r\n    \"U_D_W\" AS \"U_Width\", \r\n    \"U_YC\" AS \"U_Counts\", \r\n    \"U_YN\" AS \"U_Single_Double\", \r\n    \"U_YCPER\" AS \"PerPc_Yarn_Consumed_in_Mtr\", \n    \"U_YCINPKG\" AS \"PerPc_Yarn_Consumed_in_Kg\", \n    \"U_tyc\" AS \"Tot_Yarn_Consumed_in_Mtr\", \n   \"U_TYCKG\" AS \"Tot_Yarn_Consumed_in_Kg\", \n   \"U_TypeOfTowel\" AS \"U_TypeOfTowel\" \r\n FROM \r\n   " + "\"" + SCHEMA + "\"" + ".\"@DSG\"\r\nWHERE \r\n    \"DocEntry\" = '" + docentry + "'";
                    DataTable dt16 = Sqlhana2.GetHanaDataSQL(DSGSelcetQurey);
                    DesignYarnInformation = HanaSQL.ConvertDataTable<DesignYarnInformation>(dt16);
                    if (DesignYarnInformation.Count == 0)
                    {
                        string command15 = "SELECT \r\n    y.\"U_TypeOfTowel\",\r\n    y.\"U_Single_Double\",\r\n    y.\"U_Counts\",\r\n    d.\"Combined_Dimensions\",\r\n    CAST(SUBSTRING(d.\"Combined_Dimensions\", 1, INSTR(d.\"Combined_Dimensions\", '/') - 1) AS DECIMAL(10, 2)) AS \"U_Length\",\r\n    CAST(SUBSTRING(d.\"Combined_Dimensions\", INSTR(d.\"Combined_Dimensions\", '/') + 1) AS DECIMAL(10, 2)) AS \"U_Width\",\r\n d.\"U_Design\"\r\nFROM \r\n    (SELECT DISTINCT \r\n        \"U_Length\" || '/' || \"U_Width\" AS \"Combined_Dimensions\",\r\n        \"U_Design\"  \r\n     FROM \r\n   " + "\"" + SCHEMA + "\"" + ".\"@TOWEL_DETAILS\"\r\n     WHERE \r\n        \"DocEntry\" = '" + docentry + "') AS d\r\nCROSS JOIN \r\n    (SELECT \r\n        \"U_TypeOfTowel\",\r\n        \"U_Single_Double\",\r\n        \"U_Counts\"\r\n     FROM \r\n   " + "\"" + SCHEMA + "\"" + ".\"@YARN_INFORMATION\"\r\n     WHERE \r\n        \"DocEntry\" = '" + docentry + "') AS y\r\nORDER BY \r\n    d.\"Combined_Dimensions\", y.\"U_TypeOfTowel\"";
                        DataTable dt15 = Sqlhana2.GetHanaDataSQL(command15);
                        DesignYarnInformation = HanaSQL.ConvertDataTable<DesignYarnInformation>(dt15);
                    }
                }
                var list = new { abc2, abc3, GENRAL_INPUTS, BATHSHEETARRY, ASSUMPTION_DETAILS, Production_Process, User_Oth, FileDAta, AuditStatus, DesignYarnInformation };
                int docentry_val = docentry;
                return Json(list, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}