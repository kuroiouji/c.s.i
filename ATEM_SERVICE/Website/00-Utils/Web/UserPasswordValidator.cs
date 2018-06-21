using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;

namespace Web.Validator
{
    public class UserPasswordValidator<TUser> : IPasswordValidator<TUser> where TUser : class
    {
        public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager,
                                              TUser user,
                                              string password)
        {
            return Task.Run<IdentityResult>(() =>
            {
                //int passwordLength = 8;
                //if (password.Length >= passwordLength)
                //{
                //    int foundCondition = 0;
                //    if (password.Any(x => char.IsUpper(x))) //Uppercase
                //        foundCondition++;
                //    if (password.Any(x => char.IsLower(x))) //Lowercase
                //        foundCondition++;
                //    if (password.Any(x => char.IsDigit(x))) //Digit
                //        foundCondition++;

                //    string specialCharacters = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
                //    char[] specialCharactersArray = specialCharacters.ToCharArray();

                //    int index = password.IndexOfAny(specialCharactersArray);
                //    if (index >= 0)
                //        foundCondition++;

                //    if (foundCondition >= 2)
                //        return IdentityResult.Success;
                //}
                if (password.Length >= 4)
                    return IdentityResult.Success;

                return IdentityResult.Failed(new IdentityError
                {
                    Code = "CLE012"
                });
            });
        }
    }
}
