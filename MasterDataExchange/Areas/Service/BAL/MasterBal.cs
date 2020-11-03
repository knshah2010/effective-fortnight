using Framework.BAL;
using Framework.CustomDataType;
using Framework.Models;
using Models;
using System.Collections.Generic;

namespace DataExchange.Areas.Service.BAL
{
    public class MasterBal : BaseBal<BaseModel>
    {
        private List<ModelParameter> Data;
        private string _code = "";
        public MasterBal(Bmc BmcModel)
        {
            if (BmcModel.bmc_code != "")
            {
                Bmc NewModel = NewRepo.FindByKey<Bmc>(BmcModel.bmc_code);
                if (NewModel == null)
                {
                    Data.Add(new ModelParameter { SaveModel = BmcModel, ValidateModel = new BmcValidator() });
                }
                else
                {
                    NewModel.bmc_name = BmcModel.bmc_name;
                    NewModel.is_active = BmcModel.is_active;
                    NewModel.model_operation = "update";
                    Data.Add(new ModelParameter { SaveModel = BmcModel, ValidateModel = new BmcValidator() });
                }
            }
            else
            {
                _code = "bmc_code";
            }           
        }

        public MasterBal(Route RouteModel)
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
                    Data.Add(new ModelParameter { SaveModel = RouteModel, ValidateModel = new RouteValidator() });
                }
            }
            else
            {
                _code = "route_code";
            }
        }
    }
}
