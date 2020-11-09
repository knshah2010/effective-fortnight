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
        public IActionResult SaveMilkAck(List<string> CollectionCodeList)
        {
            foreach (string code in CollectionCodeList)
            {
                MilkCollection collection = NewRepo.FindByKey<MilkCollection>(code);
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
        public IActionResult SaveBmcAck(List<string> CollectionCodeList)
        {
            foreach (string code in CollectionCodeList)
            {
                BmcCollection collection = NewRepo.FindByKey<BmcCollection>(code);
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
