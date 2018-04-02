using System;
using System.Collections.Generic;

namespace Kangaroochinhhang.models
{
    public partial class TblOrder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Mobiles { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Name1 { get; set; }
        public string Address1 { get; set; }
        public string Mobile1 { get; set; }
        public string Mobile1s { get; set; }
        public string Email1 { get; set; }
        public string NameCp { get; set; }
        public string AddressCp { get; set; }
        public string Mst { get; set; }
        public int? TypePay { get; set; }
        public int? TypeTransport { get; set; }
        public double? Tolar { get; set; }
        public DateTime? DateByy { get; set; }
        public int? IdUser { get; set; }
        public bool? Status { get; set; }
        public bool? Active { get; set; }
    }
}
