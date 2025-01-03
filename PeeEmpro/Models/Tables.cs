using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Golden_Terry_Towels.Models
{
    public class Tables
    {
    }
    public class LoginTbl
    {
        public string U_UserName { get; set; }
        public string U_Password { get; set; }
    }

    public class Terry_Towels
    {
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string Merchandiser { get; set; }
        public string CostingDate { get; set; }
        public string Count { get; set; }
        public string CardCmbdOE { get; set; }
        public string Cost { get; set; }
        public string Count1 { get; set; }
        public string CardCmbdOE1 { get; set; }
        public string Cost1 { get; set; }
        public string Coun2 { get; set; }
        public string CardCmbdOE2 { get; set; }
        public string Cost2 { get; set; }
        public string Type { get; set; }
        public string Code { get; set; }
        public string Particulars { get; set; }
        public string COLOR_VAL { get; set; }
        public List<BathSheet> BathSheet { get; set; }
        public List<BathTowel> BathTowel { get; set; }
        public List<HandTowel> HandTowel { get; set; }
        public List<WashCloth> WashCloth { get; set; }
        public List<WashGlove> WashGlove { get; set; }
    }

    public class BathSheet
    {
        public string Length { get; set; }
        public string Width { get; set; }
        public string GSMlbsdoz { get; set; }
        public string PcWeight { get; set; }
        public string Qty { get; set; }
        public string TotalKg { get; set; }
        public string Pricepc { get; set; }
        public string COLOR_VAL { get; set; }
    }

    public class BathTowel
    {
        public string Length1 { get; set; }
        public string Width1 { get; set; }
        public string GSMlbsdoz1 { get; set; }
        public string PcWeight1 { get; set; }
        public string Qty1 { get; set; }
        public string TotalKg1 { get; set; }
        public string Pricepc1 { get; set; }
        public string COLOR_VAL { get; set; }
    }

    public class HandTowel
    {
        public string Length2 { get; set; }
        public string Width2 { get; set; }
        public string GSMlbsdoz2 { get; set; }
        public string PcWeight2 { get; set; }
        public string Qty2 { get; set; }
        public string TotalKg2 { get; set; }
        public string Pricepc2 { get; set; }
        public string COLOR_VAL { get; set; }
    }

    public class WashCloth
    {
        public string Length3 { get; set; }
        public string Width3 { get; set; }
        public string GSMlbsdoz3 { get; set; }
        public string PcWeight3 { get; set; }
        public string Qty3 { get; set; }
        public string TotalKg3 { get; set; }
        public string Pricepc3 { get; set; }
        public string COLOR_VAL { get; set; }
    }

    public class WashGlove
    {
        public string Length4 { get; set; }
        public string Width4 { get; set; }
        public string GSMlbsdoz4 { get; set; }
        public string PcWeight4 { get; set; }
        public string Qty4 { get; set; }
        public string TotalKg4 { get; set; }
        public string Pricepc4 { get; set; }
        public string COLOR_VAL { get; set; }
    }


    public class OSLP
    {
        public string SlpName { get; set; }
        public string SlpCode { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Particulars { get; set; }
    }



    //kartikey

    public class SD_INPUTS
    {
        public string SD_CODE { get; set; }
        public string SD_NAME { get; set; }
        public string SD_VAL { get; set; }
    }

    public class DOBBY_INPUTS
    {
        public string DOBBY_CODE { get; set; }
        public string DOBBY_NAME { get; set; }
    }
    public class DYED_INPUTS
    {
        public string DYED_NAME { get; set; }
        public string DYED_CODE { get; set; }
    }
    public class COLOR_INPUTS
    {
        public string COLOR_NAME { get; set; }
        public string COLOR_CODE { get; set; }
        public string COLOR_VALUE { get; set; }
    }
    public class SHEARING_INPUTS
    {
        public string SHEARING_NAME { get; set; }
        public string SHEARING_CODE { get; set; }
    }
    public class UNITS_INPUTS
    {
        public string UNIT_NAME { get; set; }

    }
    public class POLYESTER_INPUTS
    {
        public string POLYESTER_NAME { get; set; }
        public string POLYESTER_CODE { get; set; }
        public string POLYESTER_VALUE { get; set; }
    }

    public class Weaving_Type
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Code_Value { get; set; }

    }
    public class ocrd
    {
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }


    }

    public class Alltowelinfo1
    {
        public string docentry { get; set; }
        public string DocEntry_val { get; set; }
        public string Customername { get; set; }
        public string CustomerCode { get; set; }
        public string CostingDate { get; set; }
        public string merchandiser { get; set; }
        public string merchandiserCode { get; set; }
        public string CRefNo { get; set; }
        public string Merch_Status { get; set; }
        public string HRem { get; set; }
        public string Particulars { get; set; }
        public string CostingType { get; set; }
        public string CostingNum { get; set; }
        public float Rawmaterialyarn { get; set; }
        public float Wastage { get; set; }
        public float Weightloss { get; set; }
        public float Electricity { get; set; }
        public float Stream { get; set; }
        public float Embroidery { get; set; }
        public float MktgExpenses { get; set; }
        public float ValueLoss { get; set; }
        public float Contractprice { get; set; }
        public string Exchangerate { get; set; }
        public string ExchangeCode { get; set; }
        public float Oceanfreightinclddc { get; set; }
        public float Commition { get; set; }
        public float Drowback { get; set; }
        public string SD_VAL { get; set; }
        public string DOBBY_VAL { get; set; }
        public string DYED_VAL { get; set; }
        public string COLOR_VAL { get; set; }
        public string SHEARING_VAL { get; set; }
        public string UNITS_VAL { get; set; }
        public string Wsashing { get; set; }
        public string Polyester_val { get; set; }
        public string Weaving_VAL { get; set; }
        public string Status { get; set; }
        public string CustomerRefNo { get; set; }
        public string AttachedFile { get; set; }
        public Boolean Revise_val { get; set; }
        public int SaleNum { get; set; }
        public string SaleNum2 { get; set; }
        public string Audit_Status { get; set; }
        public List<TOWEL_INFORMATION> Allvalues { get; set; }
        public List<BathSheetArry> BathSheetArry { get; set; }
        public List<WashClotharry> WashClotharry { get; set; }
        public List<BathTowelArry> BathTowelArry { get; set; }
        public List<HandTowelArry> HandTowelArry { get; set; }
        public List<WashGlovearry> WashGlovearry { get; set; }
        public List<Material_slab_name> Material_slab_name { get; set; }
        public List<Material_slab_Value> Material_slab_Value { get; set; }
        public List<Material_slab_Remarks> Material_slab_Remarks { get; set; }
        public List<Material_slab> Material_slab { get; set; }
        public List<Production_Process> Production_Process { get; set; }
        public List<General_Inputs> General_Inputs { get; set; }
        public List<SAL_Detail> SAL_Detail { get; set; }
        public List<Filearr> Filearr { get; set; }
    }

    public class Alltowelinfo2
    {
        public string DocEntry_val { get; set; }
        public string Customername { get; set; }
        public string CustomerCode { get; set; }
        public string CostingDate { get; set; }
        public string merchandiser { get; set; }
        public string HRem { get; set; }
        public string CRefNo { get; set; }
        public string Merch_Status { get; set; }
        public string Tname { get; set; }
        public string Tcode { get; set; }
        public string Particulars { get; set; }
        public string CostingType { get; set; }
        public string CostingNum { get; set; }
        public string Rawmaterialyarn { get; set; }
        public string Wastage { get; set; }
        public string Weightloss { get; set; }
        public string Electricity { get; set; }
        public string Stream { get; set; }
        public string Embroidery { get; set; }
        public string MktgExpenses { get; set; }
        public string ValueLoss { get; set; }
        public string Contractprice { get; set; }
        public string Exchangerate { get; set; }
        public string ExchangeCode { get; set; }
        public string Oceanfreightinclddc { get; set; }
        public string Commition { get; set; }
        public string Drowback { get; set; }
        public string SD_VAL { get; set; }
        public string DOBBY_VAL { get; set; }
        public string DYED_VAL { get; set; }
        public string COLOR_VAL { get; set; }
        public string SHEARING_VAL { get; set; }
        public string UNITS_VAL { get; set; }
        public string Wsashing { get; set; }
        public string Polyester_val { get; set; }
        public string merchandiserCode { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Weaving_VAL { get; set; }
        public string Status { get; set; }
        public string AttachedFile { get; set; }
        public string SaleNum { get; set; }
        public string Audit_Status { get; set; }
        public bool Edtbydesign { get; set; }
        public List<TOWEL_INFORMATION> Allvalues { get; set; }
        public List<BathSheetArry> BathSheetArry { get; set; }
        public List<WashClotharry> WashClotharry { get; set; }
        public List<BathTowelArry> BathTowelArry { get; set; }
        public List<HandTowelArry> HandTowelArry { get; set; }
        public List<WashGlovearry> WashGlovearry { get; set; }
        public List<Material_slab> Material_slab { get; set; }
        public List<General_Inputs> General_Inputs { get; set; }
        public List<Production_Process> Production_Process { get; set; }
        public List<SAL_Detail> SAL_Detail { get; set; }
        public List<Filearr> Filearr { get; set; }
        public List<DesignYarnInformation> DesignYarnInformation { get; set; }
    }

    public class Material_slab
    {
        public string Material_Code { get; set; }
        public string Material_name { get; set; }
        public string Material_Value { get; set; }
        public string Unit_Price { get; set; }
        public string Quantity { get; set; }
        public string ItemsGrpCode { get; set; }
        public string Remarks { get; set; }
        public string U_SrNo { get; set; }

    }

    public class Material_slab_name
    {
        public string Mname { get; set; }
    }

    public class Material_slab_Value
    {
        public string Mval { get; set; }
    }
    public class Material_slab_Remarks
    {
        public string Mrem { get; set; }
    }

    public class last_Entry
    {
        public string DocEntry { get; set; }
        public string Costing_Numntdata { get; set; }
    }

    public class Current_Exchange_Rate
    {
        public string Rate { get; set; }

    }
    public class Filearr
    {
        public string file { get; set; }
        public string name { get; set; }
        public string UserId { get; set; }
        public string userDepartment { get; set; }

    }
    public class Current_Con
    {
        public string CurrCode { get; set; }
        public string CurrName { get; set; }
        public string U_AUDITOR_NAME { get; set; }
        public string U_AUDITOR_ACTION { get; set; }
    }

    public class TOWEL_INFORMATION
    {
        public string Type { get; set; }
        public string SD_VAL { get; set; }
        public string countdata { get; set; }
        public string card_cmbd_OE { get; set; }
        public string costdata { get; set; }
        public string Percentage { get; set; }
        public string Percentage_val { get; set; }
        public string YarnRem { get; set; }

    }

    public class Costing_slab
    {
        public string LineId { get; set; }
        public string Slab_name { get; set; }
        public string Slab_Value { get; set; }

    }



    public class BathSheetArry
    {
        public string Length { get; set; }
        public string Width { get; set; }
        public string GSMlbsdoz { get; set; }
        public string PcWeight { get; set; }
        public string Qty { get; set; }
        public string TotalKg { get; set; }
        public string Pricepc { get; set; }
        public string Shearingloss { get; set; }
        public string Particulars { get; set; }
        public string COLOR_VAL { get; set; }
        public string Color_Shades { get; set; }
        public string DyWgt { get; set; }
        public string GryWgt { get; set; }
        public string WaveLossper { get; set; }
        public string planQty { get; set; }
        public string planTotalKg { get; set; }
        public string OrderplanTotalper { get; set; }
        public string PriceKgs { get; set; }
        public string TotalWaveLoss { get; set; }
        public string Design { get; set; }

    }

    public class WashClotharry
    {
        public string Length3 { get; set; }
        public string Width3 { get; set; }
        public string GSMlbsdoz3 { get; set; }
        public string PcWeight3 { get; set; }
        public string Qty3 { get; set; }
        public string TotalKg3 { get; set; }
        public string Pricepc3 { get; set; }
        public string COLOR_VAL { get; set; }
    }

    public class BathTowelArry
    {
        public string Length1 { get; set; }
        public string Width1 { get; set; }
        public string GSMlbsdoz1 { get; set; }
        public string PcWeight1 { get; set; }
        public string Qty1 { get; set; }
        public string TotalKg1 { get; set; }
        public string Pricepc1 { get; set; }
        public string COLOR_VAL { get; set; }
    }

    public class HandTowelArry
    {
        public string Length2 { get; set; }
        public string Width2 { get; set; }
        public string GSMlbsdoz2 { get; set; }
        public string PcWeight2 { get; set; }
        public string Qty2 { get; set; }
        public string TotalKg2 { get; set; }
        public string Pricepc2 { get; set; }
        public string COLOR_VAL { get; set; }
    }

    public class WashGlovearry
    {
        public string Length4 { get; set; }
        public string Width4 { get; set; }
        public string GSMlbsdoz4 { get; set; }
        public string PcWeight4 { get; set; }
        public string Qty4 { get; set; }
        public string TotalKg4 { get; set; }
        public string Pricepc4 { get; set; }
        public string COLOR_VAL { get; set; }
    }

    public class SAMPLING
    {
        public string DocEntry { get; set; }
        public string CAD_NO { get; set; }

        public string UserNameFrom { get; set; }
        public string SAM_FOR_BUYER { get; set; }
        public string DATE { get; set; }
        public string SAM_MERC { get; set; }

    }

    public class Sample
    {
        public string Sm_Doc { get; set; }
        public string Sm_Remarks { get; set; }
        public string Sm_Art { get; set; }
        public string Sm_CAD_DEV_By { get; set; }
        public string Sm_Marchent_Name { get; set; }
        public string Sm_Sample_tech { get; set; }
        public string Sm_Product_pr { get; set; }
        public string Sm_User_Name { get; set; }
        public string Sm_Date { get; set; }
        public string Sm_Quality_Refrance { get; set; }
        public string Sm_Protocol { get; set; }
        public string Sm_Samplte_For { get; set; }
        public string Sm_Create_Date { get; set; }
        public string Sm_UpdateDate { get; set; }
        public string Sm_Creator { get; set; }
        public string Sm_SAMPLE_BASED_ON { get; set; }
        public string Sm_CARD_NO { get; set; }
        public string Sm_CARD_APPROVED_BY { get; set; }
        public string Sm_SEASON { get; set; }
        public string Sm_OPPURTUNITY { get; set; }
        public string Sm_USERNAME_TO { get; set; }
        public string Sm_Quality_Type { get; set; }
        public string Sm_Market_Type { get; set; }
        public string Sm_Aproved { get; set; }


    }

    public class Semple_R
    {
        public string DocEntry { get; set; }
        public string LineId { get; set; }
        public string VisOrder { get; set; }
        public string Object { get; set; }
        public string LogInst { get; set; }

        public string U_SIZE { get; set; }
        public string U_COLOR_CODE { get; set; }
        public string U_MATERIAL { get; set; }
        public string U_SPUN { get; set; }
        public string U_TYPE { get; set; }
        public string U_COLOR_CODES { get; set; }
        public string U_APPROVED_BY { get; set; }
        public string U_APPROVED_BY_BUYER { get; set; }
        public string U_APP_DATE { get; set; }
        public string U_CAD_A { get; set; }
        public string U_ITEM_C { get; set; }
        public string U_ITEM_N { get; set; }
        public string U_DATE { get; set; }
        public string U_PunchGsm { get; set; }
        public string U_DesignName { get; set; }
        public string U_Length { get; set; }
        public string U_Width { get; set; }
        public string U_Total { get; set; }

    }

    public class B_to_S_to
    {
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string State { get; set; }
        public string Costing_Num { get; set; }
        public string DocEntry { get; set; }
        public string TowelList { get; set; }
        public string Code { get; set; }
        public string CustomerName { get; set; }
        public string U_SN { get; set; }
    }

    public class WHS
    {
        public string WhsCode { get; set; }
        public string WhsName { get; set; }
    }
    public class OSTC
    {
        public string TaxCode { get; set; }
        public string TaxName { get; set; }
    }

    public class sale_order_data
    {
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string Bill_To_v { get; set; }
        public string Ship_To_v { get; set; }
        public string Deliviry_Date { get; set; }
        public string Post_Date { get; set; }
        public string CostingNum { get; set; }
        public List<ItemDlt> ItemDlt { get; set; }
    }
    public class ItemDlt
    {
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string SFGItemCode { get; set; }
        public string SFGItemName { get; set; }
        public string UgpCode { get; set; }
        public string CostingNum { get; set; }
        public string DocEntry { get; set; }
        public string Grand_Total { get; set; }
        public string Total_for_Sales { get; set; }
        public string Total_Sales_p { get; set; }
        public string ForeignName { get; set; }
        public string OrderQty { get; set; }
        public string Totalkg { get; set; }
        public string Priceperpc { get; set; }
        public string WhsCode { get; set; }
        public string UOM { get; set; }
        public string TaxCode { get; set; }
        public string HSN_CODE { get; set; }
        public string U_merchandiserCode { get; set; }
        public string U_Exchangerate { get; set; }
        public string U_ExchangeCode { get; set; }
        public string LineId { get; set; }
        public string SaleOrd_No { get; set; }
        public string CRN { get; set; }
        public string TotalOrderQty { get; set; }

    }

    public class SFG_Item
    {
        public string last_DocEntry { get; set; }
        public List<Production_Process> Production_Process { get; set; }
        public string SFGItemCode { get; set; }
        public string SFGItemName { get; set; }
        public string SFGForeignName { get; set; }
        public string HSN_CODE { get; set; }
        public string TowelType { get; set; }
        public string ProcessCode { get; set; }
        public string LineId { get; set; }
    }
    public class Production_Process
    {
        public string Code { get; set; }
        public string TowelType { get; set; }
        public string U_Process_Name { get; set; }
        public string U_Process_Code { get; set; }
        public string U_Process_Status { get; set; }
        public string Length { get; set; }
        public string Width { get; set; }
        public string LineId { get; set; }

    }
    public class General_Inputs
    {
        public string POLYESTER_CODE { get; set; }
        public string Currency_Code { get; set; }
        public string Current_Exchange_Rate_val { get; set; }
        public string SHEARING_VAL { get; set; }
        public string UNITS_VAL { get; set; }
        public string Wahing_Finish_CODE { get; set; }
        public string Loom_States_VAL { get; set; }
        public string Un_Sheared_VAL { get; set; }
        public string Enzyme_VAL { get; set; }
        public string Tumbeld_res { get; set; }
        public string Stitching_res { get; set; }
        public string Washing_res { get; set; }
        public string Commission_per { get; set; }
        public string Price { get; set; }

    }
    public class SAL_Detail
    {
        public string SALPiece_set { get; set; }
        public string SALPcWeight { get; set; }
        public string SALKGSSET { get; set; }
        public string SALPriceindoll { get; set; }
        public string SALExchnge { get; set; }
        public string SALTotalprice { get; set; }

    }

    public class User_Oth
    {
        public string U_UserActivity { get; set; }
        public string U_Dpt { get; set; }
    }
    public class DesignYarnInformation
    {
        public string U_TypeOfTowel { get; set; }
        public string U_Single_Double { get; set; }
        public string U_Counts { get; set; }
        public string Combined_Dimensions { get; set; }
        public string U_Length { get; set; }
        public string U_Width { get; set; }
        public string U_Design { get; set; }
        public string PerPc_Yarn_Consumed_in_Mtr { get; set; }
        public string PerPc_Yarn_Consumed_in_Kg { get; set; }
        public string Tot_Yarn_Consumed_in_Mtr { get; set; }
        public string Tot_Yarn_Consumed_in_Kg { get; set; }

    }
}