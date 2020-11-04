using Framework.BAL;
using Framework.CustomDataType;
using Framework.Library.Helper;
using Framework.Models;
using Models;
using System.Collections.Generic;
using Framework.DataAccess.Dapper;
using Framework.Extension;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Dynamic;
using System.Linq;
using Framework.DataAccess.Redis;

namespace DataExchange.Areas.Service.BAL
{
    public class UsersBal : BaseBal<BaseModel>
    {
        private List<ModelParameter> Data;
        private Users _user;
        private QueryParam _query;
        private QueryParam Query;

        public UsersBal(Users user, bool dd)
        {
            _user = new Users();
            _user = user;
        }
        public UsersBal()
        {
            Data = new List<ModelParameter>();
        }
        public dynamic MobileLogin(Users userModel)
        {


            Users UpdateUser = NewRepo.FindByColumn<Users>(new List<ConditionParameter>
                {
                    Condition("user_code",userModel.user_code),
                    Condition("otp_code",userModel.otp_code),
                });
            if (UpdateUser != null)
            {
                Data = new List<ModelParameter>();
                UpdateUser.is_active = true;
                UpdateUser.token = DbHelper.UniqueKey();
                UpdateUser.device_token = userModel.device_token;
                Data.Add(new ModelParameter { ValidateModel = new UsersValidator(), SaveModel = UpdateUser });
                CustomResult customResult = UpdateData(Data);
                if (customResult._result.code == 200)
                {
                    CacheRepository _cache = new CacheRepository();
                    Query = new QueryParam()
                    {
                        Table = "user_organization_mapping",
                        Fields = "org_type,company_code,plant_code,mcc_code,bmc_code,mpp_code",
                        Where = new List<ConditionParameter> {
                        Condition("user_code",UpdateUser.user_code)
                    }
                    };
                    List<UserOrganizationMapping> _UserOrganizationMappingList = NewRepo.FindAll<UserOrganizationMapping>(Query).ToList();
                    if (_UserOrganizationMappingList == null || _UserOrganizationMappingList.Count == 0)
                    {
                        return new CustomResult("error", "authentication_fail");
                    }
                    Query = new QueryParam()
                    {
                        Table = "user_role",
                        Fields = "role_code",
                        Where = new List<ConditionParameter> {
                        Condition("user_code",UpdateUser.user_code)
                    }
                    };
                    TmpUserToken _TmpUserToken = new TmpUserToken
                    {
                        username = UpdateUser.user_name,
                        user_code = UpdateUser.user_code,
                        id = UpdateUser.token,
                        role = NewRepo.FindAll<string>(Query).ToList(),
                        usertype = _UserOrganizationMappingList.FirstOrDefault().org_type,
                        company_code = _UserOrganizationMappingList.Select(o => o.company_code).Distinct().ToList(),
                        plant_code = _UserOrganizationMappingList.Select(o => o.plant_code).Distinct().ToList(),
                        mcc_code = _UserOrganizationMappingList.Select(o => o.mcc_code).Distinct().ToList(),
                        bmc_code = _UserOrganizationMappingList.Select(o => o.bmc_code).Distinct().ToList(),
                        mpp_code = _UserOrganizationMappingList.Select(o => o.mpp_code).Distinct().ToList(),



                        member_code = UpdateUser.reference_code,

                        exp = DateTime.Now.AddHours(6)

                    };
                    //if (UpdateUser.user_type == "member")
                    //{
                    //    Query = new QueryParam()
                    //    {
                    //        Table = "member",
                    //        Fields = "concat(mpp_name,'-',member_name)",
                    //        Join = new List<JoinParameter>()
                    //        {
                    //            new JoinParameter{table="mpp",condition="mpp.mpp_code=member.mpp_code"},
                    //        },
                    //        Where = new List<ConditionParameter> {
                    //            Condition("member_code",_TmpUserToken.member_code)
                    //        }
                    //    };
                    //    _TmpUserToken.display_data = NewRepo.Find<string>(Query);
                    //}
                    Query = new QueryParam()
                    {
                        Table = "actions",
                        Fields = "distinct service_url",
                        Join = new List<JoinParameter>()
                        {
                            new JoinParameter{table="permission_action",condition="permission_action.action_code=actions.action_code"},
                            new JoinParameter{table="role_permission",condition="role_permission.permission_code=permission_action.permission_code"}
                        },
                        Where = new List<ConditionParameter> {
                            Condition("role_code",_TmpUserToken.role.ToArray(),"in")
                        }
                    };
                    _TmpUserToken.actions = NewRepo.FindAll<string>(Query).ToList();
                    FreeAction _freeAction = _cache.GetData<FreeAction>("FreeActions");

                    if (_freeAction == null || (DateTime.Now.Subtract(_freeAction.created_at).Hours >= 24))
                    {
                        _freeAction = new FreeAction();
                        Query = new QueryParam()
                        {
                            Table = typeof(Actions).GetTableName(),
                            Fields = "distinct service_url",
                            Where = new List<ConditionParameter> {
                        Condition("is_free",true),
                        new ConditionParameter{direct_condition="service_url is not null",PropertyValue="#$#",PropertyName="service_url" } }
                        };
                        _freeAction.actions = NewRepo.FindAll<string>(Query).ToList();
                        _freeAction.created_at = DateTime.Now;
                        _cache.SaveData(_freeAction, "FreeActions");
                    }
                    _cache.SaveData(_TmpUserToken, UpdateUser.token);

                    UserToken ut = new UserToken
                    {
                        user_code = UpdateUser.user_code,
                        login_time = DateTime.Now,
                        token = UpdateUser.token,
                        is_active = true
                    };

                    Data = new List<ModelParameter>()
                    {
                        new ModelParameter{ValidateModel=new UserTokenValidator(),SaveModel=ut}
                    };
                    this.SaveData(Data);
                    return UpdateUser.token;
                }
                return customResult;
            }
            return new CustomResult("error", "authfail");
        }
    }
}
