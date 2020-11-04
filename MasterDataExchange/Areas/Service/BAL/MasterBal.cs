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
                        Data.Add(new ModelParameter { SaveModel = BmcModel, ValidateModel = new BmcValidator() });
                    }
                    else
                    {
                        NewModel.bmc_name = BmcModel.bmc_name;
                        NewModel.is_active = BmcModel.is_active;
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
                        Data.Add(new ModelParameter { SaveModel = RouteModel, ValidateModel = new RouteValidator() });
                    }
                    else
                    {
                        NewModel.route_name = RouteModel.route_name;
                        NewModel.is_active = RouteModel.is_active;
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
                        Data.Add(new ModelParameter { SaveModel = DcsModel, ValidateModel = new RouteValidator() });
                    }
                    else
                    {
                        NewModel.dcs_name = DcsModel.dcs_name;
                        NewModel.is_active = DcsModel.is_active;
                        NewModel.model_operation = "update";
                        Data.Add(new ModelParameter { SaveModel = NewModel, ValidateModel = new RouteValidator() });
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
