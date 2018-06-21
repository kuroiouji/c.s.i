using System;
using System.Collections.Generic;
using System.Text;

namespace Master.DataSvc.Models
{
    public partial class PromotionDo
    {
        public int PromotionTemplateID { get; set; }
        public string NameLC { get; set; }
        public string NameEN { get; set; }
        public string PromotionName { get; set; }
        public string PromotionType { get; set; }
        public int Qty { get; set; }
        public Nullable<System.DateTime> UsedTime { get; set; }
        public Nullable<System.DateTime> PrintTime { get; set; }
        public Nullable<System.DateTime> ActivateDate { get; set; }
        public Nullable<int> MemberID { get; set; }
        public string BrandLabel { get; set; }
        public string PromotionNumber { get; set; }
        public Nullable<decimal> PromotionValue { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<bool> FlagActive { get; set; }
        public Nullable<bool> FlagDelete { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public bool FlagGenerated { get; set; }
        public string TypeStatus { get; set; }
        public string DiscountFor { get; set; }
        public string DiscountType { get; set; }
        public Nullable<decimal> CashValue { get; set; }

        public List<PromotionBrandDo> Brands { get; set; }
        public List<PromotionAtivateDo> Promotions { get; set; }
        public bool? FlagActivate { get; set; }
    }

    public partial class UpdatePromotionResultDo : Utils.SQL.ASQLDbResult
    {
        public PromotionDo Promotion { get; set; }

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
                this.Promotion = value as PromotionDo;
            }
        }
    }
    public partial class PromotionBrandDo
    {
        public int PromotionTemplateID { get; set; }
        public string BrandCode { get; set; }
        public Nullable<decimal> DiscountValue { get; set; }
    }

    public partial class PromotionAtivateDo
    {
        public int PromotionID { get; set; }
    }

    public class PromotionResultDo : Utils.Interfaces.ASearchResultData<PromotionFSDo>
    {
        public int? TotalPromotion { get; set; }
        public int? TotalFilterPromotion { get; set; }
    }
    public partial class PromotionFSDo
    {
        public int PromotionTemplateID { get; set; }
        public int PromotionID { get; set; }
        public string NameEN { get; set; }
        public string NameLC { get; set; }
        public string PromotionType { get; set; }
        public string PromotionNumber { get; set; }
        public string PromotionTypeName { get; set; }
        public string DiscountFor { get; set; }
        public string DiscountForName { get; set; }
        public string DiscountType { get; set; }
        public Nullable<decimal> CashValue { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public int TotalFilterPromotion { get; set; }
        public int TotalPromotion { get; set; }
        public int PromotionUsed { get; set; }
        public Nullable<System.DateTime> PrintTime { get; set; }
        public Nullable<System.DateTime> UsedTime { get; set; }
        public Nullable<System.DateTime> ActivateDate { get; set; }
        public Nullable<bool> FlagActive { get; set; }
        public Nullable<bool> FlagDelete { get; set; }
        public Nullable<bool> FlagExpire { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public List<PromotionBrandDo> Brands { get; set; }

    }
}
