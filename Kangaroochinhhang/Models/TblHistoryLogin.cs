using System;
using System.Collections.Generic;

namespace Kangaroochinhhang.models
{
    public partial class TblHistoryLogin
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Task { get; set; }
        public int? IdUser { get; set; }
        public DateTime? DateCreate { get; set; }
        public bool? Active { get; set; }
    }
}
