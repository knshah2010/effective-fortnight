using AndroidAmcu.Areas.Configuration.Models;
using AndroidAmcu.Areas.Configuration.Utility;
using AndroidAmcu.Areas.General.Models;
using Framework.BAL;
using Framework.CustomDataType;
using Framework.DataAccess.Dapper;
using Framework.Extension;
using Framework.Library.Helper;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;

namespace AndroidAmcu.Areas.Configuration.BAL
{

    public class IdentityBal : BaseBal
    {
        private QueryParam _query;
        private List<ModelParameter> _data;
        private RequestFormat1 _request;
        private Dictionary<string, List<string>> Hierarchy;       
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
                         Condition("organization_type","BMC"),
                    });

                    if (InstalationModel == null)
                    {
                        InstalationModel = new AndroidInstallation
                        {
                            android_installation_id = DbHelper.UniqueKey(),
                            organization_code = BmcModel.bmc_code,
                            organization_type = "BMC",
                            module_code= BmcModel.bmc_code,
                            module_name= "BMC",
                        };
                        InstalationModel.application_installation_code = InstalationModel.android_installation_id;
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

        public IActionResult StartUp()
        {
            dynamic data = new ExpandoObject();
            AndriodInstallationBal _android = new AndriodInstallationBal();
            IdentityStartup _startup = new IdentityStartup();
            _startup.collectionConfig = new collectionConfigClass();
            _startup.shift_timing = new List<ShiftTime>();
            _startup.rate = new rateClass();
            bool is_member_rate = true;
            if (_android.CheckInstallation(_request.token, _request.deviceId, _request.organizationCode, _request.organizationType) > 0)
            {
                OrgDetail();
                if (Hierarchy != null)
                {
                    if (_request.organizationType.ToLower().Trim() == "bmc")
                    {
                        //config object
                        List<UnionConfigResult> _configResult = NewRepo.FindAll<UnionConfigResult>(new QueryParam { 
                            Table= "tbl_union_config_result",
                            Fields= "tbl_union_config_result.config_key,tbl_union_config_result.config_result_key",
                            Join=new List<JoinParameter>
                            {
                                new JoinParameter{table="tbl_config",condition="tbl_config.config_code=tbl_union_config_result.config_code"},
                            },
                            Where=new List<ConditionParameter>
                            {
                                Condition("tbl_union_config_result.union_code",Hierarchy["union_code"]),
                                Condition("tbl_config.config_for",_request.organizationType)
                            }
                        }).ToList();
                        _startup.config = new Dictionary<string, dynamic>();
                        foreach(UnionConfigResult result in _configResult)
                        {
                            if(result.config_key== "required_member_rate")
                            {
                                if (result.config_result_key == "0")
                                    is_member_rate = false;
                            }
                            TextInfo txtInfo = new CultureInfo("en-us", false).TextInfo;
                            string replace_key = txtInfo.ToTitleCase(result.config_key).Replace("_", string.Empty).Replace(" ", string.Empty);
                            replace_key = $"{replace_key.First().ToString().ToLowerInvariant()}{replace_key.Substring(1)}";
                            _startup.config.Add(replace_key, result.config_result_key);
                        }
                        _startup.config.Add("collectionBlock", false);
                        _startup.config.Add("dcsBlock", false);
                        _startup.config.Add("dispatchMandate", false);
                       //  _startup.config.Add("", "");



                        //$MappedMilkType = $model_data->tblBmcMilkType;
                        List<BmcMilkType> milktypeList = NewRepo.FindAll<BmcMilkType>(new QueryParam{
                            Fields= "milk_type_code,animal_type_name as milk_type_name",
                            Table = "tbl_bmc_milk_type",
                            Join = new List<JoinParameter>
                            {
                                new JoinParameter{table="tbl_animal_type",condition="animal_type_code=milk_type_code"}
                            },
                            Where = new List<ConditionParameter> { Condition("bmc_code", _request.organizationCode), Condition("tbl_bmc_milk_type.is_active", 1) }
                        }).ToList();
                        _startup.collectionConfig.allowedMilkType = new List<allowedMilkTypeClass>();
                        _startup.collectionConfig.milkTypeRate = new List<milkTypeRateClass>();
                        _startup.collectionConfig.collectionIncentiveDeduction = new List<collectionIncentiveDeductionClass>();
                        _startup.collectionConfig.qualityParam = new qualityParamClass();
                        foreach (BmcMilkType _type in milktypeList)
                        {                            
                            UnionRatechartRange _rateRange = NewRepo.FindByColumn<UnionRatechartRange>(new List<ConditionParameter> { Condition("union_code", Hierarchy["union_code"]), Condition("config_for",_request.organizationType) });
                            _startup.collectionConfig.milkTypeRate.Add(new milkTypeRateClass {
                                milkTypeCode = _type.milk_type_code,
                                milkTypeName = _type.milk_type_name
                            });
                            _startup.collectionConfig.allowedMilkType.Add(new allowedMilkTypeClass { 
                                milkTypeCode=_type.milk_type_code,
                                milkTypeName=_type.milk_type_name
                            });
                            if (_rateRange != null)
                            {
                                int key =_startup.collectionConfig.allowedMilkType.Count() - 1;
                                _startup.collectionConfig.allowedMilkType[key].minFat = _rateRange.min_fat;
                                _startup.collectionConfig.allowedMilkType[key].maxFat = _rateRange.max_fat;
                                _startup.collectionConfig.allowedMilkType[key].minSnf = _rateRange.min_snf;
                                _startup.collectionConfig.allowedMilkType[key].maxSnf = _rateRange.max_snf;
                                _startup.collectionConfig.allowedMilkType[key].minClr = _rateRange.min_clr;
                                _startup.collectionConfig.allowedMilkType[key].maxClr = _rateRange.max_clr;
                            }
                        }
                        //quality param
                        decimal[] quality_param = new decimal[3];                      
                        quality_param[0] = 0.0M;  //clr
                        quality_param[1] = 0.0M;  //ltrclr
                        quality_param[2] = 0.0M;   //ltr2clr

                        MilkCollectionConfig _milkConfig = NewRepo.FindByColumn<MilkCollectionConfig>(new List<ConditionParameter> { Condition("union_code", Hierarchy["union_code"]) });
                        if (_milkConfig != null)
                        {
                            quality_param[1] = _milkConfig.lr1_for_clr;
                            quality_param[2] = _milkConfig.lr2_for_clr;
                        }

                        quality_param[0] = (_startup.collectionConfig.qualityParam.fat - (_startup.collectionConfig.qualityParam.fat * quality_param[1]) - quality_param[2]) *4;
                        quality_param[0] = quality_param[0] < 0 ? 0 : Math.Round(quality_param[0], 1);
                        _startup.collectionConfig.qualityParam.clr = quality_param[0];
                        //menu mapping
                        List<string> _menuList = NewRepo.FindAll<string>(new QueryParam
                        {
                            Fields = "tbl_amcs_app_menu_mapping.action_name",
                            Table = "tbl_amcs_app_menu_mapping",
                            Join = new List<JoinParameter>
                            {
                                new JoinParameter{table="tbl_amcs_app_menu",condition="tbl_amcs_app_menu.action_code=tbl_amcs_app_menu_mapping.action_code"}
                            },
                            Where = new List<ConditionParameter>
                            {
                                Condition("application_type",_request.organizationType),
                                Condition("union_code",Hierarchy["union_code"]),
                                Condition("is_active",1),
                            }
                        }).ToList() ;
                        _startup.menuMapping = string.Join(',', _menuList);

                        //member rate
                        if (is_member_rate)
                        {
                            _startup.rate.memberApplicableRate = string.Join(',', NewRepo.FindAll<string>(
                                new QueryParam { Sp = "sp_app_amcu_v2_pending_rate_detail_member",
                                Where=new List<ConditionParameter>{
                                    Condition("p_dcs_code",string.Join(',',Hierarchy["dcs_code"])),
                                    Condition("p_device_id",_request.deviceId),
                                    Condition("p_hash_key",_request.token),
                                }
                                }).ToList());
                            
                        }
                        _startup.rate.bmcApplicableRate = string.Join(',', NewRepo.FindAll<string>(
                               new QueryParam
                               {
                                   Sp = "sp_app_amcu_v2_pending_rate_detail_bmc",
                                   Where = new List<ConditionParameter>{
                                    Condition("p_plant_code",string.Join(',',Hierarchy["plant_code"])),
                                    Condition("p_mcc_plant_code",string.Join(',',Hierarchy["mcc_plant_code"])),
                                    Condition("p_bmc_code",string.Join(',',Hierarchy["bmc_code"])),
                                    Condition("p_dcs_code",string.Join(',',Hierarchy["dcs_code"])),
                                    Condition("p_device_id",_request.deviceId),
                                    Condition("p_hash_key",_request.token),
                               }
                               }).ToList());

                        //shift time
                        _startup.shift_timing = NewRepo.FindAll<ShiftTime>(
                               new QueryParam
                               {
                                   Sp = "sp_app_amcu_v2_collection_shift_time",
                                   Where = new List<ConditionParameter>{                                   
                                    Condition("p_org_type",_request.organizationType),
                                    Condition("p_org_code",_request.organizationCode),
                               }
                         }).ToList();                        
                    }
                }
            }
            return new CustomResult2(_startup);
        }

        private AndroidInstallationDetails SetDetail(string id)
        {            
            return new AndroidInstallationDetails
            {
                android_installation_id=id,
                application_installation_code=id,
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
            if (OrgData == null || OrgData.Count()==0)
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
