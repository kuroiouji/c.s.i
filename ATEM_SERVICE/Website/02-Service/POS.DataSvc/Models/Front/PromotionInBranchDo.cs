using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class PromotionInBranchDo
    {
        public int PromotionTemplateID { get; set; }
        public int PromotionID { get; set; }
        public string PromotionType { get; set; }
        public string PromotionNumber { get; set; }
        public string PromotionName { get; set; }
        public string DiscountFor { get; set; }
        public string DiscountType { get; set; }
        public string DiscountValueDisplay { get; set; }
        public decimal? DiscountValue { get; set; }
        public decimal? CashValue { get; set; }

        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public bool FlagError { get; set; }
    }
    public partial class PromotionInBranchResultDo : Utils.SQL.ASQLDbResult
    {
        public PromotionInBranchDo Promotion { get; set; }

        public override object Data
        {
            get
            {
                if (this.Promotion != null)
                {
                    return this.Promotion;
                }

                return null;
            }
            set
            {
                this.Promotion = value as PromotionInBranchDo;
            }
        }
    }
}
