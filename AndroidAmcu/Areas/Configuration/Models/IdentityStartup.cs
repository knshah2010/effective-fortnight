using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndroidAmcu.Areas.Configuration.Models
{
    public class IdentityStartup
    {
      //  public ConfigClass config { get; set; }
        public Dictionary<string,dynamic> config { get; set; }
     
        public collectionConfigClass collectionConfig { get; set; }
        public rateClass rate { get; set; }
        public bool memberDownload { get; set; } = false;
        public string welcomeMessage { get; set; } = "Welcome to Everest Instruments Pvt. Ltd.";
     //   public List<vendorTypeClass> vendorType { get; set; }
        public List<ShiftTime> shift_timing { get; set; }
        public string menuMapping { get; set; }
        public string isSurveyor { get; set; } = "0";

    }
    public class ConfigClass
    {
        public bool collectionBlock { get; set; } = false;
        public bool dcsBlock { get; set; } = false;
        public bool dispatchMandate { get; set; } = false;
        public bool weightManual { get; set; }
        public bool qualityManual { get; set; }
        public string canPerLtr { get; set; }
        public string canWarning { get; set; }
        public string ltrToKgConstant { get; set; }
        public string clrConstant1 { get; set; }
        public string clrConstant2 { get; set; }
        public string sampleMilkSize { get; set; }
        public string acceptOtherMilk { get; set; }
        public string multiEntrySameMilk { get; set; }
        public string multiEntryOtherMilk { get; set; }
        public string collectionQtyMode { get; set; }
        public string localSaleQtyMode { get; set; }
        public string sampleMilkQtyMode { get; set; }
        public string dispatchQtyMode { get; set; }
        public string weightSetting { get; set; }
        public string qualitySetting { get; set; }
        public string avgParamShift { get; set; }
        public string allowMemberCreate { get; set; }
        public string allowRateCreate { get; set; }
        public string allowRateImport { get; set; }
        public string shiftLock { get; set; }
        public string milkCollectionExport { get; set; }
        public string maxSyncRecord { get; set; }
        public string realtimeSyncTables { get; set; }
        public string rateType { get; set; }
    }
    public class collectionConfigClass
    {
        public string mStartTime { get; set; } = "";
        public string mCutoffTime { get; set; } = "";
        public string mLockTime { get; set; } = "";
        public string eStartTime { get; set; } = "";
        public string eCutoffTime { get; set; } = "";
        public string eLockTime { get; set; } = "";
        public string incRate { get; set; } = "";
        public string incDeduction { get; set; } = "";
        public List<allowedMilkTypeClass> allowedMilkType { get; set; }
        public List<collectionIncentiveDeductionClass> collectionIncentiveDeduction { get; set; }
        public List<milkTypeRateClass> milkTypeRate { get; set; }
        public qualityParamClass qualityParam { get; set; }
        
    }
    public class rateClass
    {
        public string mPurchaseRateCode { get; set; } = "";
        public string mPurchaseRateCodeBlock { get; set; } = "";
        public string ePurchaseRateCode { get; set; } = "";
        public string ePurchaseRateCodeBlock { get; set; } = "";
        public string memberApplicableRate { get; set; } = "";
        public string bmcApplicableRate { get; set; } = "";
    }
    public class vendorTypeClass
    {
        public string customerType { get; set; }
        public string customerDesc { get; set; }
        public string codePrefix { get; set; }
        public int codeLength { get; set; }
    }
    public class allowedMilkTypeClass
    {
        public int milkTypeCode { get; set; } = 0;
        public string milkTypeName { get; set; }
        public decimal minFat { get; set; } = 0.0M;
        public decimal maxFat { get; set; } = 0.0M;
        public decimal minSnf { get; set; } = 0.0M;
        public decimal maxSnf { get; set; } = 0.0M;
        public decimal minClr { get; set; } = 0.0M;
        public decimal maxClr { get; set; } = 0.0M;
    }
    public class milkTypeRateClass
    {
        public int milkTypeCode { get; set; }
        public string milkTypeName { get; set; }
        public decimal rtpl { get; set; } = 0;
    }
    public class qualityParamClass{
        public decimal fat { get; set; } = 6.5M;
        public decimal snf { get; set; } = 9.0M;
        public decimal clr { get; set; } = 0.0M;
    }
    public class collectionIncentiveDeductionClass
    {
        public string fromTime { get; set; }
        public string toTime { get; set; }
        public int schemeType { get; set; }
        public int shiftCode { get; set; }
        public string amount { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
    }

    public class ShiftTime
    {
        [JsonProperty("collectionType")]
        public string collection_type { get; set; }
        [JsonProperty("mStartTime")]
        public string m_start_time { get; set; }
        [JsonProperty("eStartTime")]
        public string e_start_time { get; set; }
        [JsonProperty("mLockTime")]
        public string m_lock_time { get; set; }
        [JsonProperty("eLockTime")]
        public string e_lock_time { get; set; }
        [JsonProperty("dateShiftEnable")]
        public string date_shift_enable { get; set; }
        [JsonProperty("graceHr")]
        public string grace_hr { get; set; }
    }  
}
