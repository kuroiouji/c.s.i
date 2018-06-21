using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public partial class OrderTaxDo
    {
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }
        public int OrderID { get; set; }
        public int OrderTaxID { get; set; }
        public string OrderStatus { get; set; }
        public string OrderStatusName { get; set; }
        public System.DateTime TaxCreateDate { get; set; }

        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string BranchShortNameEN { get; set; }
        public string BranchShortNameTH { get; set; }
        public string BranchAddress { get; set; }
        public string CantonID { get; set; }
        public string CantonName { get; set; }
        public string DistrictID { get; set; }
        public string DistrictName { get; set; }
        public string ProvinceID { get; set; }
        public string ProvinceName { get; set; }
        public string ZipCode { get; set; }

        public int CustomerID { get; set; }
        public string TaxNo { get; set; }
        public string CustomerName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressTel { get; set; }
        public string AddressFax { get; set; }
        public string CustomerTaxID { get; set; }

        public Nullable<decimal> TotalAmtBeforeDisc { get; set; }
        public decimal? ServiceChargeRate { get; set; }
        public Nullable<decimal> ServiceChargeAmt { get; set; }
        public Nullable<int> DiscountID { get; set; }
        public string DiscountName { get; set; }
        public Nullable<decimal> DiscountAmt { get; set; }
        public Nullable<decimal> TotalLineDiscountAmt { get; set; }
        public Nullable<decimal> TotalAmtAfterDisc { get; set; }
        public Nullable<int> TotalLine { get; set; }
        public Nullable<decimal> PaymentVoucherValue { get; set; }
        public Nullable<decimal> PaymentCashValue { get; set; }
        public Nullable<decimal> PaymentCreditValue { get; set; }

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
                        string[] sp = t.TableName.Split(' ');
                        if (tableDic.ContainsKey(sp[0]) == false)
                            tableDic.Add(sp[0], new List<string>());
                        tableDic[sp[0]].Add(sp[1]);
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

    public class OrderTaxResultDo : Utils.Interfaces.ASearchResultData<OrderTaxFSDo>
    {
        public decimal? SummaryValue { get; set; }
    }
    public partial class OrderTaxFSDo
    {
        public System.DateTime PrintDate { get; set; }
        public string TaxNo { get; set; }
        public bool FlagExport { get; set; }
        public string CustomerName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string AddressTel { get; set; }
        public string AddressFax { get; set; }
        public string CustomerTaxID { get; set; }
        public string CustomerBranchCode { get; set; }

        public decimal? TotalAmt { get; set; }
        public decimal? TotalLineDiscountAmt { get; set; }
        public decimal? TotalPromotionDiscountAmt { get; set; }
        public decimal? TotalPromotionVoucherAmt { get; set; }
        public decimal? SubTotalAmt { get; set; }
        public decimal? DiscountAmt { get; set; }
        public decimal? ServiceChargeAmt { get; set; }
        public decimal? GrandTotalAmt { get; set; }
        public decimal? VatAmt { get; set; }
        public decimal? GrandTotalAmtExcludeVat { get; set; }

        public int OrderID { get; set; }
        public string OrderStatus { get; set; }
        public string OrderStatusName { get; set; }
        public int OrderTaxID { get; set; }
        public string BranchName { get; set; }
        public Nullable<int> BranchID { get; set; }
    }

    public partial class RePrintOrderTaxDo
    {
        public string FrontEndID { get; set; }
        public int BranchID { get; set; }
        public int SHIFT { get; set; }
        public int OrderID { get; set; }
        public string TransactionRefID { get; set; }

        public string OrderTaxRePrintReason { get; set; }

        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }
}
