using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity.DataSeed
{
    public static  class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync( UserManager<AppUser> _userManager )
        {
            if ( await _userManager.Users.CountAsync() == 0)
            {
                var user = new AppUser()
                {
                    DisplayName = "Emad Abdelhady",
                    Email = "mado3603@gmail.com",
                    UserName = "Emad.Abdelhady",
                    PhoneNumber = "01069766948",
                };

               await  _userManager.CreateAsync(user,"Pa$$W0rd");

            }

        }

    }
}
