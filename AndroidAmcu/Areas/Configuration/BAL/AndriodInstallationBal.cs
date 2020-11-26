using AndroidAmcu.Areas.General.Models;
using Framework.BAL;
using Framework.CustomDataType;
using System.Collections.Generic;

namespace AndroidAmcu.Areas.Configuration.BAL
{
    public class AndriodInstallationBal : BaseBal
    {
        public int CheckInstallation(string token,string deviceid,string code,string type)
        {            
            int status = NewRepo.Find<int>(new QueryParam
            {
                Fields = "count(tbl_android_installation.android_installation_id)",
                Table = "tbl_android_installation_details",
                Join = new List<JoinParameter>
                {
                    new JoinParameter{table="tbl_android_installation",condition="tbl_android_installation.android_installation_id=tbl_android_installation_details.android_installation_id"},
                },
                Where = new List<ConditionParameter> {
                    Condition("hash_key",token),
                    Condition("tbl_android_installation_details.is_active",1),
                    Condition("is_expired",0),
                    Condition("device_id",deviceid),
                    Condition("tbl_android_installation.organization_code",code),
                    Condition("organization_type",type)
                }
            });
            return status;
        }
    }
}
