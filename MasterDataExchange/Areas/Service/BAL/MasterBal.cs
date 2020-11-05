﻿using DataExchange.Areas.Service.Models;
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
        private string _NewFileName;
        private IFormFile _File;
        private List<ModelParameter> Data;
        private List<CustomResponse> _response;
        private string result = "";
        public MasterBal()
        {
            Data = new List<ModelParameter>();
            _response = new List<CustomResponse>();
        }

        public IActionResult SaveBmc(List<Bmc> BmcList)
        {
            foreach (Bmc BmcModel in BmcList)
            {
                if (BmcModel.bmc_code != "")
                {
                    Bmc NewModel = NewRepo.FindByKey<Bmc>(BmcModel.bmc_code);
                    if (NewModel == null)
                    {
                        QueryParam Query = new QueryParam
                        {
                            Fields = "*",
                            Table = "tbl_unions",
                            Where = new List<ConditionParameter>
                        {
                           Condition("union_code","1")
                        }
                        };
                        Unions UnionsModel = NewRepo.Find<Unions>(Query);
                        if (UnionsModel.has_bmc == true && UnionsModel.has_mcc == false)
                        {
                            BmcModel.mcc_plant_code = BmcModel.bmc_code;
                            BmcModel.ref_code = BmcModel.bmc_code;
                            BmcModel.union_code = UnionsModel.union_code;
                            BmcModel.originating_org_code = UnionsModel.union_code;

                            MccPlant MccPlantModel = new MccPlant();
                            MccPlantModel.mcc_plant_code = BmcModel.bmc_code;
                            MccPlantModel.name = BmcModel.bmc_name;
                            MccPlantModel.union_code = BmcModel.union_code;
                            MccPlantModel.ref_code = BmcModel.bmc_code;
                            MccPlantModel.contact_person = BmcModel.bmc_incharge_name;
                            MccPlantModel.mobile_no = BmcModel.contact_no;
                            MccPlantModel.originating_org_code = BmcModel.originating_org_code;

                            Data.Add(new ModelParameter { SaveModel = BmcModel, ValidateModel = new BmcValidator() });
                            Data.Add(new ModelParameter { SaveModel = MccPlantModel, ValidateModel = new MccPlantValidator() });
                        }
                        else
                        {
                            BmcModel.mcc_plant_code = BmcModel.bmc_code;
                            BmcModel.ref_code = BmcModel.bmc_code;
                            BmcModel.union_code = UnionsModel.union_code;

                            Data.Add(new ModelParameter { SaveModel = BmcModel, ValidateModel = new BmcValidator() });
                        }
                    }
                    else
                    {
                        NewModel.bmc_name = BmcModel.bmc_name;
                        NewModel.is_active = BmcModel.is_active;
                        NewModel.bmc_incharge_name = BmcModel.bmc_incharge_name;
                        NewModel.contact_no = BmcModel.contact_no;
                        NewModel.model_operation = "update";
                        Data.Add(new ModelParameter { SaveModel = NewModel, ValidateModel = new BmcValidator() });
                    }
                    SaveData(BmcModel.bmc_code);
                }
                else
                {
                    _response.Add(new CustomResponse {status = "300", msg = "error:bmc_code" });
                }                
            }
            return new CustomResult("success",_response);
        }        
        public IActionResult SaveRoute(List<Route> RouteList)
        {
            foreach (Route RouteModel in RouteList)
            {
                if (RouteModel.route_code != "")
                {
                    Route NewModel = NewRepo.FindByKey<Route>(RouteModel.route_code);
                    if (NewModel == null)
                    {
                        QueryParam Query = new QueryParam
                        {
                            Fields = "*",
                            Table = "tbl_unions",
                            Where = new List<ConditionParameter>
                        {
                           Condition("union_code","1")
                        }
                        };
                        Unions UnionsModel = NewRepo.Find<Unions>(Query);


                        if (RouteModel.route_type == null || RouteModel.route_type == "")
                        {
                            RouteModel.route_type = "Can";
                            RouteModel.vehicle_type_code = 1;
                        }

                        if(RouteModel.route_type=="Can")
                            RouteModel.vehicle_type_code = 1;
                        else
                            RouteModel.vehicle_type_code = 2;

                        RouteModel.ref_code = RouteModel.route_code;
                        RouteModel.union_code = UnionsModel.union_code;
                        RouteModel.originating_org_code = UnionsModel.union_code;
                        RouteModel.is_active = true;

                        Data.Add(new ModelParameter { SaveModel = RouteModel, ValidateModel = new RouteValidator() });
                    }
                    else
                    {
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
            foreach (Dcs DcsModel in DcsList)
            {
                if (DcsModel.dcs_code != "")
                {
                    QueryParam Query = new QueryParam
                    {
                        Fields = "*",
                        Table = "tbl_unions",
                        Where = new List<ConditionParameter>
                        {
                           Condition("union_code","1")
                        }
                    };
                    Unions UnionsModel = NewRepo.Find<Unions>(Query);

                    Dcs NewModel = NewRepo.FindByKey<Dcs>(DcsModel.dcs_code);
                    if (NewModel == null)
                    {
                        if (DcsModel.allow_multiple_milktype == 1)
                            DcsModel.x_col1 = "1#1";
                        else if (DcsModel.allow_multiple_milktype == 2)
                            DcsModel.x_col1 = "0#1";
                        else if (DcsModel.allow_multiple_milktype == 3)
                            DcsModel.x_col1 = "1#0";
                        else if (DcsModel.allow_multiple_milktype == 4)
                            DcsModel.x_col1 = "0#0";

                        DcsModel.ref_code = DcsModel.dcs_code.PadLeft(15,'0');
                        DcsModel.originating_org_code = UnionsModel.union_code;

                        Data.Add(new ModelParameter { SaveModel = DcsModel, ValidateModel = new DcsValidator() });
                    }
                    else
                    {
                        NewModel.dcs_name = DcsModel.dcs_name;
                        NewModel.is_active = DcsModel.is_active;
                        NewModel.contact_person = DcsModel.contact_person;
                        NewModel.mobile_no = DcsModel.mobile_no;
                        NewModel.model_operation = "update";
                        Data.Add(new ModelParameter { SaveModel = NewModel, ValidateModel = new DcsValidator() });
                    }
                    SaveData(DcsModel.dcs_code);
                }
                else
                {
                    _response.Add(new CustomResponse { status = "300", msg = "error:dcs_code" });
                }
            }
            return new CustomResult("success", _response);
        }

        public IActionResult SaveMember(List<Member> MemberList)
        {
            foreach (Member MemberModel in MemberList)
            {
                if (MemberModel.member_code != "")
                {

                    QueryParam Query = new QueryParam
                    {
                        Fields = "*",
                        Table = "tbl_unions",
                        Where = new List<ConditionParameter>
                        {
                           Condition("union_code","1")
                        }
                    };
                    Unions UnionsModel = NewRepo.Find<Unions>(Query);

                    Member NewModel = NewRepo.FindByKey<Member>(MemberModel.member_code);
                    if (NewModel == null)
                    {
                        MemberModel.originating_org_code = UnionsModel.union_code;

                        Data.Add(new ModelParameter { SaveModel = MemberModel, ValidateModel = new MemberValidator() });
                    }
                    else
                    {
                        NewModel.member_name = MemberModel.member_name;
                        NewModel.is_active = MemberModel.is_active;
                        NewModel.mobile_no = MemberModel.mobile_no;
                        NewModel.model_operation = "update";
                        Data.Add(new ModelParameter { SaveModel = NewModel, ValidateModel = new MemberValidator() });
                    }
                    SaveData(MemberModel.member_code);
                }
                else
                {
                    _response.Add(new CustomResponse { status = "300", msg = "error:dcs_code" });
                }
            }
            return new CustomResult("success", _response);
        }

        public IActionResult SaveCustomer(List<CustomerMaster> CustomerMasterList)
        {
            foreach (CustomerMaster CustomerMasterModel in CustomerMasterList)
            {
                if (CustomerMasterModel.customer_code != "")
                {
                    QueryParam Query = new QueryParam
                    {
                        Fields = "*",
                        Table = "tbl_unions",
                        Where = new List<ConditionParameter>
                        {
                           Condition("union_code","1")
                        }
                    };
                    Unions UnionsModel = NewRepo.Find<Unions>(Query);


                    CustomerMaster NewModel = NewRepo.FindByKey<CustomerMaster>(CustomerMasterModel.customer_code);
                    if (NewModel == null)
                    {
                        CustomerMasterModel.ref_code = CustomerMasterModel.customer_code;
                        CustomerMasterModel.originating_org_code = UnionsModel.union_code;
                        Data.Add(new ModelParameter { SaveModel = CustomerMasterModel, ValidateModel = new CustomerMasterValidator() });
                    }
                    else
                    {
                        NewModel.customer_name = CustomerMasterModel.customer_name;
                        NewModel.is_active = CustomerMasterModel.is_active;
                        NewModel.mobile_no = CustomerMasterModel.mobile_no;
                        NewModel.model_operation = "update";
                        Data.Add(new ModelParameter { SaveModel = NewModel, ValidateModel = new CustomerMasterValidator() });
                    }
                    SaveData(CustomerMasterModel.customer_code);
                }
                else
                {
                    _response.Add(new CustomResponse { status = "300", msg = "error:dcs_code" });
                }
            }
            return new CustomResult("success", _response);
        }

        public IActionResult SaveVehicle(List<VehicleMaster> VehicleMasterList)
        {
            foreach (VehicleMaster VehicleMasterModel in VehicleMasterList)
            {
                if (VehicleMasterModel.vehicle_code != "")
                {
                    QueryParam Query = new QueryParam
                    {
                        Fields = "*",
                        Table = "tbl_unions",
                        Where = new List<ConditionParameter>
                        {
                           Condition("union_code","1")
                        }
                    };
                    Unions UnionsModel = NewRepo.Find<Unions>(Query);

                    VehicleMaster NewModel = NewRepo.FindByKey<VehicleMaster>(VehicleMasterModel.vehicle_code);
                    if (NewModel == null)
                    {

                        VehicleMasterModel.originating_org_code = UnionsModel.union_code;
                        Data.Add(new ModelParameter { SaveModel = VehicleMasterModel, ValidateModel = new VehicleMasterValidator() });
                    }
                    else
                    {
                        NewModel.driver_name = VehicleMasterModel.driver_name;
                        NewModel.is_active = VehicleMasterModel.is_active;
                        NewModel.driver_contact_no = VehicleMasterModel.driver_contact_no;
                        NewModel.driving_license_number = VehicleMasterModel.driving_license_number;
                        NewModel.model_operation = "update";
                        Data.Add(new ModelParameter { SaveModel = NewModel, ValidateModel = new VehicleMasterValidator() });
                    }
                    SaveData(VehicleMasterModel.vehicle_code);
                }
                else
                {
                    _response.Add(new CustomResponse { status = "300", msg = "error:dcs_code" });
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

        public IActionResult Upload(IFormFile File, string process_name)
        {
            ImportFile ImportFileModel = new ImportFile();


            _File = File;

            string DirectoryPath = FileHelper.FileServerPath();
            FileHelper.CreateDirectory(DirectoryPath);
            _NewFileName = FileHelper.NewFileName(DirectoryPath, process_name);


            ImportFileModel.file_name = File.FileName;
            ImportFileModel.process_name = process_name;
            ImportFileModel.file_path = _NewFileName;

            Data = new List<ModelParameter>
                {
                new ModelParameter { ValidateModel = new ImportFileValidator(), SaveModel = ImportFileModel }
                };

            using (var fileStream = new FileStream(_NewFileName, FileMode.Create))
            {
                _File.CopyTo(fileStream);
                SaveData(Data);
            }



            return new CustomResult("success");
        }

    }
}
