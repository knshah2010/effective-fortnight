﻿using AndroidAmcu.Areas.Configuration.Models;
using AndroidAmcu.Areas.General.Models;
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

    public class IdentityBal : OrgBal
    {
        private QueryParam _query;
        private List<ModelParameter> _data;
        private RequestFormat1 _request;

        public IdentityBal(RequestFormat1 request)
        {
            _request = request;
        }

        public IActionResult Registration()
        {
            string contact_no = "";
            if (_request.organizationType == "BMC")
            {
                Bmc BmcModel = NewRepo.FindByKey<Bmc>(_request.organizationCode);
                if (BmcModel != null)
                {
                    contact_no = BmcModel.contact_no;
                }

            }
            else if (_request.organizationType == "VLC")
            {
                Dcs DscModel = NewRepo.FindByKey<Dcs>(_request.organizationCode);
                if (DscModel != null)
                {
                    contact_no = DscModel.mobile_no;
                }
            }

            dynamic data = new ExpandoObject();
            if (contact_no.Trim() != "")
            {
                if (_request.content.mobileNo != null && _request.content.mobileNo.Trim() == contact_no.Trim())
                {
                    _data = new List<ModelParameter>();
                    AndroidInstallationDetails DetailModel;
                    AndroidInstallation InstalationModel = NewRepo.FindByColumn<AndroidInstallation>(new List<ConditionParameter>
                    {
                         Condition("organization_code",_request.organizationCode),
                         Condition("organization_type",_request.organizationType),
                    });

                    if (InstalationModel == null)
                    {
                        InstalationModel = new AndroidInstallation
                        {
                            android_installation_id = DbHelper.UniqueKey(),
                            organization_code = _request.organizationCode,
                            organization_type = _request.organizationType,
                            module_code = _request.organizationCode,
                            module_name = _request.organizationType,
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
                           Condition("mobile_no",contact_no.Trim()),
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
                    CustomResult result = AUDOperation(_data);
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
                foreach (AndroidInstallationDetails UdateDetailModel in DetailList.NotEmpty())
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
                    string pcode = "", pname = "", ptype = "";
                    if (_request.organizationType == "BMC")
                    {
                        _query = new QueryParam
                        {
                            Fields = "tbl_mcc_plant.mcc_plant_code,tbl_mcc_plant.name",
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
                        pcode = MccModel.mcc_plant_code;
                        pname = MccModel.name;
                        ptype = "MCC";
                    }
                    else if (_request.organizationType == "VLC")
                    {
                        _query = new QueryParam
                        {
                            Fields = "tbl_bmc.bmc_code,bmc_name",
                            Join = new List<JoinParameter>
                            {
                                new JoinParameter{table="tbl_dcs",condition="tbl_dcs.bmc_code=tbl_bmc.bmc_code"}
                            },
                            Where = new List<ConditionParameter>
                            {
                                Condition("dcs_code",_request.organizationCode)
                            }
                        };
                        Bmc BmcModel = NewRepo.Find<Bmc>(_query);
                        if (BmcModel == null)
                        {
                            return new CustomResult2(data, "OTP Not Verified.");
                        }
                        pcode = BmcModel.bmc_code;
                        pname = BmcModel.bmc_name;
                        ptype = "BMC";
                    }


                    data.message = "OTP Verified";
                    data.syncKey = DetailModel.sync_key;
                    data.parentType = ptype;
                    data.parentCode = pcode;
                    data.parentName = pname;
                    return new CustomResult2(data);
                }
                return new CustomResult2(data, "OTP Not Verified.");
            }
        }
        public IActionResult Generate(string host)
        {
            AndroidInstallationDetails DetailModel = NewRepo.FindByColumn<AndroidInstallationDetails>(new List<ConditionParameter> { Condition("hash_key", _request.token) });
            if (DetailModel != null)
            {
                string NewFileName = FileHelper.NewFileName(FileHelper.FileServerPath("Identity/Data/"), $"{_request.organizationType}_{_request.organizationCode}_", "db");
                FileHelper.Copy(FileHelper.FileServerPath("Identity/Struct/everest_amcs.db"), NewFileName);
                DetailModel.db_path = host + "/" + NewFileName.Replace(FileHelper.ProjectPath(), "");
                DetailModel.model_operation = "update";
                _data = new List<ModelParameter> { new ModelParameter { ValidateModel = null, SaveModel = DetailModel } };
                AUDOperation(_data);

                DBRepository Sqlite = new DBRepository(NewFileName);
                _query = new QueryParam
                {
                    DirectQuery = $"SELECT tbl_name FROM sqlite_master WHERE type='table'"
                };
                List<string> SqliteTable = Sqlite.FindAll<string>(_query).ToList();
                List<TableList> AllTableList = NewRepo.FindAll<TableList>(new QueryParam { Where = new List<ConditionParameter> { Condition("is_offline", 0) } }).ToList();
                OrgDetail(_request.organizationCode,_request.organizationType);
                foreach (TableList tables in AllTableList)
                {
                    if (SqliteTable.Contains(tables.table_name.Trim()))
                    {
                        _query = new QueryParam
                        {
                            DirectQuery = $"PRAGMA table_info({tables.table_name.Trim()})"
                        };
                        List<TableInfo> TableInfoDetail = Sqlite.FindAll<TableInfo>(_query).ToList();
                        string fields = string.Join(",", TableInfoDetail.Select(x => x.name).ToArray());
                        _query = new QueryParam
                        {
                            Fields = fields,
                            Distinct = "distinct",
                            Table = tables.table_name.Trim(),
                        };
                        if (tables.key_field != null && tables.key_field.Trim() != "")
                        {
                            string comapre_field = tables.key_field == "to_dest" ? "bmc_code" : tables.key_field;
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
            bool is_member_rate = true, mcc_bmc_config = true; ;
            List<CollectionIncentiveDeduction> IncetiveList = new List<CollectionIncentiveDeduction>();
            string milktype_table = "", milktype_field = "";
            if (_android.CheckInstallation(_request.token, _request.deviceId, _request.organizationCode, _request.organizationType) > 0)
            {
                OrgDetail(_request.organizationCode,_request.organizationType);
                if (Hierarchy != null)
                {
                    _startup.config = new Dictionary<string, dynamic>();
                    if (_request.organizationType.ToLower().Trim() == "bmc")
                    {
                        milktype_table = "tbl_bmc_milk_type";
                        milktype_field = "bmc_code";

                        _startup.config.Add("collectionBlock", false);
                        _startup.config.Add("dcsBlock", false);
                        _startup.config.Add("dispatchMandate", false);
                        //  _startup.config.Add("", "");
                        Bmc BmcModel = NewRepo.FindByKey<Bmc>(Hierarchy["bmc_code"]);
                        _startup.config.Add("weightManual", BmcModel.is_weight_manual);
                        _startup.config.Add("qualityManual", BmcModel.is_quality_manual);
                    }
                    else if (_request.organizationType.ToLower().Trim() == "vlc")
                    {
                        mcc_bmc_config = false;
                        milktype_table = "tbl_dcs_milk_type";
                        milktype_field = "dcs_code";
                        rateClass rateData = NewRepo.Find<rateClass>(new QueryParam { Sp = "proc_current_rate_detail", Where = new List<ConditionParameter> { Condition("p_dcs_code", _request.organizationCode) } });
                        if (rateData != null)
                        {
                            _startup.rate.mPurchaseRateCode = rateData.mPurchaseRateCode;
                            _startup.rate.ePurchaseRateCode = rateData.ePurchaseRateCode;
                            _startup.rate.mPurchaseRateCodeBlock = rateData.mPurchaseRateCodeBlock;
                            _startup.rate.ePurchaseRateCodeBlock = rateData.ePurchaseRateCodeBlock;
                        }
                        Dcs DcsData = NewRepo.FindByKey<Dcs>(_request.organizationCode);
                        _startup.memberDownload = DcsData.is_name_request;
                        _startup.config.Add("collectionBlock", DcsData.is_active);
                        _startup.config.Add("dcsBlock", DcsData.is_active);
                        _startup.config.Add("dispatchMandate", DcsData.is_dispatch_mandate);
                        _startup.config.Add("weightManual", DcsData.is_weight_manual);
                        _startup.config.Add("qualityManual", DcsData.is_quality_manual);

                        DpuIncentiveMaster DpuConfig = NewRepo.FindByColumn<DpuIncentiveMaster>(new List<ConditionParameter> { Condition("dcs_code", _request.organizationCode) });
                        if (DpuConfig != null)
                        {
                            _startup.collectionConfig.mStartTime = DpuConfig.m_start_time;
                            _startup.collectionConfig.mCutoffTime = DpuConfig.e_cutoff_time;
                            _startup.collectionConfig.mLockTime = DpuConfig.m_lock_time;
                            _startup.collectionConfig.eStartTime = DpuConfig.e_start_time;
                            _startup.collectionConfig.eCutoffTime = DpuConfig.e_cutoff_time;
                            _startup.collectionConfig.eLockTime = DpuConfig.e_lock_time;
                            _startup.collectionConfig.incDeduction = DpuConfig.inc_deduction;
                            _startup.collectionConfig.incRate = DpuConfig.inc_rate;
                        }
                        IncetiveList = NewRepo.FindAll<CollectionIncentiveDeduction>(new QueryParam
                        {
                            Table = "tbl_collection_incentive_deduction",
                            Where = new List<ConditionParameter> { Condition("dcs_code", _request.organizationCode) }
                        }).ToList();

                    }
                    //config object
                    List<UnionConfigResult> _configResult = NewRepo.FindAll<UnionConfigResult>(new QueryParam
                    {
                        Table = "tbl_union_config_result",
                        Fields = "tbl_union_config_result.config_key,tbl_union_config_result.config_result_key",
                        Join = new List<JoinParameter>
                            {
                                new JoinParameter{table="tbl_config",condition="tbl_config.config_code=tbl_union_config_result.config_code"},
                            },
                        Where = new List<ConditionParameter>
                            {
                                Condition("tbl_union_config_result.union_code",Hierarchy["union_code"]),
                                Condition("tbl_config.config_for",_request.organizationType)
                            }
                    }).ToList();

                    foreach (UnionConfigResult result in _configResult)
                    {
                        if (result.config_key == "required_member_rate")
                        {
                            if (result.config_result_key == "0")
                                is_member_rate = false;
                        }
                        TextInfo txtInfo = new CultureInfo("en-us", false).TextInfo;
                        string replace_key = txtInfo.ToTitleCase(result.config_key).Replace("_", string.Empty).Replace(" ", string.Empty);
                        replace_key = $"{replace_key.First().ToString().ToLowerInvariant()}{replace_key.Substring(1)}";
                        _startup.config.Add(replace_key, result.config_result_key);
                    }



                    //$MappedMilkType = $model_data->tblBmcMilkType;
                    List<BmcMilkType> milktypeList = NewRepo.FindAll<BmcMilkType>(new QueryParam
                    {
                        Fields = "milk_type_code,animal_type_name as milk_type_name",
                        Table = milktype_table,
                        Join = new List<JoinParameter>
                            {
                                new JoinParameter{table="tbl_animal_type",condition="animal_type_code=milk_type_code"}
                            },
                        Where = new List<ConditionParameter> { Condition(milktype_field, _request.organizationCode), Condition($"{milktype_table}.is_active", 1) }
                    }).ToList();
                    _startup.collectionConfig.allowedMilkType = new List<allowedMilkTypeClass>();
                    _startup.collectionConfig.milkTypeRate = new List<milkTypeRateClass>();
                    _startup.collectionConfig.collectionIncentiveDeduction = new List<collectionIncentiveDeductionClass>();
                    _startup.collectionConfig.qualityParam = new qualityParamClass();
                    foreach (CollectionIncentiveDeduction ded in IncetiveList.NotEmpty())
                    {
                        _startup.collectionConfig.collectionIncentiveDeduction.Add(new collectionIncentiveDeductionClass
                        {
                            fromDate = ded.from_date,
                            fromTime = ded.from_time,
                            toDate = ded.to_date,
                            toTime = ded.to_time,
                            amount = ded.amount,
                            shiftCode = ded.shift_code,
                            schemeType = ded.scheme_type
                        });
                    }

                    foreach (BmcMilkType _type in milktypeList)
                    {
                        UnionRatechartRange _rateRange = NewRepo.FindByColumn<UnionRatechartRange>(new List<ConditionParameter> { Condition("union_code", Hierarchy["union_code"]), Condition("config_for", _request.organizationType) });
                        _startup.collectionConfig.milkTypeRate.Add(new milkTypeRateClass
                        {
                            milkTypeCode = _type.milk_type_code,
                            milkTypeName = _type.milk_type_name
                        });
                        _startup.collectionConfig.allowedMilkType.Add(new allowedMilkTypeClass
                        {
                            milkTypeCode = _type.milk_type_code,
                            milkTypeName = _type.milk_type_name
                        });
                        if (_rateRange != null)
                        {
                            int key = _startup.collectionConfig.allowedMilkType.Count() - 1;
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

                    quality_param[0] = (_startup.collectionConfig.qualityParam.fat - (_startup.collectionConfig.qualityParam.fat * quality_param[1]) - quality_param[2]) * 4;
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
                    }).ToList();
                    _startup.menuMapping = string.Join(',', _menuList);

                    //member rate
                    if (is_member_rate)
                    {
                        _startup.rate.memberApplicableRate = string.Join(',', NewRepo.FindAll<string>(
                            new QueryParam
                            {
                                Sp = "sp_app_amcu_v2_pending_rate_detail_member",
                                Where = new List<ConditionParameter>{
                                    Condition("p_dcs_code",string.Join(',',Hierarchy["dcs_code"])),
                                    Condition("p_device_id",_request.deviceId),
                                    Condition("p_hash_key",_request.token),
                            }
                            }).ToList());

                    }
                    if (mcc_bmc_config)
                    {
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
                    }


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
            return new CustomResult2(_startup);
        }

        private AndroidInstallationDetails SetDetail(string id)
        {
            return new AndroidInstallationDetails
            {
                android_installation_id = id,
                application_installation_code = id,
                device_id = _request.deviceId,
                imei_no = _request.imei,
                mobile_no = _request.content.mobileNo,
                version_no = _request.versionNo
            };
        }

    }
}
