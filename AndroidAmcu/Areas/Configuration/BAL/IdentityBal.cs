using AndroidAmcu.Areas.Configuration.Models;
using AndroidAmcu.Areas.General.Models;
using Framework.BAL;
using Framework.CustomDataType;
using Framework.DataAccess.Dapper;
using Framework.Extension;
using Framework.Library.Helper;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace AndroidAmcu.Areas.Configuration.BAL
{

    public class IdentityBal : BaseBal
    {
        private QueryParam _query;
        private List<ModelParameter> _data;
        private RequestFormat1 _request;
        private Dictionary<string, List<string>> Hierarchy;
        public IdentityBal()
        {
            DBRepository Sqlite = new DBRepository(FileHelper.FileServerPath("Identity/Struct/everest_amcs.db"));
        }
        public IdentityBal(RequestFormat1 request)
        {
            _request = request;            
        }

        public IActionResult Registration()
        {
            Bmc BmcModel = NewRepo.FindByKey<Bmc>(_request.organizationCode);
            dynamic data = new ExpandoObject();
            if (BmcModel != null)
            {
                if (_request.content.mobileNo != null && _request.content.mobileNo.Trim() == BmcModel.contact_no.Trim())
                {
                    _data = new List<ModelParameter>();
                    AndroidInstallationDetails DetailModel;
                    AndroidInstallation InstalationModel = NewRepo.FindByColumn<AndroidInstallation>(new List<ConditionParameter>
                    {
                         Condition("organization_code",BmcModel.bmc_code),
                         Condition("organization_type","bmc"),
                    });

                    if (InstalationModel == null)
                    {
                        InstalationModel = new AndroidInstallation
                        {
                            android_installation_id = DbHelper.UniqueKey(),
                            organization_code = BmcModel.bmc_code,
                            organization_type = "bmc",
                        };
                        DetailModel = SetDetail(InstalationModel.android_installation_id);
                        _data.Add(new ModelParameter { ValidateModel = null, SaveModel = InstalationModel });
                    }
                    else
                    {
                        DetailModel = NewRepo.FindByColumn<AndroidInstallationDetails>(new List<ConditionParameter>
                        {
                           Condition("android_installation_id",InstalationModel.android_installation_id),
                           Condition("device_id",_request.deviceId),
                           Condition("mobile_no",BmcModel.contact_no.Trim()),
                           Condition("is_active",1)
                        });
                        if (DetailModel == null)
                        {
                            DetailModel = SetDetail(InstalationModel.android_installation_id);
                        }
                        else
                        {
                            DetailModel.model_operation = "update";
                        }
                    }                    
                    DetailModel.hash_key = DbHelper.UniqueKey(20);
                    DetailModel.otp_code = 1234;
                    DetailModel.is_active = false;
                    DetailModel.is_expired = false;
                    _data.Add(new ModelParameter { ValidateModel = null, SaveModel = DetailModel });
                    CustomResult result=AUDOperation(_data);
                    if (result._result.message.ToLower() == "success")
                    {
                        
                        data.token = DetailModel.hash_key;
                        return new CustomResult2(data);
                    }
                    return new CustomResult2(data, "Data Not Available");
                }
                else
                {
                    return new CustomResult2(data, "Data Not Available");
                }
            }
            else
            {
                return new CustomResult2(data, "Data Not Available");
            }
        }

        public IActionResult Verification()
        {
            dynamic data = new ExpandoObject();
            AndroidInstallationDetails DetailModel = NewRepo.FindByColumn<AndroidInstallationDetails>(new List<ConditionParameter>{
                Condition("hash_key",_request.token),
                Condition("otp_code",_request.content.otpCode),
                Condition("imei_no",_request.imei),
            });
            if (DetailModel == null)
            {
                return new CustomResult2(data, "OTP Not Verified.");
            }
            else
            {
                _data = new List<ModelParameter>();
                _query = new QueryParam
                {
                    Where = new List<ConditionParameter>
                    {
                        Condition("android_installation_id",DetailModel.android_installation_id),
                        Condition("android_installation_details_id",DetailModel.android_installation_details_id,"!=")
                    }
                };
                List<AndroidInstallationDetails> DetailList = NewRepo.FindAll<AndroidInstallationDetails>(_query).ToList();
                foreach(AndroidInstallationDetails UdateDetailModel in DetailList.NotEmpty())
                {
                    UdateDetailModel.sync_active = false;
                    UdateDetailModel.model_operation = "update";
                    _data.Add(new ModelParameter { ValidateModel = null, SaveModel = UdateDetailModel });
                }
                DetailModel.is_active = true;
                DetailModel.sync_key = NumericHelper.RandomNumber().ToString();
                DetailModel.sync_active = true;
                DetailModel.model_operation = "update";
                _data.Add(new ModelParameter { ValidateModel = null, SaveModel = DetailModel });
                CustomResult result = AUDOperation(_data);
                if (result._result.message.ToLower() == "success")
                {
                    _query = new QueryParam
                    {
                        Fields= "tbl_mcc_plant.mcc_plant_code,tbl_mcc_plant.name",
                        Join = new List<JoinParameter>
                        {
                            new JoinParameter{table="tbl_bmc",condition="tbl_bmc.mcc_plant_code=tbl_mcc_plant.mcc_plant_code"}
                        },
                        Where = new List<ConditionParameter>
                        {
                            Condition("bmc_code",_request.organizationCode)
                        }
                    };
                    MccPlant MccModel = NewRepo.Find<MccPlant>(_query);
                    if (MccModel == null)
                    {
                        return new CustomResult2(data, "OTP Not Verified.");
                    }                    
                    data.message = "OTP Verified";
                    data.syncKey = DetailModel.sync_key;
                    data.parentType = "MCC";
                    data.parentCode = MccModel.mcc_plant_code;
                    data.parentName = MccModel.name;
                    return new CustomResult2(data);
                }
                return new CustomResult2(data, "OTP Not Verified.");
            }
        }
        public IActionResult Generate(string host)
        {
            AndroidInstallationDetails DetailModel = NewRepo.FindByColumn<AndroidInstallationDetails>(new List<ConditionParameter> { Condition("hash_key",_request.token)});
            if (DetailModel!=null)
            {
                string NewFileName = FileHelper.NewFileName(FileHelper.FileServerPath("Identity/Data/"),$"{_request.organizationType}_{_request.organizationCode}_", "db");
                FileHelper.Copy(FileHelper.FileServerPath("Identity/Struct/everest_amcs.db"), NewFileName);
                DetailModel.db_path = host+"/"+NewFileName.Replace(FileHelper.ProjectPath(),"");
                DetailModel.model_operation = "update";
                _data = new List<ModelParameter> { new ModelParameter { ValidateModel = null, SaveModel = DetailModel } };
                AUDOperation(_data);

                DBRepository Sqlite = new DBRepository(NewFileName);
                _query = new QueryParam
                {
                    DirectQuery = $"SELECT tbl_name FROM sqlite_master WHERE type='table'"
                };
                List<string> SqliteTable = Sqlite.FindAll<string>(_query).ToList();
                List<TableList> AllTableList = NewRepo.FindAll<TableList>(new QueryParam {Where=new List<ConditionParameter> { Condition("is_offline",0)} }).ToList();               
                OrgDetail();
                foreach (TableList tables in AllTableList)
                {
                    if (SqliteTable.Contains(tables.table_name.Trim()))
                    {
                        _query = new QueryParam
                        {
                            DirectQuery = $"PRAGMA table_info({tables.table_name.Trim()})"
                        };
                        List<TableInfo> TableInfoDetail= Sqlite.FindAll<TableInfo>(_query).ToList();
                        string fields = string.Join(",",TableInfoDetail.Select(x => x.name ).ToArray());
                        _query = new QueryParam
                        {
                            Fields = fields,
                            Distinct = "distinct",
                            Table = tables.table_name.Trim(),
                        };
                        if(tables.key_field!=null && tables.key_field.Trim() != "")
                        {
                            string comapre_field = tables.key_field== "to_dest"?"bmc_code": tables.key_field;                            
                            _query.Where = new List<ConditionParameter>
                            {
                                Condition(tables.key_field,Hierarchy[comapre_field],"in")
                            };
                        }
                        
                        IEnumerable<object> ObjectResult = NewRepo.FindAll(_query);
                        int i = 0;
                        string DirectQuery = "";
                        foreach (var ObjTmp in ObjectResult)
                        {
                            IDictionary<string, object> Tmp = ObjTmp as IDictionary<string, object>;
                            if (i == 0)
                            {
                                DirectQuery = $"insert into {tables.table_name} ({fields}) values (";
                            }
                            if (i == 200)
                            {
                                DirectQuery = DirectQuery.TrimEnd('(');
                                DirectQuery = DirectQuery.TrimEnd(',');
                                Sqlite.Add(new QueryParam { DirectQuery = DirectQuery });
                                i = 0;
                            }
                            else
                            {
                                DirectQuery += string.Join(",", Tmp.Select(x => x.Value == null ? "NULL" : $"'{x.Value.ToString()}'")) + "),(";
                                i++;
                            }                                                       
                        }
                        if (DirectQuery != "")
                        {
                            DirectQuery = DirectQuery.TrimEnd('(');
                            DirectQuery = DirectQuery.TrimEnd(',');
                            Sqlite.Add(new QueryParam { DirectQuery = DirectQuery });
                        }
                    }
                }                
                dynamic data = new ExpandoObject();
                data.dbPath = DetailModel.db_path;                
                return new CustomResult2(data);
            }
            return new CustomResult2(null, "Authentication Failed.");
        }

        private AndroidInstallationDetails SetDetail(string id)
        {
            return new AndroidInstallationDetails
            {
                android_installation_id=id,
                device_id=_request.deviceId,
                imei_no= _request.imei,
                mobile_no= _request.content.mobileNo,
                version_no= _request.versionNo
            };
        }

        private void OrgDetail()
        {
            Hierarchy = new Dictionary<string, List<string>>();
            List<Dcs> OrgData = NewRepo.FindAll<Dcs>(new QueryParam {
                Fields= "union_code,plant_code,mcc_plant_code,bmc_code,dcs_code",
                Where=new List<ConditionParameter>
                {
                    Condition("bmc_code",_request.organizationCode)
                }
            }).ToList();
            if (OrgData == null)
            {
                Bmc BmcData = NewRepo.FindByKey<Bmc>(_request.organizationCode);
                Hierarchy.Add("union_code", new List<string> { BmcData.union_code });
                Hierarchy.Add("plant_code", new List<string> { BmcData.plant_code});
                Hierarchy.Add("mcc_plant_code", new List<string> { BmcData.mcc_plant_code });
                Hierarchy.Add("bmc_code", new List<string> { _request.organizationCode });
                Hierarchy.Add("dcs_code", new List<string> {""});
            }
            else
            {
                Hierarchy.Add("union_code", new List<string> { OrgData.Select(x => x.union_code).FirstOrDefault() });
                Hierarchy.Add("plant_code", new List<string> { OrgData.Select(x => x.plant_code).FirstOrDefault() });
                Hierarchy.Add("mcc_plant_code", new List<string> { OrgData.Select(x => x.mcc_plant_code).FirstOrDefault() });
                Hierarchy.Add("bmc_code", new List<string> { _request.organizationCode });
                Hierarchy.Add("dcs_code", OrgData.Select(x => x.dcs_code).ToList());
            }              
        }
    }
}
