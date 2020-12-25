using DataExchange.Areas.Service.Models;
using DataExchange.Areas.Service.Utility;
using Framework.BAL;
using Framework.CustomDataType;
using Framework.Extension;
using Framework.Library.Helper;
using Framework.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataExchange.Areas.Service.BAL
{
    public class CustomServiceBal : BaseBal<BaseModel>
    {
        private string result = "";
        private List<ModelParameter> Data;
        private List<CustomResponse> _response;

        public CustomServiceBal()
        {
            _response = new List<CustomResponse>();
        }


        public IActionResult Savefarmer(List<Member> MemberList)
        {
            Unions UnionsModel = GetLastRecord<Unions>("tbl_unions");

            foreach (Member MemberModel in MemberList)
            {
                Data = new List<ModelParameter>();
                if (MemberModel.member_unique_code.Trim() != "")
                {
                    if (MemberModel.member_code.Length > 4)
                    {
                        return new CustomResult("success", new CustomResponse { status = "300", msg = "error:member_code:Max Length Should be 4" });
                    }

                    QueryParam _query = new QueryParam
                    {
                        Where = new List<ConditionParameter>
                        {
                            Condition("dcs_code",MemberModel.dcs_code),
                        }
                    };

                    Dcs DcsModel = NewRepo.Find<Dcs>(_query);
                    if (DcsModel != null)
                    {
                        WelcomeParamStationResult _welcome = NewRepo.Find<WelcomeParamStationResult>(new QueryParam
                        {
                            Fields = "welcome_param_station_result.id,welcome_param_id,station_code,param_value",
                            Join = new List<JoinParameter>
                        {
                            new JoinParameter{table="welcome_params",condition="welcome_params.id=welcome_param_id and param_name=\"NameFlag\""},
                            new JoinParameter{table="tbl_dcs",condition="tbl_dcs.ref_code=welcome_param_station_result.station_code"}
                        },
                            Where = new List<ConditionParameter>
                        {
                            Condition("tbl_dcs.dcs_code",MemberModel.dcs_code)
                        }
                        });
                        if (_welcome != null)
                        {
                            _welcome.model_operation = "update";
                            _welcome.param_value = "Y";
                            Data.Add(new ModelParameter { SaveModel = _welcome, ValidateModel = null });
                        }

                        Member NewModel = NewRepo.Find<Member>(new QueryParam { Where = new List<ConditionParameter> { Condition("ref_code", MemberModel.member_unique_code) } });

                        if (NewModel == null)
                        {

                            DcsModel.is_name_request = true;
                            DcsModel.model_operation = "update";
                            Data.Add(new ModelParameter { SaveModel = DcsModel, ValidateModel = new DcsValidator() });

                            MemberModel.ex_member_code = MemberModel.member_code.PadLeft(4, '0');
                            MemberModel.member_code = MemberModel.dcs_code + MemberModel.member_code.PadLeft(4, '0');
                            MemberModel.originating_org_code = UnionsModel.union_code;
                            MemberModel.ref_code = MemberModel.member_unique_code;
                            Data.Add(new ModelParameter { SaveModel = MemberModel, ValidateModel = new MemberValidator() });
                        }
                        else
                        {

                            DcsModel.is_name_request = true;
                            DcsModel.model_operation = "update";
                            Data.Add(new ModelParameter { SaveModel = DcsModel, ValidateModel = new DcsValidator() });

                            NewModel.member_unique_code = MemberModel.member_unique_code;
                            NewModel.member_name = MemberModel.member_name;
                            NewModel.is_active = MemberModel.is_active;
                            NewModel.mobile_no = MemberModel.mobile_no;
                            NewModel.model_operation = "update";
                            Data.Add(new ModelParameter { SaveModel = NewModel, ValidateModel = new MemberValidator() });
                        }
                        SaveData(MemberModel.member_unique_code);
                    }
                    else
                    {
                        CustomerMaster CustomerMasterModel = new CustomerMaster();
                        QueryParam query = new QueryParam
                        {
                            Fields = "code_length",
                            Table= "tbl_customer_type",
                            Where = new List<ConditionParameter>
                            {
                                Condition("customer_type","BULKVEN"),
                            }
                        };
                        CustomerType CustomerTypeModel = NewRepo.Find<CustomerType>(query);
                        if (MemberModel.member_code.Length > CustomerTypeModel.code_length)
                        {
                            return new CustomResult("success", new CustomResponse { status = "300", msg = "error:member_code:Max Length Should be 4" });
                        }

                        Bmc BmcModel = NewRepo.Find<Bmc>(new QueryParam { Where = new List<ConditionParameter> { Condition("bmc_code", MemberModel.dcs_code) } });
                        if (BmcModel != null)
                        {
                            Route RouteModel = NewRepo.Find<Route>(new QueryParam { Where = new List<ConditionParameter> { Condition("to_dest", MemberModel.dcs_code) } });
                            CustomerMaster NewModel = NewRepo.Find<CustomerMaster>(new QueryParam { Where = new List<ConditionParameter> { Condition("ref_code", MemberModel.member_unique_code) } });

                            if (NewModel == null)
                            {
                                CustomerMasterModel.customer_code_ex = "A" + MemberModel.member_code.PadLeft(CustomerTypeModel.code_length, '0');
                                int code = NewRepo.Find<int>(new QueryParam
                                {
                                    DirectQuery = "select max(cast(ifnull((substring(customer_code,length(concat(union_code,bmc_code))+1)),0)as unsigned))  from tbl_customer_master where substring(customer_code,1,length(concat(union_code,bmc_code)))=concat(union_code,bmc_code)",
                                });
                                CustomerMasterModel.customer_code = UnionsModel.union_code + BmcModel.bmc_code + (code + 1);
                                CustomerMasterModel.ref_code = MemberModel.member_unique_code;
                                CustomerMasterModel.customer_unique_code = MemberModel.member_unique_code;
                                CustomerMasterModel.x_col1 = "1#1";
                                CustomerMasterModel.originating_org_code = CustomerMasterModel.union_code = UnionsModel.union_code;
                                CustomerMasterModel.customer_name = MemberModel.member_name;
                                CustomerMasterModel.customer_type = "BULKVEN";
                                CustomerMasterModel.bmc_code = MemberModel.dcs_code;
                                CustomerMasterModel.route_code = RouteModel.route_code;
                                CustomerMasterModel.mobile_no = MemberModel.mobile_no;
                                CustomerMasterModel.is_active = MemberModel.is_active;
                                CustomerMasterModel.x_col5 = MemberModel.rate_class;

                                Data.Add(new ModelParameter { SaveModel = CustomerMasterModel, ValidateModel = new CustomerMasterValidator() });
                            }
                            else
                            {

                                NewModel.customer_unique_code = MemberModel.member_unique_code;
                                NewModel.route_code = RouteModel.route_code;
                                NewModel.customer_name = MemberModel.member_name;
                                NewModel.is_active = MemberModel.is_active;
                                NewModel.mobile_no = MemberModel.mobile_no;
                                NewModel.customer_type = "BULKVEN";
                                NewModel.x_col1 = "1#1";
                                NewModel.x_col5 = MemberModel.rate_class;
                                NewModel.model_operation = "update";
                                Data.Add(new ModelParameter { SaveModel = NewModel, ValidateModel = new CustomerMasterValidator() });
                            }
                            SaveData(MemberModel.member_unique_code);
                        }
                        else
                        {
                            _response.Add(new CustomResponse { status = "300", msg = "error:dcs_code not exist" });
                        }
                    }
                    
                }
                else
                {
                    _response.Add(new CustomResponse { status = "300", msg = "error:member_unique_code:require" });
                }               
            }
            return new CustomResult("success", _response);
        }


        public IActionResult PurchaseRateApplicability(List<PurchaseRateApplicability> PurchaseRateApplicabilityList)
        {
            Unions UnionsModel = GetLastRecord<Unions>("tbl_unions");
            QueryParam Query = new QueryParam
            {
                Fields = "*"
            };
            IEnumerable<Shift> shiftList = NewRepo.FindAll<Shift>(Query);
            int code = NewRepo.Find<int>(new QueryParam { DirectQuery = "select IFNULL(max(rate_app_code),0) from tbl_purchase_rate_applicability" });

            foreach (PurchaseRateApplicability PurchaseRateApplicabilityModel in PurchaseRateApplicabilityList)
            {
                Data = new List<ModelParameter>();

                if (PurchaseRateApplicabilityModel.module_name.ToLower() == "dcs")
                {
                    Dcs DcsModel = NewRepo.Find<Dcs>(new QueryParam { Where = new List<ConditionParameter> { Condition("dcs_code", PurchaseRateApplicabilityModel.module_code) } });
                    if (DcsModel != null)
                    {
                        PurchaseRateApplicabilityModel.rate_app_code = (code + 1).ToString();
                        PurchaseRateApplicabilityModel.ref_code = PurchaseRateApplicabilityModel.applicability_unique_code;
                        PurchaseRateApplicabilityModel.originating_org_code = PurchaseRateApplicabilityModel.union_code = UnionsModel.union_code;
                        PurchaseRateApplicabilityModel.dcs_code = PurchaseRateApplicabilityModel.module_code;
                        PurchaseRateApplicabilityModel.shift_code = shiftList.Where(x => x.short_name.ToLower() == PurchaseRateApplicabilityModel.shift.ToLower()).Select(x => x.id).FirstOrDefault();
                        string time = PurchaseRateApplicabilityModel.wef_date.ToString("yyyy-MM-dd") + " " + shiftList.Where(x => x.short_name.ToLower() == PurchaseRateApplicabilityModel.shift.ToLower()).Select(x => x.shift_time.ToString(@"hh\:mm\:ss")).FirstOrDefault();
                        PurchaseRateApplicabilityModel.wef_date = DateHelper.ParseDate(time);
                        Data.Add(new ModelParameter { SaveModel = PurchaseRateApplicabilityModel, ValidateModel = new PurchaseRateApplicabilityValidator() });
                        SaveData(PurchaseRateApplicabilityModel.applicability_unique_code);

                    }
                    else
                    {
                        Bmc BmcModel = NewRepo.Find<Bmc>(new QueryParam { Where = new List<ConditionParameter> { Condition("bmc_code", PurchaseRateApplicabilityModel.module_code) } });
                        if(BmcModel!=null)
                        {
                            QueryParam _query = new QueryParam
                            {
                                Where = new List<ConditionParameter>
                                {
                                    Condition("bmc_code",PurchaseRateApplicabilityModel.module_code),
                                }
                            };
                            List<CustomerMaster> CustomerMasterList = NewRepo.FindAll<CustomerMaster>(_query).ToList();
                            DcsPurchaseRateApplicability DcsPurchaseRateApplicabilityModel = new DcsPurchaseRateApplicability();

                            foreach (CustomerMaster CustomerMasterModel in CustomerMasterList)
                            {
                                //int code1 = NewRepo.Find<int>(new QueryParam { DirectQuery = "select IFNULL(max(rate_app_code),0) from tbl_dcs_purchase_rate_applicability" });
                                Data = new List<ModelParameter>();

                                DcsPurchaseRateApplicabilityModel.applicability_unique_code = PurchaseRateApplicabilityModel.applicability_unique_code;
                                DcsPurchaseRateApplicabilityModel.is_active = PurchaseRateApplicabilityModel.is_active;
                                string time = PurchaseRateApplicabilityModel.wef_date.ToString("yyyy-MM-dd") + " " + shiftList.Where(x => x.short_name.ToLower() == PurchaseRateApplicabilityModel.shift.ToLower()).Select(x => x.shift_time.ToString(@"hh\:mm\:ss")).FirstOrDefault();
                                DcsPurchaseRateApplicabilityModel.wef_date = DateHelper.ParseDate(time);
                                DcsPurchaseRateApplicabilityModel.purchase_rate_code = PurchaseRateApplicabilityModel.purchase_rate_code + CustomerMasterModel.x_col5;
                                DcsPurchaseRateApplicabilityModel.shift_code = shiftList.Where(x => x.short_name.ToLower() == PurchaseRateApplicabilityModel.shift.ToLower()).Select(x => x.id).FirstOrDefault();
                                DcsPurchaseRateApplicabilityModel.originating_org_code = DcsPurchaseRateApplicabilityModel.union_code = UnionsModel.union_code;
                                DcsPurchaseRateApplicabilityModel.applicable_code = CustomerMasterModel.customer_code;
                                DcsPurchaseRateApplicabilityModel.applicable_for = CustomerMasterModel.customer_type;
                                Data.Add(new ModelParameter { SaveModel = DcsPurchaseRateApplicabilityModel, ValidateModel = new DcsPurchaseRateApplicabilityValidator() });
                                SaveData(PurchaseRateApplicabilityModel.applicability_unique_code);
                            }
                            //SaveData(PurchaseRateApplicabilityModel.applicability_unique_code);
                        }
                        else
                        {
                            _response.Add(new CustomResponse { status = "300", msg = "error:module code not exist" });
                        }
                    }
                    

                }
                else
                {
                    _response.Add(new CustomResponse { status = "300", msg = "error:applicability only for dcs" });
                }
            }
                return new CustomResult("success", _response);
        }

            private T GetLastRecord<T>(string table) where T : BaseModel
        {
            QueryParam Query = new QueryParam
            {
                Fields = "*",
                Table = table,
                OrderBy = "created_at desc"
            };
            return NewRepo.Find<T>(Query);
        }

        private void SaveData(string code)
        {
            result = SingleRecordTransaction(Data, 3);
            if (result == "success")
            {
                _response.Add(new CustomResponse { key_code = code });
            }
            else
            {
                _response.Add(new CustomResponse { key_code = code, status = "300", msg = result });
            }
        }
    }
}
