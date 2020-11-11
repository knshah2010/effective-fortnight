using DataExchange.Areas.Service.Models;
using Framework.BAL;
using Framework.CustomDataType;
using Framework.Extension;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Collections.Generic;


namespace DataExchange.Areas.Service.BAL
{
    public class TransactionBal : BaseBal
    {
        public TransactionBal()
        {            
        }
        public IActionResult SaveMilkAck(List<CustomResponse> CollectionCodeList)
        {
            foreach (CustomResponse code in CollectionCodeList)
            {
                MilkCollection collection = NewRepo.FindByKey<MilkCollection>(code.key_code);
                if (collection!=null)
                {
                    collection.data_post_status = 1;
                    collection.model_operation = "update";
                    List<ModelParameter> Data = new List<ModelParameter>()
                    {
                        new ModelParameter{SaveModel=collection,ValidateModel=null}
                    };
                    SingleRecordTransaction(Data, 3);
                }
                
            }
            return new CustomResult("success",new CustomResponse { status = "200", msg = "success" });
        }
        public IActionResult SaveBmcAck(List<CustomResponse> CollectionCodeList)
        {
            foreach (CustomResponse code in CollectionCodeList)
            {
                BmcCollection collection = NewRepo.FindByKey<BmcCollection>(code.key_code);
                if (collection != null)
                {
                    collection.data_post_status = 1;
                    collection.model_operation = "update";
                    List<ModelParameter> Data = new List<ModelParameter>()
                    {
                        new ModelParameter{SaveModel=collection,ValidateModel=null}
                    };
                    SingleRecordTransaction(Data, 3);
                }

            }
            return new CustomResult("success", new CustomResponse { status = "200", msg = "success" });
        }
    }
}
