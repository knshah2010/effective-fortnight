using AndroidAmcu.Areas.Configuration.Models;
using AndroidAmcu.Areas.General.Models;
using Framework.BAL;
using Framework.CustomDataType;
using Framework.Extension;
using Framework.Library.Helper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Dynamic;

namespace AndroidAmcu.Areas.Configuration.BAL
{
    public class MasterDataBal : BaseBal
    {
        private List<ModelParameter> _data;
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
        public IActionResult SaveAck(Acknowledgemen _requst)
        {
            string message = "Sentbox Not Updated.";
            dynamic data = new ExpandoObject();
            string status = NewRepo.Find<string>(new QueryParam
            {
                Fields= "tbl_android_installation.android_installation_id",
                Table = "tbl_android_installation_details",
                Join = new List<JoinParameter>
                {
                    new JoinParameter{table="tbl_android_installation",condition="tbl_android_installation.android_installation_id=tbl_android_installation_details.android_installation_id"},
                },
                Where = new List<ConditionParameter> {
                    Condition("hash_key",_requst.token),
                    Condition("tbl_android_installation_details.is_active",1),
                    Condition("is_expired",0),
                    Condition("device_id",_requst.deviceId),
                    Condition("tbl_android_installation.organization_code",_requst.organizationCode),
                    Condition("organization_type",_requst.organizationType)
                }
            });
            if (status.Trim() != "")
            {
                bool flag=NewRepo.Delete(new List<QueryParam> {
                    new QueryParam
                    {
                        Table="tbl_sentbox",
                        Where=new List<ConditionParameter>
                        {
                            Condition("uuid",_requst.content.uuid.ToArray())
                        }
                    },
                    new QueryParam
                    {
                        Table="tbl_sentbox_clone",
                        Where=new List<ConditionParameter>
                        {
                            Condition("uuid",_requst.content.uuid.ToArray())
                        }
                    }
                });
                if (flag)
                {
                    message = "Sentbox Updated Successfully.";
                }
            }
            data.message = message;
            return new CustomResult2(data);
        }
        public IActionResult SaveInbox(RequestFormat3 _requst)
        {
            List<string> SuccessId = new List<string>();
            List<string> ErrorId = new List<string>();
            string message = "Unable to save!";
            dynamic data = new ExpandoObject();
            foreach (Inbox _inbox in _requst.content.NotEmpty())
            {
                message = "Successfully Saved!";
                _data = new List<ModelParameter>();
                Inbox InboxModel = NewRepo.FindByColumn<Inbox>(new List<ConditionParameter> { Condition("uuid", _inbox.uuid) });
                if (InboxModel == null)
                {
                    _data.Add(new ModelParameter() {ValidateModel=null,SaveModel=_inbox });
                    string status = SingleRecordTransaction(_data);
                    if (status.ToLower().Trim() == "success")
                    {
                        SuccessId.Add(_inbox.uuid);
                    }
                    else
                    {
                        ErrorId.Add(_inbox.uuid);
                    }
                }
                else
                {
                    SuccessId.Add(_inbox.uuid);
                }
            }
            data.successId = string.Join(',', SuccessId);
            data.errorId = string.Join(',', ErrorId);
            return new CustomResult2(data, message);
        }
    }
}
