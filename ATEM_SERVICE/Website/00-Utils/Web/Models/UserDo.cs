using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Models.User
{
    public class UserDo
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int GroupID { get; set; }
        public bool FlagActive { get; set; }
        public bool FlagSystemAdmin { get; set; }
        public string Email { get; set; }
        public int? PasswordAge { get; set; }
        public DateTime? LastUpdatePasswordDate { get; set; }
        public string Remark { get; set; }

        public UserDo()
        {
            this.FlagActive = true;
            this.FlagSystemAdmin = false;
        }
    }

    public class UserLoginDo
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string WithPermissionID { get; set; }
    }
    public class UserPasswordDo
    {
        public string Username { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public DateTime? LastUpdatePasswordDate { get; set; }
    }
}
