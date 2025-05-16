using Booking.Core.Enities.Authentication;
using Microsoft.AspNetCore.Identity;
using Booking.Infrasturcture.Data;
using Microsoft.EntityFrameworkCore;
using Booking.Infrasturcture.Helpers;

namespace Booking.Infrasturcture.DatabaseInitilizer
{
        public class DBIntializer : IDBIntializer
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly RoleManager<IdentityRole> _roleManager;
            private readonly BookingDbContext _context;
            public DBIntializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, BookingDbContext context)
            {
                _userManager = userManager;
                _roleManager = roleManager;
                _context = context;
            }
            public void Initialize()
            {
                // Update Migration to databse
                try
                {
                    if (_context.Database.GetPendingMigrations().Count() > 0)
                    {
                        _context.Database.Migrate();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

                // Create Roles
                if (!_roleManager.RoleExistsAsync(Roles.AdminRole).GetAwaiter().GetResult())
                {
                    _roleManager.CreateAsync(new IdentityRole(Roles.AdminRole)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(Roles.EditorRole)).GetAwaiter().GetResult();
                    _roleManager.CreateAsync(new IdentityRole(Roles.UserRole)).GetAwaiter().GetResult();

                    // Create Admin User
                    _userManager.CreateAsync(new ApplicationUser
                    {
                        UserName = "Admin",
                        Email = "Admin@booking.com",
                        PhoneNumber = "01124338655"
                    }, "P@$$w0rd").GetAwaiter().GetResult();

                    ApplicationUser adminUser = _context.Users.FirstOrDefault(e => e.Email == "Admin@booking.com");

                    if (adminUser != null)
                    {
                        _userManager.AddToRoleAsync(adminUser, Roles.AdminRole).GetAwaiter().GetResult();
                    }
                }
            }
        }
}
