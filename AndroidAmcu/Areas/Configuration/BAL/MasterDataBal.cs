using AndroidAmcu.Areas.Configuration.Models;
using AndroidAmcu.Areas.General.Models;
using Framework.BAL;
using Framework.CustomDataType;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Dynamic;

namespace AndroidAmcu.Areas.Configuration.BAL
{
    public class MasterDataBal : BaseBal
    {
        public JObject PrepareData(RequestFormat1 request)
        {
            SearchParam Param = new SearchParam()
            {
                Param=new List<ConditionParameter>
                {
                    Condition("dest_org_id",request.organizationCode),
                    Condition("dest_org_type",request.organizationType),
                    Condition("device_id",request.deviceId),                    
                },
                //Offset=1,
                //Limit=5
            };
            dynamic data = new ExpandoObject();
            data.data = Param;
            return JObject.FromObject(data);
        }
        
    }
}
