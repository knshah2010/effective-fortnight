using Framework.Models;
using Dapper.Contrib.Extensions;
using FluentValidation;
using Framework.Library.Validator;
using System;
using Framework.Library.Helper;
using System.Numerics;

namespace Models
{
    [Table("import_file")]
    public class ImportFile :BaseModel
    {
        [Key]
        [ExplicitKey]
        public int code { get; set; }
        public string operation_type { get; set; }
        public string process_name { get; set; }
        public string file_name { get; set; }
        public string file_path { get; set; }
        public string error_file_path { get; set; }
        public int total_count { get; set; }
        /* 
         * 1=upload
         * 2=in process
         * 3=complete
         */
        public char file_status { get; set; } = '1';
        /*
         * 0 : not processed {bydefault this set} {while upload file}
         * 1 : successfully processed all record
         * 2 : there are error in some record
         */
        public char record_status { get; set; } = '0';
        public int success_count { get; set; }
        public int error_count { get; set; }
        public string error_data { get; set; }

        [Computed]
        public new string flg_sentbox_entry { get; set; }
        //public string created_by { get; set; }
        //public DateTime created_at { get; set; }
    }
    public class ImportFileValidator : AbstractValidator<ImportFile>
    {
        public ImportFileValidator()
        {
            RuleFor(d => d.process_name).Require();
        }
    }
}
