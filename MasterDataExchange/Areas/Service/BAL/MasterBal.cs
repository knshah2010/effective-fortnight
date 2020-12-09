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
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataExchange.Areas.Service.BAL
{
    public class MasterBal : BaseBal<BaseModel>
    {
        private string result = "";
        private List<ModelParameter> Data;
        private List<CustomResponse> _response;

        public MasterBal()
        {
            _response = new List<CustomResponse>();
        }

        public IActionResult SaveBmc(List<Bmc> BmcList)
        {
            Unions UnionsModel = GetLastRecord<Unions>("tbl_unions");
            Plant PlantModel = GetLastRecord<Plant>("tbl_plant");
            MccPlant MccModel = GetLastRecord<MccPlant>("tbl_mcc_plant");
            foreach (Bmc BmcModel in BmcList)
            {
                Data = new List<ModelParameter>();
                if (BmcModel.bmc_code != "" && BmcModel.milk_type != 0)
                {
                    Bmc NewModel = NewRepo.FindByKey<Bmc>(BmcModel.bmc_code);
                    if (NewModel == null)
                    {
 
                        BmcModel.ref_code = BmcModel.bmc_code;
                        BmcModel.union_code = BmcModel.originating_org_code = UnionsModel.union_code;
                        BmcModel.plant_code = PlantModel.plant_code;
                        if (UnionsModel.has_bmc == true && UnionsModel.has_mcc == false)
                        {
                            BmcModel.mcc_plant_code = BmcModel.bmc_code;
                            MccPlant MccPlantModel = new MccPlant
                            {
                                mcc_plant_code = BmcModel.bmc_code,
                                plant_code = PlantModel.plant_code,
                                name = BmcModel.bmc_name,
                                ref_code = BmcModel.ref_code,
                                contact_person = BmcModel.bmc_incharge_name,
                                mobile_no = BmcModel.contact_no
                            };
                            MccPlantModel.union_code = MccPlantModel.originating_org_code = BmcModel.union_code;
                            Data.Add(new ModelParameter { SaveModel = MccPlantModel, ValidateModel = new MccPlantValidator() });
                            
                        }
                        else
                        {
                            BmcModel.mcc_plant_code = MccModel.mcc_plant_code;
                        }

                        BmcSilosInfo BmcSilosInfoModel = new BmcSilosInfo();

                        BmcSilosInfoModel.silo_no = "1";
                        BmcSilosInfoModel.description = "Silo Mix";
                        BmcSilosInfoModel.manufacturer_code = 1;
                        BmcSilosInfoModel.storage_capacity = 3000;
                        BmcSilosInfoModel.chilling_capacity = 3000;
                        BmcSilosInfoModel.owning_type = "own";
                        BmcSilosInfoModel.module_name = "BMC";
                        BmcSilosInfoModel.module_code = BmcModel.bmc_code;
                        BmcSilosInfoModel.bmc_code = BmcModel.bmc_code;
                        BmcSilosInfoModel.milk_type_code = 3;
                        BmcSilosInfoModel.union_code = UnionsModel.union_code;
                        BmcSilosInfoModel.mcc_plant_code = BmcModel.bmc_code;
                        BmcSilosInfoModel.wef_date= DateHelper.CurrentDate();

                        Data.Add(new ModelParameter { SaveModel = BmcSilosInfoModel, ValidateModel = new BmcSilosInfoValidator() });
                        Data.Add(new ModelParameter { SaveModel = BmcModel, ValidateModel = new BmcValidator() });
                        SetBmcMilkType(BmcModel);
                    }
                    else
                    {
                        if (UnionsModel.has_bmc == true && UnionsModel.has_mcc == false)
                        {
                            MccPlant MccPlantModel = NewRepo.FindByKey<MccPlant>(BmcModel.bmc_code);
                            MccPlantModel.name = BmcModel.bmc_name;
                            MccPlantModel.contact_person = BmcModel.bmc_incharge_name;
                            MccPlantModel.mobile_no = BmcModel.contact_no;
                            MccPlantModel.model_operation = "update";
                            Data.Add(new ModelParameter { SaveModel = MccPlantModel, ValidateModel = new MccPlantValidator() });
                        }
                        NewModel.bmc_name = BmcModel.bmc_name;
                        NewModel.is_active = BmcModel.is_active;
                        NewModel.bmc_incharge_name = BmcModel.bmc_incharge_name;
                        NewModel.contact_no = BmcModel.contact_no;
                        NewModel.model_operation = "update";
                        Data.Add(new ModelParameter { SaveModel = NewModel, ValidateModel = new BmcValidator() });
                        SetBmcMilkType(BmcModel);
                    }
                    SaveData(BmcModel.bmc_code);
                }
                else
                {
                    _response.Add(new CustomResponse { status = "300", msg = "error:bmc_code or milktype require" });
                }
            }
            return new CustomResult("success", _response);
        }
        public IActionResult SaveRoute(List<Route> RouteList)
        {
            Unions UnionsModel = GetLastRecord<Unions>("tbl_unions");
            foreach (Route RouteModel in RouteList)
            {
                Data = new List<ModelParameter>();
                if (RouteModel.route_code != "")
                {
                    Route NewModel = NewRepo.FindByKey<Route>(RouteModel.route_code);
                    if (RouteModel.route_type == null || RouteModel.route_type == "")
                    {
                        RouteModel.route_type = "Can";
                    }
                    if (NewModel == null)
                    {
                        RouteModel.vehicle_type_code = (RouteModel.route_type == "Can") ? 1 : 2;
                        RouteModel.to_dest = RouteModel.bmc_code;
                        RouteModel.to_type = "bmc";
                        RouteModel.ref_code = RouteModel.route_code;
                        RouteModel.union_code = RouteModel.originating_org_code = UnionsModel.union_code;
                        Data.Add(new ModelParameter { SaveModel = RouteModel, ValidateModel = new RouteValidator() });
                    }
                    else
                    {
                        NewModel.vehicle_type_code = (RouteModel.route_type == "Can") ? 1 : 2;
                        NewModel.to_dest = RouteModel.bmc_code;
                        NewModel.bmc_code = RouteModel.bmc_code;
                        NewModel.route_name = RouteModel.route_name;
                        NewModel.is_active = RouteModel.is_active;
                        NewModel.route_supervisor_name = RouteModel.route_supervisor_name;
                        NewModel.contact_no = RouteModel.contact_no;
                        NewModel.model_operation = "update";
                        Data.Add(new ModelParameter { SaveModel = NewModel, ValidateModel = new RouteValidator() });
                    }
                    SaveData(RouteModel.route_code);
                }
                else
                {
                    _response.Add(new CustomResponse { status = "300", msg = "error:route_code" });
                }
            }
            return new CustomResult("success", _response);
        }

        public IActionResult SaveMpp(List<Dcs> DcsList)
        {
            Unions UnionsModel = GetLastRecord<Unions>("tbl_unions");
            MccPlant MccPlantModel = GetLastRecord<MccPlant>("tbl_mcc_plant");
            foreach (Dcs DcsModel in DcsList)
            {
                Data = new List<ModelParameter>();
                if (DcsModel.dcs_code != "" && DcsModel.milk_type != 0)
                {
                    Dcs NewModel = NewRepo.FindByKey<Dcs>(DcsModel.dcs_code);
                    if (NewModel == null)
                    {
                        if (UnionsModel.has_mcc == true)
                            DcsModel.mcc_plant_code = MccPlantModel.mcc_plant_code;
                        else
                            DcsModel.mcc_plant_code = DcsModel.bmc_code;

                        DcsModel.ref_code = DcsModel.dcs_code.PadLeft(15, '0');
                        DcsModel.originating_org_code = DcsModel.union_code = MccPlantModel.union_code;
                        DcsModel.plant_code = MccPlantModel.plant_code;
                        DcsModel.x_col1 = SetDcsXcol(DcsModel.allow_multiple_milktype);
                        Data.Add(new ModelParameter { SaveModel = DcsModel, ValidateModel = new DcsValidator() });
                        SetDcsMilkType(DcsModel);
                    }
                    else
                    {
                        NewModel.dcs_name = DcsModel.dcs_name;
                        NewModel.bmc_code = DcsModel.bmc_code;
                        NewModel.route_code = DcsModel.route_code;
                        NewModel.is_active = DcsModel.is_active;
                        NewModel.contact_person = DcsModel.contact_person;
                        NewModel.mobile_no = DcsModel.mobile_no;
                        NewModel.allow_multiple_milktype = DcsModel.allow_multiple_milktype;
                        NewModel.x_col1 = SetDcsXcol(DcsModel.allow_multiple_milktype);
                        NewModel.model_operation = "update";
                        Data.Add(new ModelParameter { SaveModel = NewModel, ValidateModel = new DcsValidator() });
                        SetDcsMilkType(DcsModel);
                    }
                    SaveData(DcsModel.dcs_code);
                }
                else
                {
                    _response.Add(new CustomResponse { status = "300", msg = "error:dcs_code or milktype require" });
                }
            }
            return new CustomResult("success", _response);
        }

        public IActionResult SaveMember(List<Member> MemberList)
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
                        MemberModel.ex_member_code = MemberModel.member_code;
                        MemberModel.member_code = MemberModel.dcs_code + MemberModel.member_code.PadLeft(4, '0');
                        MemberModel.originating_org_code = UnionsModel.union_code;
                        MemberModel.ref_code = MemberModel.member_unique_code;
                        Data.Add(new ModelParameter { SaveModel = MemberModel, ValidateModel = new MemberValidator() });
                    }
                    else
                    {
                        NewModel.member_unique_code = MemberModel.member_unique_code;
                        NewModel.dcs_code = MemberModel.dcs_code;
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
                    _response.Add(new CustomResponse { status = "300", msg = "error:member_unique_code:require" });
                }
            }
            return new CustomResult("success", _response);
        }

        public IActionResult SaveCustomer(List<CustomerMaster> CustomerMasterList)
        {
            Unions UnionsModel = GetLastRecord<Unions>("tbl_unions");
            QueryParam Query = new QueryParam
            {
                Fields = "*",
                Table = "tbl_customer_type"
            };
            List<CustomerType> CustomerTypeList = NewRepo.FindAll<CustomerType>(Query).ToList();
            foreach (CustomerMaster CustomerMasterModel in CustomerMasterList)
            {
                Data = new List<ModelParameter>();

                if (CustomerMasterModel.customer_unique_code.Trim() != "")
                {
                    string prefix = CustomerTypeList.Where(x => x.customer_type == CustomerMasterModel.customer_type).Select(x => x.code_prefix).FirstOrDefault();
                    if (CheckPrefix(prefix, CustomerMasterModel.customer_code))
                    {
                        CustomerMaster NewModel = NewRepo.Find<CustomerMaster>(new QueryParam { Where = new List<ConditionParameter> { Condition("ref_code", CustomerMasterModel.customer_unique_code) } });
                        if (NewModel == null)
                        {
                            CustomerMasterModel.customer_code_ex = CustomerMasterModel.customer_code;
                           // CustomerMasterModel.customer_code = CustomerMasterModel.bmc_code + new GetKey().ReturnValue("customer_code") ;
                            CustomerMasterModel.ref_code = CustomerMasterModel.customer_unique_code;
                            CustomerMasterModel.x_col1 = SetDcsXcol(CustomerMasterModel.allow_multiple_milktype);
                            CustomerMasterModel.originating_org_code = CustomerMasterModel.union_code = UnionsModel.union_code;
                            Data.Add(new ModelParameter { SaveModel = CustomerMasterModel, ValidateModel = new CustomerMasterValidator() });
                        }
                        else
                        {
                            NewModel.customer_unique_code = CustomerMasterModel.customer_unique_code;
                            NewModel.customer_code_ex = CustomerMasterModel.customer_code;
                            NewModel.bmc_code = CustomerMasterModel.bmc_code;
                            NewModel.x_col1 = SetDcsXcol(CustomerMasterModel.allow_multiple_milktype);
                            NewModel.route_code = CustomerMasterModel.route_code;
                            NewModel.customer_name = CustomerMasterModel.customer_name;
                            NewModel.is_active = CustomerMasterModel.is_active;
                            NewModel.mobile_no = CustomerMasterModel.mobile_no;
                            NewModel.customer_type = CustomerMasterModel.customer_type;
                            NewModel.model_operation = "update";
                            Data.Add(new ModelParameter { SaveModel = NewModel, ValidateModel = new CustomerMasterValidator() });
                        }
                        SaveData(CustomerMasterModel.customer_unique_code);
                    }
                    else
                    {
                        _response.Add(new CustomResponse { status = "300", msg = "error:customer_code:Invalid Code" });
                    }

                }
                else
                {
                    _response.Add(new CustomResponse { status = "300", msg = "error:customer_unique_code:Require" });
                }
            }
            return new CustomResult("success", _response);
        }
        private bool CheckPrefix(string prefix, string code)
        {
            if (prefix == null || prefix.Trim() == "")
                return true;
            else
            {
                if (code.Substring(0, prefix.Length) == prefix)
                    return true;
                else
                    return false;
            }
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
                    if (PurchaseRateApplicabilityModel.rate_for == "farmer_collection")
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

                }
            }
            return new CustomResult("success", _response);
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
        private string SetDcsXcol(int allow_multiple_milktype)
        {
            if (allow_multiple_milktype == 1)
                return "1#1";
            else if (allow_multiple_milktype == 2)
                return "0#1";
            else if (allow_multiple_milktype == 3)
                return "1#0";
            else if (allow_multiple_milktype == 4)
                return "0#0";
            return "1#1";
        }

        private void SetBmcMilkType(Bmc BmcModel)
        {
            BmcMilkType BmcMilkTypeModel = new BmcMilkType();
            QueryParam _queryParam = new QueryParam();
            _queryParam.Where = new List<ConditionParameter>()
             {
                Condition("bmc_code",BmcModel.bmc_code)
             };

            BmcMilkType BmcMilkTypeModelDelete = NewRepo.Find<BmcMilkType>(_queryParam);
            if (BmcMilkTypeModelDelete != null)
            {
                BmcMilkTypeModelDelete.model_operation = "delete";
                Data.Add(new ModelParameter() { ValidateModel = new BmcMilkTypeValidator(), SaveModel = BmcMilkTypeModelDelete });
            }

            int milk_type = BmcModel.milk_type;

            int[,] milkTypeArray = new int[7, 3] { { 1, 0, 0 }, { 2, 0, 0 }, { 3, 0, 0 }, { 1, 2, 0 }, { 2, 3, 0 }, { 1, 3, 0 }, { 1, 2, 3 } };

            for (int i = 1; i <= 7; i++)
            {
                if (milk_type == i)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (milkTypeArray[i - 1, j] != 0)
                        {
                           
                            BmcMilkTypeModel.bmc_code = BmcModel.bmc_code;
                            BmcMilkTypeModel.milk_type_code = milkTypeArray[i - 1, j];
                            BmcMilkTypeModel.is_active = true;
                            Data.Add(new ModelParameter() { ValidateModel = new BmcMilkTypeValidator(), SaveModel = BmcMilkTypeModel });
                        }
                    }
                }
            }
        }

        private void SetDcsMilkType(Dcs DcsModel)
        {
            DcsMilkType DcsMilkTypeModel = new DcsMilkType();
            QueryParam _queryParam = new QueryParam();
             _queryParam.Where = new List<ConditionParameter>()
             {
                Condition("dcs_code",DcsModel.dcs_code)
             };

            DcsMilkType DcsMilkTypeModelDelete = NewRepo.Find<DcsMilkType>(_queryParam);
            if (DcsMilkTypeModelDelete != null)
            {
                DcsMilkTypeModelDelete.model_operation = "delete";
                Data.Add(new ModelParameter() { ValidateModel = new DcsMilkTypeValidator(), SaveModel = DcsMilkTypeModelDelete });
            }


            int milk_type = DcsModel.milk_type;

            int[,] milkTypeArray = new int[7, 3] { { 1, 0, 0 }, { 2, 0, 0 }, { 3, 0, 0 }, { 1, 2, 0 }, { 2, 3, 0 }, { 1, 3, 0 }, { 1, 2, 3 } };

            for (int i = 1; i <= 7; i++)
            {
                if (milk_type == i)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (milkTypeArray[i - 1, j] != 0)
                        {
                            
                            DcsMilkTypeModel.dcs_code = DcsModel.dcs_code;
                            DcsMilkTypeModel.milk_type_code = milkTypeArray[i - 1, j];
                            DcsMilkTypeModel.is_active = true;
                            Data.Add(new ModelParameter() { ValidateModel = new DcsMilkTypeValidator(), SaveModel = DcsMilkTypeModel });
                        }
                    }
                }
            }
        }

        //private string CheckCustomerCode(string customer_type)
        //{
        //    QueryParam Query = new QueryParam
        //    {
        //        Fields = "*",
        //        Table = "tbl_customer_type",
        //        Where = new List<ConditionParameter>
        //            {
        //                Condition("customer_type",customer_type)
        //            }
        //    };
        //    CustomerType CustomerTypeModel = NewRepo.Find<CustomerType>(Query);


        //    if (CustomerTypeModel.customer_type== "VENDOR") { }



        //}
        public IActionResult Upload(IFormFile File, int rate_type)
        {
            Data = new List<ModelParameter>();
            if (Path.GetExtension(File.FileName).ToLower() != ".csv")
            {
                return new CustomResult("success", new CustomResponse { key_code = "0", status = "300", msg = "error:Only Csv File Allowed" });
            }
            string[] param = File.FileName.Split('_');
            if (param.Count() != 2)
            {
                return new CustomResult("success", new CustomResponse { key_code = "0", status = "300", msg = "error:Wrong FileName Format" });
            }
            string DirectoryPath = FileHelper.FileServerPath();
            FileHelper.CreateDirectory(FileHelper.FileServerPath());
            ImportFile ImportFileModel = new ImportFile
            {
                file_name = File.FileName,
                process_name = "purchase_rate",
                new_file_path = FileHelper.NewFileName(DirectoryPath, "purchase_rate", "csv")
            };
            Data.Add(new ModelParameter { ValidateModel = new ImportFileValidator(), SaveModel = ImportFileModel });
            using (var fileStream = new FileStream(ImportFileModel.new_file_path, FileMode.Create))
            {
                File.CopyTo(fileStream);
                SaveData(Data);
            }
            string rate = NewRepo.Find<string>(new QueryParam
            {
                Fields = "purchase_rate_code",
                Table = "tbl_purchase_rate",
                Where = new List<ConditionParameter> { Condition("purchase_rate_code", param[0].Trim()) }
            });
            if (rate == param[0].Trim())
            {
                return new CustomResult("success", new CustomResponse { key_code = param[0], status = "300", msg = "error:Rate already Exist" });
            }
            string tablename = NumericHelper.RandomNumber().ToString() + "tmp";
            string DirectQuery = $"create table {tablename}(id int not null AUTO_INCREMENT PRIMARY KEY,fat decimal(18,2),snf decimal(18,2),milk_type varchar(5),rtpl decimal(18,2),class varchar(5) null default '0',milk_type_code int null); LOAD DATA LOCAL INFILE  '{ImportFileModel.new_file_path.Replace('\\', '/')}' INTO TABLE {tablename}  FIELDS TERMINATED BY ';'  IGNORE 1 ROWS (fat,snf,milk_type,rtpl); ";
            if (rate_type != 0)
            {
                DirectQuery = $"create table {tablename}(id int not null AUTO_INCREMENT PRIMARY KEY,fat decimal(18,2),snf decimal(18,2),milk_type varchar(5),rtpl decimal(18,2),class varchar(5) null default '0',milk_type_code int null); LOAD DATA LOCAL INFILE  '{ImportFileModel.new_file_path.Replace('\\', '/')}' INTO TABLE {tablename} FIELDS TERMINATED BY ';'  IGNORE 1 ROWS (fat,snf,milk_type,rtpl,class); ";
            }
            NewRepo.Add(new QueryParam
            {
                DirectQuery = DirectQuery
            });
            string  value = NewRepo.FindAll<string>(new QueryParam
            {
                Sp = "import_rate",
                Where = new List<ConditionParameter>
                {
                    Condition("p_rate_code",param[0].Trim()),
                    Condition("p_rate_date",$"{param[1].Substring(4,4)}-{param[1].Substring(2,2)}-{param[1].Substring(0,2)}"),
                    Condition("p_table_name",tablename),
                    Condition("p_usercode",UserData.user_code),
                     Condition("p_rate_type",rate_type),
                }
            }).FirstOrDefault().ToString();
            if (value == "1")
            {
                return new CustomResult("success", new CustomResponse { key_code = param[0], status = "200", msg = "success" });
            }
            else
            {
                return new CustomResult("success", new CustomResponse { key_code = param[0], status = "300", msg = "error" });
            }

           
        }

    }
}
