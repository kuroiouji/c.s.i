using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public class LastestInterfaceDataDo
    {
        public bool CanInterfaceFIN
        {
            get
            {
                if (this.BranchList != null)
                    return this.BranchList.Exists(x => x.RBranch > x.SFIN);
                
                return false;
            }
        }
        public bool IsHQ { get; set; }

        public List<LastestInterfaceBranchDataDo> BranchList { get; set; }
    }
    public class LastestInterfaceBranchDataDo
    {
        public int BranchID { get; set; }
        public string BrandCode { get; set; }
        public string BranchName { get; set; }

        public DateTime? SBranch { get; set; }
        public DateTime? RBranch { get; set; }
        public DateTime? SFIN { get; set; }
        public DateTime? SHQ { get; set; }
        public DateTime? RHQ { get; set; }

        public string SBranchEDetail { get; set; }
        public string RBranchEDetail { get; set; }
        public string SFINEDetail { get; set; }
        public string SHQEDetail { get; set; }
        public string RHQEDetail { get; set; }
    }
}
