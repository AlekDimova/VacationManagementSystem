using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VacationManagementSystem.Models;

namespace VacationManagementSystem.Data
{
    public class SeedData
    {
        public static async Task Initialize(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                // Apply pending migrations
                context.Database.Migrate();

                // Create Roles
                if (!await roleManager.RoleExistsAsync("Employee"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Employee"));
                }

                if (!await roleManager.RoleExistsAsync("HR"))
                {
                    await roleManager.CreateAsync(new IdentityRole("HR"));
                }

                // Create Admin User (if it doesn't exist)
                var adminUser = await userManager.FindByEmailAsync("admin@example.com");
                if (adminUser == null)
                {
                    adminUser = new IdentityUser { UserName = "admin@example.com", Email = "admin@example.com", EmailConfirmed = true }; //Added EmailConfirmed
                    var result = await userManager.CreateAsync(adminUser, "Alex1234^"); // Change this password!

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "HR");

                        // Create a corresponding Employee record
                        if (!context.Employees.Any(e => e.Email == "admin@example.com"))
                        {
                            context.Employees.Add(new Employee { FirstName = "Admin", LastName = "User", Email = "admin@example.com" });
                            await context.SaveChangesAsync();
                        }

                        // Create a corresponding LeaveBalance record
                        var adminFromDb = await context.Employees.FirstOrDefaultAsync(e => e.Email == "admin@example.com");
                        if (adminFromDb != null && !context.LeaveBalances.Any(lb => lb.EmployeeId == adminFromDb.EmployeeId))
                        {
                            context.LeaveBalances.Add(new LeaveBalance { EmployeeId = adminFromDb.EmployeeId, AnnualLeaveDays = 20, BonusLeaveDays = 5 });
                            await context.SaveChangesAsync();
                        }
                    }
                }

                // Create Employee User (if it doesn't exist)
                var employeeUser = await userManager.FindByEmailAsync("employee@example.com");
                if (employeeUser == null)
                {
                    employeeUser = new IdentityUser { UserName = "employee@example.com", Email = "employee@example.com", EmailConfirmed = true }; //Added EmailConfirmed
                    var result = await userManager.CreateAsync(employeeUser, "Zozo#123"); // Change password as needed

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(employeeUser, "Employee");

                        // Create a corresponding Employee record
                        if (!context.Employees.Any(e => e.Email == "employee@example.com"))
                        {
                            context.Employees.Add(new Employee { FirstName = "Default", LastName = "Employee", Email = "employee@example.com" });
                            await context.SaveChangesAsync();
                        }

                        // Create a corresponding LeaveBalance record
                        var employeeFromDb = await context.Employees.FirstOrDefaultAsync(e => e.Email == "employee@example.com");
                        if (employeeFromDb != null && !context.LeaveBalances.Any(lb => lb.EmployeeId == employeeFromDb.EmployeeId))
                        {
                            context.LeaveBalances.Add(new LeaveBalance { EmployeeId = employeeFromDb.EmployeeId, AnnualLeaveDays = 20, BonusLeaveDays = 5 });
                            await context.SaveChangesAsync();
                        }
                    }
                }
            }
        }
    }
}