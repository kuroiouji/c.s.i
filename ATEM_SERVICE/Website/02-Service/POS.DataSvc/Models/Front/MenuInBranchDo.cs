using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class MenuGroupInBranchDo
    {
        public int GroupID { get; set; }
        public string Name { get; set; }
        public bool FlagTakeAway { get; set; }

        public bool FlagOutOfStock
        {
            get
            {
                if (this.Categories != null)
                    return !this.Categories.Exists(x => x.FlagOutOfStock == false);

                return false;
            }
        }

        public DateTime? UpdateDate
        {
            get
            {
                if (this.Categories != null)
                    return this.Categories.Max(x => x.UpdateDate);

                return null;
            }
        }

        public List<MenuCategoryInBranchDo> Categories { get; set; }
    }
    public partial class MenuCategoryInBranchDo
    {
        public int GroupID { get; set; }
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public bool FlagTakeAway { get; set; }

        public bool FlagOutOfStock
        {
            get
            {
                if (this.MenuSubs != null)
                    return !this.MenuSubs.Exists(x => x.FlagOutOfStock == false);

                return false;
            }
        }

        public DateTime? UpdateDate
        {
            get
            {
                if (this.MenuSubs != null)
                    return this.MenuSubs.Max(x => x.UpdateDate);

                return null;
            }
        }

        public List<MenuSubInBranchDo> MenuSubs { get; set; }
    }
    public partial class MenuSubInBranchDo
    {
        public int GroupID { get; set; }
        public int CategoryID { get; set; }
        public int SubCategoryID { get; set; }

        public string Name { get; set; }
        public bool FlagHas1Menu { get; set; }
        public bool FlagTakeAway { get; set; }

        public bool FlagOutOfStock
        {
            get
            {
                if (this.Menus != null)
                    return !this.Menus.Exists(x => x.FlagOutOfStock == false);

                return false;
            }
        }

        public DateTime? UpdateDate
        {
            get
            {
                if (this.Menus != null)
                    return this.Menus.Max(x => x.UpdateDate);

                return null;
            }
        }

        public List<MenuInBranchDo> Menus { get; set; }
    }

    public partial class MenuInBranchDo
    {
        public int GroupID { get; set; }
        public int CategoryID { get; set; }
        public int SubCategoryID { get; set; }
        public int MenuID { get; set; }

        public string GroupName { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public decimal Price { get; set; }
        public string Printer { get; set; }
        public bool FlagTakeAway { get; set; }
        public bool FlagAllowDiscount { get; set; }
        public bool FlagSpecifyPrice { get; set; }
        public bool FlagOutOfStock { get; set; }
        public bool FlagHas1Menu { get; set; }
        public string SeparateBill { get; set; }
        public string BillHeader { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string MenuType { get; set; }
    }

    public partial class OutStockMenuInBranchDo
    {
        public int GroupID { get; set; }
        public int CategoryID { get; set; }
        public int SubCategoryID { get; set; }
        public int MenuID { get; set; }
        public bool FlagOutOfStock { get; set; }
    }

    public partial class UpdateOutStockMenuInBranchDo
    {
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }

        public List<MenuInBranchDo> Menus { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
