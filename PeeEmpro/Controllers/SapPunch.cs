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
using System.Reflection.Emit;
using Microsoft.Ajax.Utilities;
using System.Runtime.InteropServices;
using System.Web.Configuration;
using System.EnterpriseServices;

namespace PeeEmpro.Controllers
{
    public class SapPunch : Controller
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
        List<ItemDlt> DLT = new List<ItemDlt>();
        List<SFG_Item> SFGDLT = new List<SFG_Item>();
        List<SFG_Item> SFGSFGDLT = new List<SFG_Item>();
        // GET: SapPunch
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public string Costing_List_Approv(string DocEntry)
        {
            try
            {
                string ItemRes = ItemCreate(DocEntry);
                if (ItemRes == "SAPItmCreated")
                {
                    con2.Open();
                    string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];
                    string updqury = "UPDATE " + "\"" + SCHEMA + "\"" + ".\"@COSTING_NEW\" " + "SET" + "\"U_Status\"='" + "Approved" + "'  where \"DocEntry\"='" + DocEntry + "'";
                    HanaCommand insertHeader = new HanaCommand(updqury, con2);
                    HeaderrecordsAffected = insertHeader.ExecuteNonQuery();
                    con2.Close();
                    return ItemRes;
                }
                else if (ItemRes != "SAPItmCreated")
                {
                    return ItemRes;
                }
                else
                {
                    return ItemRes;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        [HttpPost]
        public string ItemCreate(string DocEntry)
        {
            try
            {
                int number = 0;
                int SaleNum = 0;
                db = new SapConnection();
                _company = db.connecttocompany(ref ErrorCode, ref _errorMessage);
                string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];
                con2.Open();
                string qurey1 = "select  \"U_Code\",\"U_SN\" from " + "\"" + SCHEMA + "\"" + ". \"@COSTING_NEW\" where \"DocEntry\"='" + DocEntry + "'";
                DataTable DT1 = Sqlhana2.GetHanaDataSQL(qurey1);
                string C_Code = DT1.Rows[0]["U_Code"].ToString();
                string SaleN = DT1.Rows[0]["U_SN"].ToString();

                string qurey2 = "select  \"CardName\" from " + "\"" + SCHEMA + "\"" + ". \"OCRD\" where \"CardCode\"='" + C_Code + "'";
                DataTable DT2 = Sqlhana2.GetHanaDataSQL(qurey2);
                string AliasName = DT2.Rows[0]["CardName"].ToString();

                string qurey = "select T1.\"LineId\",T1.\"U_Particulars_Towel\" AS \"TowelList\",T1.\"U_Length\" AS \"Length\",T1.\"U_Width\" AS \"Width\" ,T0.\"U_Costing_Num\" AS \"CostingNum\",\"U_Color\" AS \"ColorType\" from " + "\"" + SCHEMA + "\"" + ". \"@COSTING_NEW\" T0 INNER JOIN " + "\"" + SCHEMA + "\"" + ".\"@TOWEL_DETAILS\"  T1 ON T0.\"DocEntry\"=T1.\"DocEntry\" where T0.\"DocEntry\"='" + DocEntry + "'";
                DataTable DT = Sqlhana2.GetHanaDataSQL(qurey);
                string[] towelListArray = new string[DT.Rows.Count];
                string cosnum = (DT.Rows[0]["CostingNum"]).ToString();

                string SFG_qurey = "select distinct T0.\"LineId\",T0.\"U_Process_Name\" AS \"U_Process_Name\",\r\nT0.\"U_Process_Code\" AS \"U_Process_Code\"\r\nfrom " + "\"" + SCHEMA + "\"" + ". \"@SFG_PROCESS_NEW\" \r\nT0  JOIN \"" + SCHEMA + "\"" + ".\"@TOWEL_DETAILS\"  T1 ON T0.\"DocEntry\"=T1.\"DocEntry\"\r\nwhere T1.\"DocEntry\"='" + DocEntry + "'";
                DataTable DT3 = Sqlhana2.GetHanaDataSQL(SFG_qurey);

                string[] SFG_towelListArray = new string[DT3.Rows.Count];
                List<Production_Process> SFG_List = new List<Production_Process>();
                SFG_List = HanaSQL.ConvertDataTable<Production_Process>(DT3);

                string qurey3 = "SELECT TOP 1 A.\"FrgnName\", A.\"ItemCode\" from (SELECT CASE WHEN \"FrgnName\" = '' THEN '0' ELSE CAST(\"FrgnName\" AS INT) END \"FrgnName\", \"ItemCode\" FROM " + "\"" + SCHEMA + "\"" + ". \"OITM\" WHERE \"ItemCode\" LIKE 'GT-%')A ORDER BY \"FrgnName\" DESC\r\n";

                string SFGSFG_Qurey = "SELECT \"U_Single_Double\" AS \"SFGItemName\",\"U_Counts\" AS \"SFGItemCode\",\"U_Percentage\" AS \"HSN_CODE\" from " + "\"" + SCHEMA + "\"" + ".\"@YARN_INFORMATION\" where \"DocEntry\"='" + DocEntry + "'";
                DataTable DTSFGSGF = Sqlhana2.GetHanaDataSQL(SFGSFG_Qurey);
                List<SFG_Item> SFGSFGDLT = new List<SFG_Item>();
                SFGSFGDLT = HanaSQL.ConvertDataTable<SFG_Item>(DTSFGSGF);

                DataTable DT4 = Sqlhana2.GetHanaDataSQL(qurey3);
                string lastitem;
                if (DT4.Rows.Count > 0)
                {
                    lastitem = DT4.Rows[0]["FrgnName"].ToString();
                    number = int.Parse(lastitem) + 1;
                }
                else
                {
                    lastitem = "1";
                    number = int.Parse(lastitem);
                }
                // Change this value for testing
                string Total_Zero = number < 0 ? "0000" :
                                    number <= 9 ? "000" :
                                    number <= 99 ? "00" :
                                    number <= 999 ? "0" : "";
                if (SaleN != "")
                {
                    SaleNum = Convert.ToInt32(SaleN);
                }

                //for (int i = 0; i < DT.Rows.Count; i++)
                //{
                //    // Initialize a new ItemDlt instance
                //    ItemDlt item = new ItemDlt();
                //    towelList = DT.Rows[i]["TowelList"].ToString();
                //    colorType = DT.Rows[i]["ColorType"].ToString();
                //    Length = DT.Rows[i]["Length"].ToString();
                //    Width = DT.Rows[i]["Width"].ToString();
                //    item.LineId = DT.Rows[i]["LineId"].ToString();

                //    // Set common properties for item
                //    item.ItemName = SaleNum + ":" + AliasName + "-" + towelList + "-" + colorType + "-" + Length + "X" + Width;
                //    item.ForeignName = Convert.ToString(number + i);

                //    string HSNqurey = "select  \"U_HSN_CODE\" AS  \"HSN_CODE\"  from " + "\"" + SCHEMA + "\"" + ". \"@PARTICULARS\" where \"Name\"='" + towelList + "'";
                //    DataTable HSNDT = Sqlhana2.GetHanaDataSQL(HSNqurey);
                //    item.HSN_CODE = HSNDT.Rows[0]["HSN_CODE"].ToString();
                //    // Set ItemCode based on TowelList
                //    switch (towelList)
                //    {
                //        case "Bath Towel":
                //            item.ItemCode = "GT" + "-" + AliasName + "-" + "BT" + "-" + colorType + "-" + Total_Zero + (i + number);
                //            break;
                //        case "Wash Cloth":
                //            item.ItemCode = "GT" + "-" + AliasName + "-" + "WC" + "-" + colorType + "-" + Total_Zero + (i + number);
                //            break;
                //        case "Bath Sheet":
                //            item.ItemCode = "GT" + "-" + AliasName + "-" + "BS" + "-" + colorType + "-" + Total_Zero + (i + number);
                //            break;
                //        case "Wash Glove":
                //            item.ItemCode = "GT" + "-" + AliasName + "-" + "WG" + "-" + colorType + "-" + Total_Zero + (i + number);
                //            break;
                //        case "Hand Towel":
                //            item.ItemCode = "GT" + "-" + AliasName + "-" + "HT" + "-" + colorType + "-" + Total_Zero + (i + number);
                //            break;
                //        default:
                //            continue; // Skip items with unknown TowelList values
                //    }
                //    DLT.Add(item);
                //}
                ItemDlt item = new ItemDlt();
                item.ItemName = SaleN + "-" + "FG" + "-" + AliasName;
                item.ItemCode = SaleN + "-" + "FG" + "-" + C_Code; //AliasName is CustomerName
                string HSNqurey = "select  \"U_HSN_CODE\" AS  \"HSN_CODE\"  from " + "\"" + SCHEMA + "\"" + ". \"@PARTICULARS\" where \"Name\"='Bath Towel'";
                DataTable HSNDT = Sqlhana2.GetHanaDataSQL(HSNqurey);
                item.HSN_CODE = HSNDT.Rows[0]["HSN_CODE"].ToString();
                DLT.Add(item);
                // Create and add new SFG_Item instances to SFGDLT
                for (int i = 0; i < SFG_List.Count; i++)
                {
                    var sfgItem = SFG_List[i];
                    SFG_Item SFG_item = new SFG_Item();
                    SFG_item.SFGItemName = SaleNum + "-" + "SFG" + "-" + AliasName + "-" + sfgItem.U_Process_Name;
                    SFG_item.SFGItemCode = SaleNum + "-" + "SFG" + "-" + C_Code + "-" + sfgItem.U_Process_Code;
                    SFG_item.SFGForeignName = Convert.ToString(number + i);
                    SFG_item.TowelType = sfgItem.TowelType;
                    SFG_item.ProcessCode = sfgItem.U_Process_Code;
                    SFG_item.LineId = sfgItem.LineId;
                    // Add new SFG_Item instance to SFGDLT
                    SFGDLT.Add(SFG_item);
                }
                con2.Close();
                string res = SapItemPunch(DLT, SFGDLT, cosnum, DocEntry);
                if (res == "SAPItmCreated")
                {

                    return res;
                }
                else
                {
                    return res;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string SapItemPunch(List<ItemDlt> DLT, List<SFG_Item> SFGDLT, string cosnum, string DocEntry)
        {
            int SFGGrpCode = Convert.ToInt32(WebConfigurationManager.AppSettings["SFGGrpCode"]);
            int FGGrpCode = Convert.ToInt32(WebConfigurationManager.AppSettings["FGGrpCode"]);
            #region Iten Punch code
            string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];
            db = new SapConnection();
            if (!_company.Connected)
            {
                _company = db.connecttocompany(ref ErrorCode, ref _errorMessage);
            }
            if (ErrorCode == 0)
            {
                for (int j = 0; j < SFGDLT.Count; j++)
                {
                    Items = _company.GetBusinessObject(BoObjectTypes.oItems);
                    Items.ItemCode = SFGDLT[j].SFGItemCode;
                    Items.ItemName = SFGDLT[j].SFGItemName;
                    Items.ForeignName = SFGDLT[j].SFGForeignName;
                    Items.ManageBatchNumbers = BoYesNoEnum.tYES;
                    Items.ItemsGroupCode = SFGGrpCode;
                    Items.GSTRelevnt = BoYesNoEnum.tYES;
                    Items.InventoryUOM = "PCS";
                    Items.UserFields.Fields.Item("U_Costing_No").Value = cosnum;
                    ErrorCode = Items.Add();
                    if (ErrorCode == 0)
                    {
                        con2.Open();
                        string UpdateQuery = "UPDATE " + "\"" + SCHEMA + "\"" + ".\"ITM10\" " + "SET" + "\"ISOriCntry\"='" + "IN" + "'  where \"ItemCode\"='" + SFGDLT[j].SFGItemCode + "' ";
                        HanaCommand InsertHeaders = new HanaCommand(UpdateQuery, con2);
                        HeaderrecordsAffected = InsertHeaders.ExecuteNonQuery();
                        string updqury = "UPDATE " + "\"" + SCHEMA + "\"" + ".\"@SFG_PROCESS_NEW\" " + "SET" + "\"U_ItemCode\" ='" + SFGDLT[j].SFGItemCode + "' , \"U_ItemName\"='" + SFGDLT[j].SFGItemName + "' where \"DocEntry\"='" + DocEntry + "' and \"LineId\"='" + SFGDLT[j].LineId + "'";
                        HanaCommand insertHeader = new HanaCommand(updqury, con2);
                        HeaderrecordsAffected = insertHeader.ExecuteNonQuery();
                        _errorMessage = _company.GetNewObjectKey();
                        con2.Close();
                    }
                    else
                    {
                        _company.GetLastError(out ErrorCode, out _errorMessage);
                        //_errorMessage = _company.GetNewObjectKey();
                        return _errorMessage;
                    }
                }
                for (int k = 0; k < DLT.Count; k++)
                {
                    Items = _company.GetBusinessObject(BoObjectTypes.oItems);
                    Items.ItemCode = DLT[k].ItemCode;
                    Items.ItemName = DLT[k].ItemName;
                    Items.ForeignName = DLT[k].ForeignName;
                    Items.ManageBatchNumbers = BoYesNoEnum.tYES;
                    Items.ChapterID = Convert.ToInt32(DLT[k].HSN_CODE);
                    Items.ItemsGroupCode = FGGrpCode;
                    Items.InventoryUOM = "PCS";
                    Items.GSTRelevnt = BoYesNoEnum.tYES;
                    Items.UserFields.Fields.Item("U_Costing_No").Value = cosnum;
                    ErrorCode = Items.Add();
                    if (ErrorCode == 0)
                    {
                        con2.Open();
                        string UpdateQuery = "UPDATE " + "\"" + SCHEMA + "\"" + ".\"ITM10\" " + "SET" + "\"ISOriCntry\"='" + "IN" + "'  where \"ItemCode\"='" + DLT[k].ItemCode + "' ";
                        HanaCommand InsertHeadercs = new HanaCommand(UpdateQuery, con2);
                        HeaderrecordsAffected = InsertHeadercs.ExecuteNonQuery();

                        string updqury = "UPDATE " + "\"" + SCHEMA + "\"" + ".\"@TOWEL_DETAILS\" " + "SET" + "\"U_ItemCode\" ='" + DLT[k].ItemCode + "' , \"U_ItemName\"='" + DLT[k].ItemName + "' where \"DocEntry\"='" + DocEntry + "'";
                        HanaCommand insertHeader = new HanaCommand(updqury, con2);
                        HeaderrecordsAffected = insertHeader.ExecuteNonQuery();
                        con2.Close();
                        _errorMessage = _company.GetNewObjectKey();
                    }
                    else
                    {
                        _company.GetLastError(out ErrorCode, out _errorMessage);
                        //_errorMessage = _company.GetNewObjectKey();
                        return _errorMessage;
                    }
                }
                //for (int j = 0; j < SFGDLT.Count; j++)
                //{
                //    if (SFGDLT[j].ProcessCode == "PPR")
                //        BOMCreateSR(DocEntry, SFGDLT[j].SFGItemCode, false);
                //}
                //for (int k = 0; k < DLT.Count; k++)
                //{
                //    string[] subs = DLT[k].ItemCode.Split('-');
                //    if (subs[2] == "WG")
                //        BOMCreateSR(DocEntry, DLT[k].ItemCode, true, "Wash Glove");
                //    else if (subs[2] == "BT")
                //        BOMCreateSR(DocEntry, DLT[k].ItemCode, true, "Bath Towel");
                //    else if (subs[2] == "WC")
                //        BOMCreateSR(DocEntry, DLT[k].ItemCode, true, "Wash Cloth");
                //    else if (subs[2] == "BS")
                //        BOMCreateSR(DocEntry, DLT[k].ItemCode, true, "Bath Sheet");
                //    else if (subs[2] == "HT")
                //        BOMCreateSR(DocEntry, DLT[k].ItemCode, true, "Hand Towel");

                //}
                return "SAPItmCreated";
            }
            else
            {
                _company.GetLastError(out ErrorCode, out _errorMessage);
                //_errorMessage = _company.GetNewObjectKey();
                return _errorMessage;
            }
            #endregion
        }
        public string BOMCreateSR(string DocEntry, string ItemCode, bool FG, string FgItemType = null)
        {
            try
            {
                db = new SapConnection();
                float Qty = 1;
                if (!_company.Connected)
                {
                    _company = db.connecttocompany(ref ErrorCode, ref _errorMessage);
                }
                string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];
                if (ErrorCode == 0)
                {
                    if (!FG)
                    {
                        string SFGSFG_Qurey = "SELECT \"U_Single_Double\" AS \"SFGItemName\",\"U_Counts\" AS \"SFGItemCode\",\"U_Percentage\" AS \"HSN_CODE\" from " + "\"" + SCHEMA + "\"" + ".\"@YARN_INFORMATION\" where \"DocEntry\"='" + DocEntry + "'";
                        DataTable DTSFGSGF = Sqlhana2.GetHanaDataSQL(SFGSFG_Qurey);
                        SFGSFGDLT = HanaSQL.ConvertDataTable<SFG_Item>(DTSFGSGF);
                    }
                    else
                    {
                        string SFGSFG_Qurey = "SELECT \"U_ItemName\" AS \"SFGItemName\",\"U_ItemCode\" AS \"SFGItemCode\" from " + "\"" + SCHEMA + "\"" + ".\"@SFG_PROCESS_NEW\" where \"DocEntry\"='" + DocEntry + "' and \"U_TowelType\"  = '" + FgItemType + "'";
                        DataTable DTSFGSGF = Sqlhana2.GetHanaDataSQL(SFGSFG_Qurey);
                        SFGSFGDLT = HanaSQL.ConvertDataTable<SFG_Item>(DTSFGSGF);
                    }
                    productTree = _company.GetBusinessObject(BoObjectTypes.oProductTrees);
                    productTree.TreeCode = ItemCode;
                    productTree.TreeType = BoItemTreeTypes.iProductionTree;
                    productTree.Quantity = 1;
                    productTree.Warehouse = "01";
                    productTree.PlanAvgProdSize = 100;
                    productTree.PriceList = -1;
                    int currentLine = 0;
                    for (int b = 0; b < SFGSFGDLT.Count; b++)
                    {
                        if (!FG)
                        {
                            float qtyper = Convert.ToInt32(SFGSFGDLT[b].HSN_CODE);
                            Qty = (qtyper / 100);
                        }
                        productTree.Items.SetCurrentLine(currentLine);
                        productTree.Items.ItemCode = SFGSFGDLT[b].SFGItemCode;
                        productTree.Items.ItemType = ProductionItemType.pit_Item;
                        productTree.Items.IssueMethod = BoIssueMethod.im_Manual;
                        productTree.Items.Warehouse = "01";
                        productTree.Items.Quantity = Qty;
                        productTree.Items.PriceList = -1;
                        productTree.Items.Add();
                        currentLine++;
                    }
                    ErrorCode = productTree.Add();
                    if (ErrorCode == 0)
                    {
                        _errorMessage = _company.GetNewObjectKey();
                    }

                    else
                    {
                        _company.GetLastError(out ErrorCode, out _errorMessage);
                        //_errorMessage = _company.GetNewObjectKey();
                        return _errorMessage;
                    }
                    return "BOM";
                }
                else
                {
                    _company.GetLastError(out ErrorCode, out _errorMessage);
                    //_errorMessage = _company.GetNewObjectKey();
                    return _errorMessage;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        [HttpPost]
        public string BOMCreate(string DocEntry)
        {
            try
            {
                db = new SapConnection();
                _company = db.connecttocompany(ref ErrorCode, ref _errorMessage);
                string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];
                if (ErrorCode == 0)
                {
                    string SFGSFG_Qurey = "SELECT \"U_Single_Double\" AS \"SFGItemName\",\"U_Counts\" AS \"SFGItemCode\",\"U_Percentage\" AS \"HSN_CODE\" from " + "\"" + SCHEMA + "\"" + ".\"@YARN_INFORMATION\" where \"DocEntry\"='" + DocEntry + "'";
                    DataTable DTSFGSGF = Sqlhana2.GetHanaDataSQL(SFGSFG_Qurey);
                    SFGSFGDLT = HanaSQL.ConvertDataTable<SFG_Item>(DTSFGSGF);

                    for (int a = 0; a < DLT.Count; a++)
                    {
                        productTree = _company.GetBusinessObject(BoObjectTypes.oProductTrees);
                        productTree.TreeCode = DLT[a].ItemCode;
                        productTree.TreeType = BoItemTreeTypes.iProductionTree;
                        productTree.Quantity = 1;
                        productTree.Warehouse = "01";
                        productTree.PlanAvgProdSize = 100;
                        productTree.PriceList = -1;
                        int currentLine = 0;
                        for (int b = 0; b < SFGDLT.Count; b++)
                        {

                            string[] FGtname = (DLT[a].ItemName).Split('-');
                            string[] SFGtname = (SFGDLT[b].SFGItemName).Split('-');
                            if (FGtname[1] == SFGtname[2])
                            {
                                productTree.Items.SetCurrentLine(currentLine);
                                productTree.Items.ItemCode = SFGDLT[b].SFGItemCode;
                                productTree.Items.ItemType = ProductionItemType.pit_Item;
                                productTree.Items.IssueMethod = BoIssueMethod.im_Manual;
                                productTree.Items.Warehouse = "01";
                                productTree.Items.Quantity = 1;
                                productTree.Items.PriceList = -1;
                                productTree.Items.Add();
                                currentLine++;
                            }
                        }
                        ErrorCode = productTree.Add();
                    }
                    if (ErrorCode == 0)
                    {
                        //////////////////////// BOM Of BOM //////////////////////

                        for (int k = 0; k < SFGDLT.Count; k++)
                        {
                            string abc = SFGDLT[k].SFGItemCode;
                            if (abc.Contains("PPR"))
                            {
                                productTree = _company.GetBusinessObject(BoObjectTypes.oProductTrees);
                                productTree.TreeCode = SFGDLT[k].SFGItemCode;
                                productTree.TreeType = BoItemTreeTypes.iProductionTree;
                                productTree.Quantity = 1;
                                productTree.Warehouse = "01";
                                productTree.PlanAvgProdSize = 100;
                                productTree.PriceList = -1;
                                int currentLine2 = 0;
                                for (int b = 0; b < SFGSFGDLT.Count; b++)
                                {
                                    productTree.Items.SetCurrentLine(currentLine2);
                                    productTree.Items.ItemCode = SFGSFGDLT[b].SFGItemCode;
                                    productTree.Items.ItemType = ProductionItemType.pit_Item;
                                    productTree.Items.IssueMethod = BoIssueMethod.im_Manual;
                                    int qty = Convert.ToInt32(SFGSFGDLT[b].HSN_CODE) / 100;
                                    productTree.Items.Warehouse = Convert.ToString(qty);
                                    productTree.Items.Quantity = 1;
                                    productTree.Items.PriceList = -1;
                                    productTree.Items.Add();
                                    currentLine2++;
                                }
                                ErrorCode = productTree.Add();
                                if (ErrorCode == 0)
                                {
                                    _errorMessage = _company.GetNewObjectKey();
                                }
                                else
                                {
                                    _company.GetLastError(out ErrorCode, out _errorMessage);
                                    _errorMessage = _company.GetNewObjectKey();
                                    return _errorMessage;
                                }
                            }
                            if (ErrorCode == 0)
                            {
                                _errorMessage = _company.GetNewObjectKey();
                            }
                            else
                            {
                                _company.GetLastError(out ErrorCode, out _errorMessage);
                                _errorMessage = _company.GetNewObjectKey();
                                return _errorMessage;
                            }
                        }
                        _errorMessage = _company.GetNewObjectKey();
                    }

                    else
                    {
                        _company.GetLastError(out ErrorCode, out _errorMessage);
                        _errorMessage = _company.GetNewObjectKey();
                        return _errorMessage;
                    }

                    if (ErrorCode == 0)
                    {
                        _errorMessage = _company.GetNewObjectKey();
                    }
                    else
                    {
                        _company.GetLastError(out ErrorCode, out _errorMessage);
                        _errorMessage = _company.GetNewObjectKey();
                        return _errorMessage;
                    }

                    return "BOM";
                }
                else
                {
                    _company.GetLastError(out ErrorCode, out _errorMessage);
                    _errorMessage = _company.GetNewObjectKey();
                    return _errorMessage;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string ProductioOrder(string DocEntry, int saleOrderNum)
        {
            try
            {
                string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];
                string SFG_qurey = "select\"U_ItemCode\" AS \"U_Process_Code\",\"U_ItemName\" AS \"U_Process_Name\",\r\n\"U_Process_Code\" AS \"Code\"\r\nfrom " + "\"" + SCHEMA + "\"" + ". \"@SFG_PROCESS_NEW\" \r\nwhere\"DocEntry\"='" + DocEntry + "'";
                DataTable DT3 = Sqlhana2.GetHanaDataSQL(SFG_qurey);
                string[] SFG_towelListArray = new string[DT3.Rows.Count];
                List<Production_Process> SFG_List = new List<Production_Process>();
                SFG_List = HanaSQL.ConvertDataTable<Production_Process>(DT3);

                _company = db.connecttocompany(ref ErrorCode, ref _errorMessage);

                foreach (var item in SFG_List)
                {
                    int totalQty = 0;
                    int salesDocNum = 0;
                    string SFGSFG_Qurey = "SELECT \r\n    \"U_YN\" AS \"SFGItemName\",\r\n    \"U_YC\" AS \"SFGItemCode\",\r\n    \"U_TypeOfTowel\" AS \"TowelType\",\r\n    SUM(\"U_TYCKG\") AS \"HSN_CODE\"\r\nfrom " + "\"" + SCHEMA + "\"" + ".\"@DSG\"\r\nWHERE \r\n    \"DocEntry\" = '" + DocEntry + "'\r\n    AND (\"U_TypeOfTowel\" = 'Ground yarn' OR \"U_TypeOfTowel\" = 'Pile yarn')\r\nGROUP BY \r\n    \"U_YN\", \r\n    \"U_YC\", \r\n  \"U_TypeOfTowel\"";
                    DataTable DTSFGSGF = Sqlhana2.GetHanaDataSQL(SFGSFG_Qurey);
                    List<SFG_Item> SFGSFGDLT = new List<SFG_Item>();
                    SFGSFGDLT = HanaSQL.ConvertDataTable<SFG_Item>(DTSFGSGF);
                    foreach (var i in SFGSFGDLT)
                    {
                        if (i.HSN_CODE != null)
                        {
                            decimal parsedValue = Convert.ToDecimal(i.HSN_CODE);
                            totalQty += Convert.ToInt32(parsedValue);
                        }
                    }
                    if (item.Code == "PPR")
                    {
                      
                        string salesDocQurey = "select \"DocEntry\" from " + "\"" + SCHEMA + "\"" + ".\"ORDR\" where \"DocNum\"='" + saleOrderNum + "'";
                        DataTable dt = Sqlhana2.GetHanaDataSQL(salesDocQurey);
                        if (dt != null)
                        {
                            String a = dt.Rows[0]["DocEntry"].ToString();
                            salesDocNum = Convert.ToInt16(a);
                        }
                        
                        if (ErrorCode == 0)
                        {
                            SAPbobsCOM.ProductionOrders oPO = _company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductionOrders);
                            oPO.ProductionOrderStatus = SAPbobsCOM.BoProductionOrderStatusEnum.boposPlanned;
                            oPO.ProductionOrderType = SAPbobsCOM.BoProductionOrderTypeEnum.bopotSpecial;
                            oPO.ItemNo = item.U_Process_Code;
                            //oPO.ProductDescription = item.U_Process_Name;
                            oPO.PlannedQuantity = totalQty;
                            oPO.Warehouse = "WHSE 10";
                            oPO.Series = 37;
                            oPO.ProductionOrderOrigin = SAPbobsCOM.BoProductionOrderOriginEnum.bopooSalesOrder;
                            oPO.ProductionOrderOriginEntry = Convert.ToInt32(salesDocNum);
                            //oPO.ProductionOrderOriginEntry =saleOrderNum;                  
                            //oPO.UserFields.Fields.Item("U_JobOrderNO").Value = jobno;
                            int a = 0;
                            foreach (var x in SFGSFGDLT)
                            {
                                int Qty = 0;
                                if (x.HSN_CODE != null)
                                {
                                    decimal parsedValue = Convert.ToDecimal(x.HSN_CODE);
                                    Qty = Convert.ToInt32(parsedValue);
                                }
                                oPO.Lines.SetCurrentLine(a++);
                                oPO.Lines.ItemType = ProductionItemType.pit_Item;
                                //oPO.Lines.ItemType="Item";
                                oPO.Lines.ItemNo = x.SFGItemCode;
                                //oPO.ProductDescription = x.SFGItemName;
                                oPO.Lines.PlannedQuantity = Qty;
                                //oPO.Lines.ProductionOrderIssueType = BoIssueMethod.im_Manual;
                                oPO.Lines.Warehouse = "WHSE 10";
                                oPO.Lines.UserFields.Fields.Item("U_YTP").Value = x.TowelType;
                                oPO.Lines.Add();
                            }
                            ErrorCode = oPO.Add();
                            if (ErrorCode != 0)
                            {
                                _company.GetLastError(out ErrorCode, out _errorMessage);
                                return _errorMessage;
                            }
                            else
                            {
                                _errorMessage = _company.GetNewObjectKey();
                            }
                        }
                        else
                        {
                            _company.GetLastError(out ErrorCode, out _errorMessage);
                            _errorMessage = "Error : " + _errorMessage;
                            return _errorMessage;
                        }
                    }
                    if (item.Code == "WVG")
                    {
                        string SFGSFG_Qurey2 = "SELECT \r\n    \"U_YN\" AS \"SFGItemName\",\r\n    \"U_YC\" AS \"SFGItemCode\",\r\n    \"U_TypeOfTowel\" AS \"TowelType\",\r\n    SUM(\"U_TYCKG\") AS \"HSN_CODE\"\r\nfrom " + "\"" + SCHEMA + "\"" + ".\"@DSG\"\r\nWHERE \r\n    \"DocEntry\" = '" + DocEntry + "'\r\n    AND (\"U_TypeOfTowel\" = 'Weft yarn')\r\nGROUP BY \r\n    \"U_YN\", \r\n    \"U_YC\", \r\n  \"U_TypeOfTowel\"";
                        DataTable DTSFGSGF2 = Sqlhana2.GetHanaDataSQL(SFGSFG_Qurey2);
                        List<SFG_Item> SFGSFGDLT2 = new List<SFG_Item>();
                        SFGSFGDLT2 = HanaSQL.ConvertDataTable<SFG_Item>(DTSFGSGF2);

                        string salesDocQurey = "select \"DocEntry\" from " + "\"" + SCHEMA + "\"" + ".\"ORDR\" where \"DocNum\"='" + saleOrderNum + "'";
                        DataTable dt = Sqlhana2.GetHanaDataSQL(salesDocQurey);
                        if (dt != null)
                        {
                            String a = dt.Rows[0]["DocEntry"].ToString();
                            salesDocNum = Convert.ToInt16(a);
                        }
                        int totalQty1 = 0;
                        foreach (var i in SFGSFGDLT2)
                        {
                            if (i.HSN_CODE != null)
                            {
                                decimal parsedValue = Convert.ToDecimal(i.HSN_CODE);
                                totalQty1 += Convert.ToInt32(parsedValue);
                            }
                        }
                        if (ErrorCode == 0)
                        {
                            SAPbobsCOM.ProductionOrders oPO = _company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oProductionOrders);
                            oPO.ProductionOrderStatus = SAPbobsCOM.BoProductionOrderStatusEnum.boposPlanned;
                            oPO.ProductionOrderType = SAPbobsCOM.BoProductionOrderTypeEnum.bopotSpecial;
                            //oPO.ProductDescription = item.U_Process_Name;
                            oPO.ItemNo=item.U_Process_Code;
                            oPO.PlannedQuantity = (totalQty + totalQty1);
                            oPO.Warehouse = "WHSE 02";
                            oPO.Series = 37;
                            oPO.ProductionOrderOrigin = SAPbobsCOM.BoProductionOrderOriginEnum.bopooSalesOrder;
                            oPO.ProductionOrderOriginEntry = Convert.ToInt32(salesDocNum);
                            //oPO.ProductionOrderOriginEntry =saleOrderNum;                            //oPO.UserFields.Fields.Item("U_JobOrderNO").Value = jobno;
                            int a = 0;
                            var PPRItem = SFG_List.Find(x => x.Code == "PPR");

                            foreach (var x in SFGSFGDLT2)
                            {
                                int Qty = 0;
                                if (x.HSN_CODE != null)
                                {
                                    decimal parsedValue = Convert.ToDecimal(x.HSN_CODE);
                                    Qty = Convert.ToInt32(parsedValue);
                                }
                                oPO.Lines.SetCurrentLine(a++);
                                oPO.Lines.ItemType = ProductionItemType.pit_Item;
                                // oPO.Lines.ItemType="Item";
                                oPO.Lines.ItemNo = x.SFGItemCode;
                                //oPO.ProductDescription = x.SFGItemName;
                                oPO.Lines.PlannedQuantity = Qty;
                                //oPO.Lines.ProductionOrderIssueType = BoIssueMethod.im_Manual;
                                oPO.Lines.Warehouse = "WHSE 02";
                                oPO.Lines.UserFields.Fields.Item("U_YTP").Value = x.TowelType;
                                oPO.Lines.Add();
                            }
                            if (PPRItem != null)
                            {
                                oPO.Lines.ItemNo = PPRItem.U_Process_Code;
                                //oPO.ProductDescription = PPRItem.U_Process_Name;
                                oPO.Lines.PlannedQuantity = totalQty;
                                oPO.Lines.Warehouse = "WHSE 02";
                                oPO.Lines.Add();
                            }
                            ErrorCode = oPO.Add();
                            if (ErrorCode != 0)
                            {
                                _company.GetLastError(out ErrorCode, out _errorMessage);
                                return _errorMessage;
                            }
                            else
                            {
                                _errorMessage = _company.GetNewObjectKey();
                            }
                        }
                        else
                        {
                            _company.GetLastError(out ErrorCode, out _errorMessage);
                            _errorMessage = "Error : " + _errorMessage;
                            return _errorMessage;
                        }
                    }
                }
                return "POPunch";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string SaleOrderPunch(sale_order_data obj)
        {
            try
            {
                int saleOrderNum = Convert.ToInt32(obj.ItemDlt[0].SaleOrd_No);
                //string res = PO(obj.ItemDlt[0].DocEntry, saleOrderNum);
                int BPLID = Convert.ToInt32(WebConfigurationManager.AppSettings["BPLID"]);
                db = new SapConnection();
                _company = db.connecttocompany(ref ErrorCode, ref _errorMessage);
                string[] a = (obj.ItemDlt[0].U_Exchangerate).Split(':');
                string[] b = a[0].Split('-');
                if (ErrorCode == 0)
                {
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    Documents = _company.GetBusinessObject(BoObjectTypes.oOrders);
                    Documents.CardCode = obj.CustomerCode;
                    Documents.NumAtCard = obj.ItemDlt[0].CostingNum;
                    Documents.Series = -1;
                    Documents.NumAtCard = obj.ItemDlt[0].CRN;
                    Documents.DocNum = saleOrderNum;
                    Documents.DocCurrency = obj.ItemDlt[0].U_ExchangeCode;
                    Documents.DocRate = Convert.ToDouble(obj.ItemDlt[0].U_Exchangerate);
                    //Documents.C = Convert.ToDouble(obj.ItemDlt[0].U_Exchangerate);
                    Documents.SalesPersonCode = Convert.ToInt32(obj.ItemDlt[0].U_merchandiserCode);
                    if (BPLID != 0)
                        Documents.BPL_IDAssignedToInvoice = 1;
                    DateTime DocDateNew2 = DateTime.ParseExact(obj.Deliviry_Date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault);
                    Documents.DocDueDate = Convert.ToDateTime(DocDateNew2);
                    DateTime DocDateNew = DateTime.ParseExact(obj.Post_Date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault);
                    Documents.DocDate = Convert.ToDateTime(DocDateNew);
                    Documents.DocDate = Convert.ToDateTime(DocDateNew);
                    //Documents.ShipToCode = obj.Ship_To_v;
                    //Documents.PayToCode = obj.Bill_To_v;
                    //Documents.Comments = "This Sale Order for " + "/" + obj.CostingNum;
                    Documents.UserFields.Fields.Item("U_Costing_No").Value = obj.ItemDlt[0].CostingNum;
                    //Documents.DocNum = 8;

                    foreach (var x in obj.ItemDlt)
                    {
                        Documents.Lines.ItemCode = x.ItemCode;
                        Documents.Lines.Quantity = Convert.ToDouble(x.TotalOrderQty);
                        //Documents.Lines.Currency = Convert.ToString(b[1]);
                        Documents.Lines.Currency = x.U_ExchangeCode;
                        Documents.Lines.ItemDescription = x.ItemName;
                        Documents.Lines.Price = Convert.ToDouble(x.Priceperpc);
                        Documents.Lines.TaxCode = x.TaxCode;
                        Documents.Lines.WarehouseCode = x.WhsCode;
                        Documents.Lines.MeasureUnit = x.UOM;
                        //Documents.Lines.UnitsOfMeasurment = Convert.ToDouble(x.UMO);
                        Documents.Lines.UserFields.Fields.Item("U_PPERKG").Value = Convert.ToDouble(x.Priceperpc);
                        Documents.Lines.UserFields.Fields.Item("U_AltQtyKgs").Value = Convert.ToDouble(x.Totalkg);
                        ////Documents.Lines.TaxCode = BoYesNoEnum.tYES;
                        Documents.Lines.Add();
                    }

                    ErrorCode = Documents.Add();
                    if (ErrorCode != 0)
                    {
                        _company.GetLastError(out ErrorCode, out _errorMessage);
                        return _errorMessage;
                    }
                    else
                    {
                        _errorMessage = _company.GetNewObjectKey();
                        string res = ProductioOrder(obj.ItemDlt[0].DocEntry, saleOrderNum);
                        if (res == "POPunch")
                        {
                            string SCHEMA = con2.ConnectionString.Split(';')[2].Split('=')[1];
                            con2.Open();
                            string sqlCommandText = "UPDATE " + "\"" + SCHEMA + "\"" + ".\"@COSTING_NEW\" " + "SET" + "\"U_CSP\" ='Yes' where \"DocEntry\"='" + obj.ItemDlt[0].DocEntry + "' ";
                            HanaCommand insertTINFOarray = new HanaCommand(sqlCommandText, con2);
                            int recordsAffectedTINFO = insertTINFOarray.ExecuteNonQuery();
                            con2.Close();
                            if (recordsAffectedTINFO != 1)
                            {
                                return "U_CSPNotUpdate";
                            }
                            return "ok";
                        }
                        else
                        {
                            return "PONotok";
                        }
                    }
                }
                else
                {
                    _company.GetLastError(out ErrorCode, out _errorMessage);
                    _errorMessage = "Error : " + _errorMessage;
                    return _errorMessage;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}