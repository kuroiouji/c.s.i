using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class ActivityDo
    {
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public string BranchName { get; set; }
        public int SHIFT { get; set; }
        public decimal ServiceCharge { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string BrandName { get; set; }
    }
    public partial class ClientInfoDo
    {
        public string CashierCom { get; set; }
        public string CashierPrinter { get; set; }
    }

    public partial class OpenActivityDo
    {
        public int BranchID { get; set; }
        public int SHIFT { get; set; }
        public decimal? OpenAmount { get; set; }

        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
    }

    public partial class CloseActivityDo
    {
        public string FrontEndID { get; set; }
        public int? BranchID { get; set; }
        public int? SHIFT { get; set; }

        public decimal? CloseAmount { get; set; }
        public List<CloseCreditDo> Credits { get; set; }

        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }
    public partial class CloseCreditDo
    {
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }
        public string CreditCode { get; set; }
        public decimal Amount { get; set; }
        public string AccountCode { get; set; }
    }
}
