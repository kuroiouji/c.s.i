using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class SlipOrderDo
    {
        public int BranchID { get; set; }
        public int OrderID { get; set; }
        public Nullable<int> BillNo { get; set; }
        public string Status { get; set; }
        public string TaxNo { get; set; }
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmt { get; set; }
        public Nullable<decimal> TotalLineDiscountAmt { get; set; }
        public string DiscountName { get; set; }
        public decimal DiscountAmt { get; set; }
        public Nullable<decimal> TotalPromotionDiscountAmt { get; set; }
        public Nullable<decimal> TotalPromotionVoucherAmt { get; set; }
        public Nullable<decimal> ServiceChargeRate { get; set; }
        public Nullable<decimal> ServiceChargeAmt { get; set; }
        public decimal SubTotalAmt { get; set; }
        public decimal GrandTotalAmt { get; set; }
        public Nullable<int> Qty { get; set; }
        public Nullable<decimal> CashAmt { get; set; }
        public Nullable<decimal> CreditAmt { get; set; }
        public Nullable<decimal> VoucherAmt { get; set; }
        public string CreateUser { get; set; }
        public string UpdateUser { get; set; }

        public List<OrderTableDo> Tables { get; set; }
        public List<OrderItemGroupDo> Items { get; set; }

        public string OrderTableHeader
        {
            get
            {
                if (this.Tables != null)
                {
                    IDictionary<string, List<string>> tableDic = new Dictionary<string, List<string>>();
                    foreach (OrderTableDo t in this.Tables)
                    {
                        if (Utils.CommonUtil.IsNullOrEmpty(t.TableName) == false)
                        {
                            string[] sp = t.TableName.Split(' ');
                            if (tableDic.ContainsKey(sp[0]) == false)
                                tableDic.Add(sp[0], new List<string>());
                            tableDic[sp[0]].Add(sp[1]);
                        }
                    }

                    string header = "";
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

                        if (header != "")
                            header += ", ";
                        header += string.Format("[{0} {1}]", key, tno);
                    }

                    return header;
                }

                return null;
            }
        }
    }
}
