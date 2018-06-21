using System;
using System.Collections.Generic;
using System.Text;

namespace Master.DataSvc.Models
{
    public partial class MenuGroupDo
    {
        public int GroupID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool FlagActive { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public bool FlagDelete { get; set; }
    }
    public partial class MenuCategoryDo
    {
        public int GroupID { get; set; }
        public int CategoryID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool FlagActive { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public bool FlagDelete { get; set; }
    }
    public partial class MenuSubCategoryDo
    {
        public int GroupID { get; set; }
        public int CategoryID { get; set; }
        public int SubCategoryID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool FlagActive { get; set; }
        public bool FlagHas1Menu { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public bool FlagDelete { get; set; }

        public List<MenuDo> Menus { get; set; }
    }
    public partial class MenuDo
    {
        public int GroupID { get; set; }
        public int CategoryID { get; set; }
        public int SubCategoryID { get; set; }
        public int MenuID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public decimal Price { get; set; }
        public string Printer { get; set; }
        public string PrinterName { get; set; }
        public bool FlagTakeAway { get; set; }
        public bool FlagAllowDiscount { get; set; }
        public string SeparateBill { get; set; }
        public string SeparateBillName { get; set; }
        public bool FlagActive { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public bool FlagSpecifyPrice { get; set; }
        public string BillHeader { get; set; }
        public string BillHeaderName { get; set; }
        public bool FlagDelete { get; set; }
        public string MenuType { get; set; }
        public string MenuTypeName { get; set; }
        public List<MenuBrandDo> Brands { get; set; }
    }
    public partial class MenuBrandDo
    {
        public int GroupID { get; set; }
        public int CategoryID { get; set; }
        public int SubCategoryID { get; set; }
        public int MenuID { get; set; }
        public string BrandCode { get; set; }
    }

    public partial class MenuGroupResultDo : Utils.Interfaces.ASearchResultData<MenuGroupDo>
    {

    }
    public partial class MenuCategoryResultDo : Utils.Interfaces.ASearchResultData<MenuCategoryDo>
    {

    }
    public partial class MenuSubCategoryResultDo : Utils.Interfaces.ASearchResultData<MenuSubCategoryDo>
    {

    }

    public class UpdateMenuGroupDo
    {
        public List<MenuGroupDo> Groups { get; set; }

        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }

    public partial class UpdateMenuGroupResultDo : Utils.SQL.ASQLDbResult
    {
        public MenuGroupDo Group { get; set; }

        public override object Data
        {
            get
            {
                if (this.Group != null)
                {
                    return this.Group;
                }

                return null;
            }
            set
            {
                this.Group = value as MenuGroupDo;
            }
        }
    }

    public class UpdateMenuCategoryDo
    {
        public int GroupID { get; set; }
        public List<MenuCategoryDo> Categories { get; set; }

        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }

    public partial class UpdateMenuCategoryResultDo : Utils.SQL.ASQLDbResult
    {
        public MenuCategoryDo Category { get; set; }

        public override object Data
        {
            get
            {
                if (this.Category != null)
                {
                    return this.Category;
                }

                return null;
            }
            set
            {
                this.Category = value as MenuCategoryDo;
            }
        }
    }

    public class UpdateMenuDo
    {
        public int GroupID { get; set; }
        public int CategoryID { get; set; }

        public List<MenuSubCategoryDo> SubCategories { get; set; }
        public List<MenuDo> Menus { get; set; }
        public List<MenuBrandDo> Brands { get; set; }

        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }
    
    public partial class UpdateMenuSubCategoryResultDo : Utils.SQL.ASQLDbResult
    {
        public MenuSubCategoryDo SubCategory { get; set; }
        
        public override object Data
        {
            get
            {
                if (this.SubCategory != null)
                {
                    return this.SubCategory;
                }

                return null;
            }
            set
            {
                this.SubCategory = value as MenuSubCategoryDo;
            }
        }
    }
    public partial class UpdateMenuResultDo : Utils.SQL.ASQLDbResult
    {
        public MenuDo Menu { get; set; }

        public override object Data
        {
            get
            {
                if (this.Menu != null)
                {
                    return this.Menu;
                }

                return null;
            }
            set
            {
                this.Menu = value as MenuDo;
            }
        }
    }
}
