using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;

namespace Models
{
    [Table("tbl_milk_collection")]
    public class MilkCollection : BaseModel
    {
        [Key]
        [ExplicitKey]
        public int milk_collection_code { get; set; }
        public string member_code { get; set; }
        public string dcs_code { get; set; }
        public string name { get; set; }
        public string mobile_no { get; set; }
        public int milk_type_code { get; set; }
        public decimal fat { get; set; }
        public decimal snf { get; set; }
        public decimal water { get; set; }
        public decimal qty { get; set; }
        public decimal rtpl { get; set; }
        public decimal amount { get; set; }
        public string auto_flag { get; set; }
        public string shift_code { get; set; }
        public DateTime date_time_of_collection { get; set; }
        public DateTime date_time_of_recieve { get; set; }
        public string village_code { get; set; }
        public int sample_no { get; set; }
        public string type_of_data_receive { get; set; }
        public string purchase_rate_code { get; set; }
        public string error_log { get; set; }
        public bool ack { get; set; }
        public string soc_bmc_flag { get; set; }
        public DateTime dt_date { get; set; }
        public string sms_status { get; set; }
        public string sms_msgid { get; set; }
        public string sms_mobile { get; set; }
        public string sms_errorlog { get; set; }
        public DateTime sms_timestamp { get; set; }
        public int data_post_status { get; set; }
        public decimal clr { get; set; }
        public string status { get; set; }
        public bool qty_mode { get; set; }
        public DateTime qlty_time { get; set; }
        public DateTime qty_time { get; set; }
        public int no_of_can { get; set; }
        public int milk_quality_type_code { get; set; }
        public bool qlty_auto { get; set; }
        public bool qty_auto { get; set; }
        public string route_code { get; set; }
        public string bmc_code { get; set; }
        public decimal converted_qty { get; set; }
        public int is_approved { get; set; }
        public string data_post_id { get; set; }
        public string resp_status { get; set; }
        public string resp_desc { get; set; }
        public string ftp_txn_file_name { get; set; }
        public string tag_1 { get; set; }
        public string tag_2 { get; set; }
        public int ftp_txn_log_id { get; set; }
        public string error_desc { get; set; }
        public string last_edited_type { get; set; }
        public string remarks { get; set; }
        public string sync_status { get; set; }
        public string union_code { get; set; }
        public string plant_code { get; set; }
        public string mcc_plant_code { get; set; }
        public string version_no { get; set; }
        public decimal protein { get; set; }
        public decimal density { get; set; }
        public decimal lactose { get; set; }
        public int dcs_payment_cycle_code { get; set; }
        public int milk_analyser_type_code { get; set; }
        public int ws_code { get; set; }
        public string x_col1 { get; set; }
        public string x_col2 { get; set; }
        public string x_col3 { get; set; }
        public string x_col4 { get; set; }
        public string x_col5 { get; set; }
        public decimal incentive { get; set; }
        public decimal deduction { get; set; }
        public decimal total_amount { get; set; }
        public string own_bmc_code { get; set; }
        public string own_mcc_plant_code { get; set; }
        public int send_status { get; set; }
        public DateTime response_datetime { get; set; }
        public string adt_param { get; set; }
        public decimal adt_value { get; set; }
        public int txfarmer_id { get; set; }
        public string data_inserted_from { get; set; }
        public int is_provisional { get; set; }
        public string device_lat { get; set; }
        public string device_long { get; set; }
        public string mob_lat { get; set; }
        public string mob_long { get; set; }
        public decimal dpu_rtpl { get; set; }
        public decimal dpu_amount { get; set; }
        public decimal dpu_incentive { get; set; }
        public decimal dpu_deduction { get; set; }
        public decimal dpu_total_amount { get; set; }
        public string token { get; set; }
        [Computed]
        public new string flg_sentbox_entry { get; set; } = "N";
    }

    public class MilkCollectionValidator : AbstractValidator<MilkCollection>
    {
        public MilkCollectionValidator()
        {

        }
    }
}
