using DataExchange.Areas.Service.BAL;
using DataExchange.Areas.Service.Models;
using Framework.Controllers;
using Framework.CustomDataType;
using Framework.Extension;
using Framework.Library.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;

namespace DataExchange.Areas.Service.Controllers
{
    [Area("Service")]
    [Route("{v:apiVersion}/Service/[controller]")]
    public class TransactionController : BaseController
    {

        [HttpPost]
        [Route("milk_collection")]
        public IActionResult List([FromBody] object data)
        {
            ListEngine _listEngine = new ListEngine();
            SetParam();
            return _listEngine.List(data, FileName, "milk_collection");
        }

        [HttpPost]
        [Route("rmrd_collection")]
        public IActionResult RmrdCollection([FromBody] object data)
        {
            ListEngine _listEngine = new ListEngine();
            SetParam();
            return _listEngine.List(data, FileName, "rmrd_collection");
        }

        [HttpPost]
        [Route("milkcollection_ack")]
        public IActionResult Ack([FromBody] object data)
        {
            TransactionBal _bal = new TransactionBal();
            return _bal.SaveMilkAck(data.ParseRequestList<CustomResponse>());
        }
        [HttpPost]
        [Route("bmccollection_ack")]
        public IActionResult BmcAck([FromBody] object data)
        {
            TransactionBal _bal = new TransactionBal();
            return _bal.SaveMilkAck(data.ParseRequestList<CustomResponse>());
        }

        [HttpGet]
        [Route("download_data")]
        public IActionResult DownloadData(DateTime fromdate, DateTime todate)
        {
            SearchParam Param = new SearchParam()
            {
                Param = new List<ConditionParameter>()
                {
                     new ConditionParameter{PropertyName="p_from_date",PropertyValue=fromdate},
                     new ConditionParameter{PropertyName="p_to_date",PropertyValue=todate},
                }
            };
            dynamic data = new ExpandoObject();
            data.data = Param;
            ListEngine _listEngine = new ListEngine();
            SetParam();
            List<Download> DataList=_listEngine.List<Download>(JObject.FromObject(data), FileName, "download_data");
            if (DataList == null || DataList.Count == 0)
            {
                return new CustomResult("success", new CustomResponse { status = "300", msg = "error:no_record_found" }); 
            }

            return File(
                fileContents: Encoding.ASCII.GetBytes(CsvHelper.ExportListToCSV<Download>(DataList)),
                contentType: "text/csv",
                fileDownloadName: "collection.csv"
            );
        }

    }
}
