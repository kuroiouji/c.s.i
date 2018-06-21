using System;
using System.Collections.Generic;
using System.Text;

namespace POS.DataSvc.Models
{
    public class CashdrawerInfoDo
    {
        public string IPAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Path { get; set; }
        public string AppPath { get; set; }

        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }
}
