using DataExchange.Areas.Service.Models;
using Framework.BAL;
using Framework.CustomDataType;
using Framework.Extension;
using Framework.Models;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections.Generic;

namespace DataExchange.Areas.Service.BAL
{
    public class MasterBal : BaseBal<BaseModel>
    {
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
                        BmcModel.mcc_plant_code = BmcModel.bmc_code;
                        BmcModel.ref_code = BmcModel.bmc_code;

                        Data.Add(new ModelParameter { SaveModel = BmcModel, ValidateModel = new BmcValidator() });
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
                    Member NewModel = NewRepo.FindByKey<Member>(MemberModel.member_code);
                    if (NewModel == null)
                    {
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
                    CustomerMaster NewModel = NewRepo.FindByKey<CustomerMaster>(CustomerMasterModel.customer_code);
                    if (NewModel == null)
                    {
                        CustomerMasterModel.ref_code = CustomerMasterModel.customer_code;
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
                    VehicleMaster NewModel = NewRepo.FindByKey<VehicleMaster>(VehicleMasterModel.vehicle_code);
                    if (NewModel == null)
                    {
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



    }
}
