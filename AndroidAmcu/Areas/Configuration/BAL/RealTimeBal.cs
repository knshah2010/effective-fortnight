using AndroidAmcu.Areas.Configuration.Models;
using AndroidAmcu.Areas.General.Models;
using Framework.CustomDataType;
using Framework.Extension;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace AndroidAmcu.Areas.Configuration.BAL
{
    public class RealTimeBal : OrgBal
    {       
        private RequestFormat1 _request;
        public RealTimeBal(){}
        public RealTimeBal(RequestFormat1 request)
        {
            _request = request;
        }
        public JObject PrepareData(RequestFormat1 request)
        {
            SearchParam Param = new SearchParam()
            {
                Param = new List<ConditionParameter>
                {
                    Condition("p_dcs_code",request.content.dcsCode),                    
                }                
            };
            dynamic data = new ExpandoObject();
            data.data = Param;
            return JObject.FromObject(data);
        }
        public void GetRate()
        {
            bool isMultiApplicabilty = (_request.content.rateType.Trim() == "") ? false : true;
            string rate_table = "tbl_purchase_rate", base_table = "tbl_purchase_rate_based",app_table= "tbl_purchase_rate_applicability",op="=";
            if (_request.content.rateType.Trim() == "BMC")
            {
                rate_table = "tbl_dcs_purchase_rate"; base_table = "tbl_dcs_purchase_rate_based"; app_table = "tbl_dcs_purchase_rate_applicability"; op = "!=";
            }            
            dynamic data = new ExpandoObject();
            data.purchaseRate = NewRepo.Find<TmpPurchaseRate>(new QueryParam { 
                Table= rate_table,
                Where=new List<ConditionParameter> { Condition("purchase_rate_code ", _request.content.purchaseRateCode)}
            });
            data.purchaseRateBased= NewRepo.FindAll<TmpPurchaseRateBase>(new QueryParam
            {
                Table = base_table,
                Where = new List<ConditionParameter> { Condition("purchase_rate_code ", _request.content.purchaseRateCode) }
            });
            if (isMultiApplicabilty)
            {
                data.purchaseRateApplicability = null;
                OrgDetail(_request.organizationCode);
                QueryParam query = new QueryParam
                {
                    Table = app_table,
                    Join = new List<JoinParameter>
                    {
                        new JoinParameter{type="left",table=$"tbl_rate_download_ack",condition=$"tbl_rate_download_ack.rate_app_code={app_table}.rate_app_code  AND tbl_rate_download_ack.device_id='{_request.deviceId}' AND tbl_rate_download_ack.hash_key='{_request.token}' AND tbl_rate_download_ack.applicable_for{op}'MEMBER'"}
                    },
                    Where=new List<ConditionParameter>
                    {
                        Condition($"{app_table}.purchase_rate_code",_request.content.purchaseRateCode),
                        new ConditionParameter{direct_condition="tbl_rate_download_ack.ack_id is null"}
                    }
                };
                if (_request.content.rateType.Trim() == "BMC")
                {
                    string customer_code = NewRepo.Find<string>(new QueryParam {Fields= "customer_code", Table= "tbl_customer_master", Where=new List<ConditionParameter> { Condition("bmc_code",Hierarchy["bmc_code"])} });
                    string customer_type = NewRepo.Find<string>(new QueryParam { Fields = "customer_type", Table = "tbl_customer_type", Where = new List<ConditionParameter> { Condition("is_organisation",0) } });  
                    query.Where.Add(new ConditionParameter
                    {
                        direct_condition = $"(( applicable_code='{Hierarchy["plant_code"]}' and applicable_for='PLANT') or ( applicable_code='{Hierarchy["mcc_plant_code"]}' and applicable_for='MCC') or ( applicable_code='{Hierarchy["bmc_code"]}' and applicable_for='BMC') or ( applicable_code='{Hierarchy["dcs_code"]}' and applicable_for='DCS') or ( applicable_code='{customer_code}' and applicable_for='{customer_type}'))"
                    }); ;
                }
                else
                {
                    query.Where.Add(Condition("tbl_purchase_rate_applicability.dcs_code",Hierarchy["dcs_code"]));
                }
                data.purchaseRateApplicabilityMultiple = NewRepo.FindAll<TmpRateApplicability>(query);
            }
            else
            {
                data.purchaseRateApplicabilityMultiple = new List<string>();
                if (_request.content.rateType.Trim() == "BMC")
                    data.purchaseRateApplicability = null;
                else
                {
                    data.purchaseRateApplicability = NewRepo.Find<TmpRateApplicability>(new QueryParam
                    {
                        Table= "tbl_purchase_rate_applicability",
                        Fields="*",
                        Where=new List<ConditionParameter>
                        {
                            Condition("purchase_rate_code",_request.content.purchaseRateCode),
                            Condition("dcs_code",Hierarchy["dcs_code"])
                        },
                        OrderBy="wef_date desc"
                    });
                }
            }

        }
        public IActionResult GetRateDetail()
        {
           
            if (_request.content.rateType.Trim() == "")
                _request.content.rateType = "MEMBER";
            if (_request.content.rateClass.Trim() == "")
                _request.content.rateClass = "0";
            string[] returnList = NewRepo.FindAll<string>(new QueryParam { Sp = "proc_purchase_rate_detail" ,
                Where=new List<ConditionParameter> { 
                    Condition("p_purchase_rate_code",_request.content.purchaseRateCode),
                    Condition("p_milk_quality_type_code",_request.content.milkQualityTypeCode),
                    Condition("p_milk_type_code",_request.content.milkTypeCode),
                    Condition("p_rate_type",_request.content.rateType),
                    Condition("p_rate_class",_request.content.rateClass),
                }
            }).ToArray();
            if (returnList.Count() <= 0)
            {
                dynamic data = new ExpandoObject();
                return new CustomResult2(data);
            }
            else
            {
                dynamic data = returnList;
                return new CustomResult2(data);
            }            
        }
        public IActionResult UpdateAck()
        {
            dynamic data = new ExpandoObject();
            data.message = "Acknowledgement Updated.";
            if (_request.content.ackType.Trim() == "RATE")
            {
                NewRepo.Update(new List<QueryParam> { 
                    new QueryParam
                    {
                        Table="tbl_purchase_rate_applicability",
                        Fields=$"updated_at=now(),download_date_time=now(),is_download=0",
                        Where=new List<ConditionParameter>
                        {
                            Condition("purchase_rate_code",_request.content.purchaseRateCode),
                            Condition("dcs_code",_request.content.dcsCode)
                        }
                    },
                    new QueryParam
                    {
                        Table="tbl_dcs",
                        Fields=$"updated_at=now(),rate_flag=0,member_rate_code=NULL",
                        Where=new List<ConditionParameter>
                        {                           
                            Condition("dcs_code",_request.content.dcsCode)
                        }
                    }
                });
            }
            else if(_request.content.ackType.Trim() == "MEMBER")
            {
                NewRepo.Update(
                    new QueryParam
                    {
                        Table= "tbl_dcs",
                        Fields=$"updated_at=now(),is_name_request=0",
                        Where=new List<ConditionParameter>
                        {                            
                            Condition("dcs_code",_request.content.dcsCode)
                        }
                    });
            }
            return new CustomResult2(data);
        }
        public IActionResult UpdateRateAck()
        {
            dynamic data = new ExpandoObject();
            data.message = "Acknowledgement Updated.";
            if(_request.content.rateType.Trim() == "BMC")
            {
                NewRepo.Add(new QueryParam {
                    DirectQuery= $"insert into tbl_rate_download_ack (rate_app_code,purchase_rate_code,wef_date,shift_code,applicable_code,applicable_for,device_id,hash_key,union_code,download_date_time) select rate_app_code,purchase_rate_code,wef_date,shift_code,applicable_code,applicable_for,'{_request.deviceId}','{_request.token}',union_code,now() from tbl_dcs_purchase_rate_applicability where rate_app_code = '{_request.content.rateAppCode}'"
                });
            }
            else if(_request.content.rateType.Trim() == "MEMBER")
            {
                NewRepo.Add(new QueryParam
                {
                    DirectQuery = $"insert into tbl_rate_download_ack (rate_app_code,purchase_rate_code,wef_date,shift_code,applicable_code,applicable_for,device_id,hash_key,union_code,download_date_time) select rate_app_code,purchase_rate_code,wef_date,shift_code,dcs_code,'MEMBER','{_request.deviceId}','{_request.token}',union_code,now() from tbl_purchase_rate_applicability where rate_app_code = '{_request.content.rateAppCode}'"
                });
            }
            return new CustomResult2(data);
        }
       
    }
}
