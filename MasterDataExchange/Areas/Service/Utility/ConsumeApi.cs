﻿using DataExchange.Areas.Service.Models;
using Framework.CustomDataType;
using Framework.DataAccess.Dapper;
using Framework.Extension;
using Framework.Library.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DataExchange.Areas.Service.Utility
{
    public class ConsumeApi
    {
        private string _token;
        private ApiConfig _Config;
        private IEnumerable<object> DataList;
        private List<CustomResponse> _responseList;
        private DBRepository Repo ;
        public ConsumeApi(string token)
        {
            _token = token;
            Repo = new DBRepository();
        }
        public async Task Call()
        {

           


            try
            {
                JObject json = JObject.Parse(FileHelper.ReadFile(FileHelper.FullPath() + "Areas/Service/Config/ApiConfig.json"));
                _Config = json.SelectToken(_token).ToObject<ApiConfig>();
                GetData();
                if (DataList.Count()>0)
                {
                    dynamic data = new ExpandoObject();
                    data.data = DataList;
                    var request = new HttpRequestMessage(HttpMethod.Post, _Config.url);
                    foreach (KeyValuePair<string, string> header in _Config.headers.NotEmpty())
                    {
                        request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                    }                    
                    request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, _Config.type);
                    HttpResponseMessage result;
                    using (var client = new HttpClient())
                    {
                        result = await client.SendAsync(request);
                        if (result.IsSuccessStatusCode)
                        {
                            var jsonString = await result.Content.ReadAsStringAsync();
                            object ResultData = JObject.Parse(jsonString);
                            _responseList = ResultData.ParseRequestList<CustomResponse>();                           
                            UpdateData();
                        }
                        else
                        {
                            Repo.Add(new QueryParam
                            {
                                DirectQuery = $"insert into error_log(table_name,error_log) values('hosted service','{result.StatusCode}')"
                            });
                        }
                    }
                }
                
            }
            catch(Exception E)
            {
                Repo.Add(new QueryParam
                {
                    DirectQuery = $"insert into error_log(table_name,error_log) values('hosted service','{E.Message}')"
                });
            }
        }
        private void GetData()
        {
            
            DataList = Repo.FindAll(new QueryParam
            {
                Sp = _Config.sp
            });
        }

        private void UpdateData()
        {   
            if (_responseList.Count() > 0)
            {
                string tablename = NumericHelper.RandomNumber().ToString() + _token;
                string tmp1 = $"insert into {tablename} (key_code,update_status) values ('", tmp2 = $"insert into {tablename} (key_code,update_status) values ('";
                string[] flg1 = _responseList.Where(x => x.status == "200").Select(x => x.key_code).ToArray();
                if (flg1.Count() > 0)
                {
                    tmp1 += string.Join("',1),('", flg1) + "',1);";
                }
                else
                {
                    tmp1 = "";
                }
                string[] flg2 = _responseList.Where(x => x.status == "300").Select(x => x.key_code).ToArray();
                if (flg2.Count() > 0)
                {
                    tmp2 += string.Join("',2),('", flg2) + "',2);";
                }
                else
                {
                    tmp2 = "";
                }

                Repo.Add(new QueryParam
                {
                    DirectQuery = $"create table {tablename}(id int not null AUTO_INCREMENT PRIMARY KEY,key_code varchar(20) null,update_status int null); {tmp1}{tmp2} update {_Config.table} inner join {tablename} on key_code={_Config.field} set data_post_status=update_status,response_datetime=now();drop table {tablename}"
                });
            }
            
        }
    }
    class ApiConfig
    {
        public string url { get; set; }       
        public string sp { get; set; }         
        public string type { get; set; }
        public string table { get; set; }
        public string field { get; set; }
        public Dictionary<string,string> headers { get; set; }
    }
  
    
}
