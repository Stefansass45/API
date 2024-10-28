using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace prjAuthApi.Data
{
    public class AuthDbContex : IdentityDbContext<IdentityUser>
    {
        public AuthDbContex(DbContextOptions options) : base(options)
        {

        }
    }
}
