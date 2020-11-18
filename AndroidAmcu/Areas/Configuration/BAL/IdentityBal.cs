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
                    if (result._result.message == "success")
                    {
                        dynamic data = new ExpandoObject();
                        data.token = DetailModel.hash_key;
                        return new CustomResult2(data);
                    }
                    return new CustomResult2(null, "Data Not Available");
                }
                else
                {
                    return new CustomResult2(null, "Data Not Available");
                }
            }
            else
            {
                return new CustomResult2(null, "Data Not Available");
            }
        }

        public IActionResult Verification()
        {
            AndroidInstallationDetails DetailModel = NewRepo.FindByColumn<AndroidInstallationDetails>(new List<ConditionParameter>{
                Condition("token",_request.token),
                Condition("otp_code",_request.content.otpCode),
                Condition("imei",_request.imei),
            });
            if (DetailModel == null)
            {
                return new CustomResult2(null, "OTP Not Verified.");
            }
            else
            {
                _data = new List<ModelParameter>();
                _query = new QueryParam
                {
                    Where = new List<ConditionParameter>
                    {
                        Condition("android_installation_id",DetailModel.android_installation_id)
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
                if (result._result.message == "success")
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
                        return new CustomResult2(null, "OTP Not Verified.");
                    }
                    dynamic data = new ExpandoObject();
                    data.message = "OTP Verified";
                    data.syncKey = DetailModel.sync_key;
                    data.parentType = "MCC";
                    data.parentCode = MccModel.mcc_plant_code;
                    data.parentName = MccModel.name;
                    return new CustomResult2(data);
                }
                return new CustomResult2(null, "OTP Not Verified.");
            }
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
    }
}
