using Framework.BAL;
using Framework.CustomDataType;
using Models;
using System.Collections.Generic;
using System.Linq;

namespace AndroidAmcu.Areas.Configuration.BAL
{
    public class OrgBal : BaseBal
    {
        protected Dictionary<string, List<string>> Hierarchy;
        protected void OrgDetail(string code,string type="BMC")
        {
            Hierarchy = new Dictionary<string, List<string>>();
            if (type == "BMC")
            {
                List<Dcs> OrgData = NewRepo.FindAll<Dcs>(new QueryParam
                {
                    Fields = "union_code,plant_code,mcc_plant_code,bmc_code,dcs_code",
                    Where = new List<ConditionParameter>
                    {
                        Condition("bmc_code",code)
                    }
                }).ToList();

                if (OrgData == null || OrgData.Count() == 0)
                {
                    Bmc BmcData = NewRepo.FindByKey<Bmc>(code);
                    Hierarchy.Add("union_code", new List<string> { BmcData.union_code });
                    Hierarchy.Add("plant_code", new List<string> { BmcData.plant_code });
                    Hierarchy.Add("mcc_plant_code", new List<string> { BmcData.mcc_plant_code });
                    Hierarchy.Add("bmc_code", new List<string> { code });
                    Hierarchy.Add("dcs_code", new List<string> { "" });
                }
                else
                {
                    Hierarchy.Add("union_code", new List<string> { OrgData.Select(x => x.union_code).FirstOrDefault() });
                    Hierarchy.Add("plant_code", new List<string> { OrgData.Select(x => x.plant_code).FirstOrDefault() });
                    Hierarchy.Add("mcc_plant_code", new List<string> { OrgData.Select(x => x.mcc_plant_code).FirstOrDefault() });
                    Hierarchy.Add("bmc_code", new List<string> { code });
                    Hierarchy.Add("dcs_code", OrgData.Select(x => x.dcs_code).ToList());
                }
            }
            else if (type=="VLC")
            {
                Dcs DcsData = NewRepo.FindByKey<Dcs>(code);
                Hierarchy.Add("union_code", new List<string> { DcsData.union_code });
                Hierarchy.Add("plant_code", new List<string> { DcsData.plant_code });
                Hierarchy.Add("mcc_plant_code", new List<string> { DcsData.mcc_plant_code });
                Hierarchy.Add("bmc_code", new List<string> { DcsData.bmc_code });
                Hierarchy.Add("dcs_code", new List<string> { code});
            }


            
        }
    }
}
