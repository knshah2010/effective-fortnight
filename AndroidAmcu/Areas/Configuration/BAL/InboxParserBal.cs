using AndroidAmcu.Areas.Configuration.Models;
using Dapper;
using Framework.BAL;
using Framework.CustomDataType;
using Framework.Library.Helper;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace AndroidAmcu.Areas.Configuration.BAL
{
    public class InboxParserBal : BaseBal
    {
        public void ParseInbox()
        {
            List<Inbox> DataList = NewRepo.FindAll<Inbox>(new QueryParam
            {                
                    Table="tbl_inbox",
                    Fields="*",
                    Join=new List<JoinParameter>
                    {
                        new JoinParameter{table="tbl_sync_priority",condition="tbl_sync_priority.table_name=tbl_inbox.table_name"}
                    },
                    Where=new List<ConditionParameter>
                    {                        
                        Condition("sync_type","tbl_inbox"),
                        //new ConditionParameter{direct_condition="tbl_inbox.error_log is null or tbl_inbox.error_log=''"}
                    },
                    OrderBy="ifnull(tbl_sync_priority.sequence_no,99),posting_timestamp",
                    Offset=0,
                    Limit=50
                }).ToList();
                //},
                //new QueryParam
                //{
                //    Table="tbl_inbox",
                //    Fields="*",
                //    Join=new List<JoinParameter>
                //    {
                //        new JoinParameter{table="tbl_sync_priority",condition="tbl_sync_priority.table_name=tbl_inbox.table_name"}
                //    },
                //    Where=new List<ConditionParameter>
                //    {
                //        Condition("sync_type","tbl_inbox"),
                //        new ConditionParameter{direct_condition="tbl_inbox.error_log is not null or tbl_inbox.error_log=''",PropertyName="error_log"},
                //        Condition("tbl_inbox.error_timestamp",DateHelper.CurrentDate().AddHours(-1),"<")
                //    },
                //    OrderBy="ifnull(tbl_sync_priority.sequence_no,99),posting_timestamp",
                //    Offset=0,
                //    Limit=10
                //}

                foreach (Inbox _inboxModel in DataList)
                {
                SyncLog _syncLogModel = _inboxModel.Parse<SyncLog,Inbox>();
                try
                {
                    JObject DataParam = JObject.Parse(_inboxModel.json_text);
                    DynamicParameters param = new DynamicParameters();
                    List<string> plist= DataParam.Properties().Select(x => x.Name).ToList();
                    foreach (string pname in plist)
                    {
                        if (DataParam[pname].Type.ToString() == "Date")
                        {
                            param.Add(UnderScore(pname), DataParam[pname].Value<DateTime>());
                        }
                        //else if (DataParam[pname].Type.ToString() == "")
                        //{

                        //}
                        else
                        {
                            param.Add(UnderScore(pname), DataParam[pname].Value<string>());
                        }
                        
                    }
                    using (var transaction = new TransactionScope())
                    {
                        NewRepo.AddWithoutSentbox(_syncLogModel);
                        if (DataParam["x_col1"]!=null && DataParam["x_col1"].ToString()!="")
                        {
                            NewRepo.AddWithoutSentbox(_inboxModel.table_name, Columns(DataParam,"update"), param, "update");
                        }
                        else
                        {
                            NewRepo.AddWithoutSentbox(_inboxModel.table_name, Columns(DataParam), param);
                        }
                        NewRepo.Delete(_inboxModel);
                        transaction.Complete();
                    }
                    
                }
                catch(Exception E)
                {
                    InboxConstraint _constraintModel= _inboxModel.Parse<InboxConstraint, Inbox>();
                    _constraintModel.error_log = E.Message;
                    _constraintModel.error_timestamp = DateHelper.CurrentDate();
                    NewRepo.AddWithoutSentbox(_constraintModel);
                    NewRepo.Delete(_inboxModel);
                }
            }
        }
        private string Columns(JObject DataParam,string Op = "insert")
        {
            List<string> columnList;
            columnList = DataParam.Properties().Select(x => UnderScore(x.Name)).ToList();
            if (Op== "insert")
            {
                string stringOfColumns = string.Join(", ", columnList);
                string stringOfParameters = string.Join(", ", columnList.Select(e => "@" + e));
                return $"({ stringOfColumns}) values({ stringOfParameters})";
            }
            else
            {
                return string.Join(", ", columnList.Select(e => $"{e} = @{e}"));
            }            
        }
        private string UnderScore(string name)
        {
            return Regex.Replace(name, "((?<!^)[A-Z])", delegate (Match m) { return "_" + m.ToString().ToLower(); }, RegexOptions.None);
        }
    }
}
