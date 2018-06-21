using System;
using System.Collections.Generic;
using System.Text;

namespace Master.DataSvc.Models
{
    public class TemplateResultDo : Utils.Interfaces.ASearchResultData<TemplateDo>
    {

    }
    public partial class TemplateDo
    {
        public int TemplateID { get; set; }
        public string BrandCode { get; set; }
        public string TemplateName { get; set; }
        public bool FlagActive { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }



    public partial class TemplateMenuGroupDo
    {
        public int GroupID { get; set; }
        public string Name { get; set; }
        public bool FlagTakeAway { get; set; }

        public int Seq { get; set; }

        public List<TemplateMenuCategoryDo> Categories { get; set; }

        public bool FlagSelect { get; set; }
    }
    public partial class TemplateMenuCategoryDo
    {
        public int GroupID { get; set; }
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public bool FlagTakeAway { get; set; }

        public int Seq { get; set; }

        public List<TemplateMenuSubCategoryDo> SubCategories { get; set; }

        public bool FlagSelect { get; set; }
    }
    public partial class TemplateMenuSubCategoryDo
    {
        public int GroupID { get; set; }
        public int CategoryID { get; set; }
        public int SubCategoryID { get; set; }

        public string SubCategoryCode { get; set; }
        public string Name { get; set; }
        public bool FlagTakeAway { get; set; }

        public int Seq { get; set; }

        public List<TemplateMenuDo> Menus { get; set; }

        public bool FlagSelect { get; set; }
    }

    public partial class TemplateMenuDo
    {
        public int GroupID { get; set; }
        public int CategoryID { get; set; }
        public int SubCategoryID { get; set; }
        public int MenuID { get; set; }

        public string GroupName { get; set; }
        public string CategoryName { get; set; }

        public string SubCategoryName { get; set; }
        public string SubCategoryCode { get; set; }

        public string MenuCode { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public decimal Price { get; set; }
        public string Printer { get; set; }
        public bool FlagAllowDiscount { get; set; }
        public string SeparateBill { get; set; }
        public string BillHeader { get; set; }

        public bool FlagTakeAway { get; set; }

        public int GroupSeq { get; set; }
        public int CategorySeq { get; set; }
        public int SubCategorySeq { get; set; }
        public int Seq { get; set; }

        public bool FlagSelect { get; set; }
    }

    public class UpdateTemplateDo
    {
        public int TemplateID { get; set; }
        public List<TemplateMenuDo> Menus { get; set; }
    }
    public class UpdateTemplateSeqDo
    {
        public int TemplateID { get; set; }

        public List<MenuGroupForTemplateDo> Groups { get; set; }
        public List<MenuCategoryForTemplateDo> Categories { get; set; }
        public List<MenuSubCategoryForTemplateDo> SubCategories { get; set; }
        public List<MenuForTemplateDo> Menus { get; set; }
    }

    public partial class MenuGroupForTemplateDo
    {
        public Nullable<bool> FlagSelect { get; set; }
        public int GroupID { get; set; }
        public string Name { get; set; }
        public Nullable<int> Seq { get; set; }
    }
    public partial class MenuCategoryForTemplateDo
    {
        public Nullable<bool> FlagSelect { get; set; }
        public int GroupID { get; set; }
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public Nullable<int> Seq { get; set; }
    }

    public partial class MenuSubCategoryForTemplateDo
    {
        public Nullable<bool> FlagSelect { get; set; }
        public int GroupID { get; set; }
        public int CategoryID { get; set; }
        public int SubCategoryID { get; set; }
        public string Name { get; set; }
        public Nullable<int> Seq { get; set; }
        public bool FlagHas1Menu { get; set; }
    }
    public partial class MenuForTemplateDo
    {
        public Nullable<bool> FlagSelect { get; set; }
        public int GroupID { get; set; }
        public int CategoryID { get; set; }
        public int SubCategoryID { get; set; }
        public int MenuID { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public bool FlagTakeAway { get; set; }
        public Nullable<int> Seq { get; set; }
    }
}
