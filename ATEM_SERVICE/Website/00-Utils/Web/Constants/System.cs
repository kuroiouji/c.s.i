using System;
using System.Collections.Generic;
using System.Text;

namespace Web
{
    public partial class Constants
    {
        public static int HQ { get; private set; }
        public static string MEMBER_PRINTER { get; private set; }
        public static string VOUCHER_PRINTER { get; private set; }
        public static string CASHDRAWER_IP { get; private set; }
        public static string CASHDRAWER_USER { get; private set; }
        public static string CASHDRAWER_PASSWORD { get; private set; }
        public static string CASHDRAWER_PATH { get; private set; }

        public static void UpdateCashdrawer(string ip, string u, string p, string ph)
        {
            Type t = typeof(Web.Constants);

            t.GetProperty("CASHDRAWER_IP").SetValue(null, ip, null);
            t.GetProperty("CASHDRAWER_USER").SetValue(null, u, null);
            t.GetProperty("CASHDRAWER_PASSWORD").SetValue(null, p, null);
            t.GetProperty("CASHDRAWER_PATH").SetValue(null, ph, null);
        }
    }
}
