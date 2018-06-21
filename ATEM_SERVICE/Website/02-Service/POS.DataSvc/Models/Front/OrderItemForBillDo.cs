using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class OrderForBillDo
    {
        public string TransactionRefID { get; set; }
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }
        public int OrderID { get; set; }

        public string BranchName { get; set; }
        public int CustomerQty { get; set; }
        public string CreateUser { get; set; }
        public Nullable<int> BillNo { get; set; }
        public Nullable<System.DateTime> PrintDate { get; set; }
        public int PrintNo { get; set; }
        public string ApproveUser { get; set; }
        public decimal TotalAmt { get; set; }
        public decimal TotalLineDiscountAmt { get; set; }
        public decimal TotalPromotionDiscountAmt { get; set; }
        public decimal TotalPromotionVoucherAmt { get; set; }
        public decimal SubTotalAmt { get; set; }
        public decimal GrandTotalAmt { get; set; }
        public string DiscountName { get; set; }
        public decimal DiscountAmt { get; set; }
        public Nullable<decimal> ServiceChargeRate { get; set; }
        public decimal ServiceChargeAmt { get; set; }
        public string PrinterName { get; set; }

        public List<OrderTableForBillDo> Tables { get; set; }
        public List<OrderItemForBillDo> Items { get; set; }
        public List<OrderRemarkForBillDo> Remarks { get; set; }

        public string TableName
        {
            get
            {
                if (this.Tables != null)
                {
                    IDictionary<string, List<string>> tableDic = new Dictionary<string, List<string>>();
                    foreach (OrderTableForBillDo t in this.Tables)
                    {
                        string[] sp = t.TableName.Split(' ');
                        if (tableDic.ContainsKey(sp[0]) == false)
                            tableDic.Add(sp[0], new List<string>());
                        tableDic[sp[0]].Add(sp[1]);
                    }

                    string name = "";
                    foreach (string key in tableDic.Keys)
                    {
                        int currno = 0;
                        int step = 0;
                        string tno = "";
                        string lastno = "";
                        foreach (string no in tableDic[key])
                        {
                            string _no = no;
                            if (_no[0] == '0')
                                _no = _no.Substring(1);

                            int n = 0;
                            int.TryParse(_no, out n);
                            if (currno == 0)
                            {
                                currno = n;
                                tno = no;
                            }
                            else if (currno + step + 1 != n)
                            {
                                if (lastno != "")
                                    tno += "-" + lastno;
                                lastno = "";
                                step = 0;
                                currno = n;

                                tno += ", ";
                                tno += no;
                            }
                            else
                            {
                                step++;
                                lastno = no;
                            }
                        }
                        if (lastno != "")
                            tno += "-" + lastno;

                        if (name != "")
                            name += ", ";
                        name += string.Format("[{0} {1}]", key, tno);
                    }

                    return name;
                }

                return null;
            }
        }
    }
    public partial class OrderItemForBillDo
    {
        public string TransactionRefID { get; set; }
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }
        public int OrderID { get; set; }
        public int ItemID { get; set; }
        public string BranchName { get; set; }
        public int CustomerQty { get; set; }
        public string CreateUser { get; set; }
        public Nullable<int> BillNo { get; set; }
        public Nullable<System.DateTime> PrintDate { get; set; }
        public int PrintNo { get; set; }
        public string ApproveUser { get; set; }
        public decimal TotalAmt { get; set; }
        public decimal TotalLineDiscountAmt { get; set; }
        public decimal TotalPromotionDiscountAmt { get; set; }
        public decimal TotalPromotionVoucherAmt { get; set; }
        public decimal SubTotalAmt { get; set; }
        public decimal GrandTotalAmt { get; set; }
        public string DiscountName { get; set; }
        public decimal DiscountAmt { get; set; }
        public Nullable<decimal> ServiceChargeRate { get; set; }
        public decimal ServiceChargeAmt { get; set; }
        public string MenuName { get; set; }
        public Nullable<int> Qty { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string CommentType { get; set; }
        public string CommentName { get; set; }
        public Nullable<int> CommentQty { get; set; }
        public Nullable<decimal> CommentPrice { get; set; }
        public string PrinterName { get; set; }
    }
    public partial class OrderRemarkForBillDo
    {
        public string TransactionRefID { get; set; }
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }
        public int OrderID { get; set; }
        public int OldBillNo { get; set; }
        public string OldTableName { get; set; }
        public int OldPrintNo { get; set; }
        public string OldApproveUser { get; set; }
        public int OldOrderID { get; set; }
        public int LogID { get; set; }
    }
    public partial class OrderTableForBillDo
    {
        public string TransactionRefID { get; set; }
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }
        public int OrderID { get; set; }
        public int TableID { get; set; }
        public string TableName { get; set; }
    }
}
