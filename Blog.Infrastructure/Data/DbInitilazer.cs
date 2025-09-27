using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Data
{
    public static class DbInitilazer
    {
        // Add Roles
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            // List Of Roles
            var Roles = new List<string> { "Admin", "Reader" , "Editor" };
            foreach(var Role in Roles)
            {
                if(! await roleManager.RoleExistsAsync(Role)) // add role
                {
                    var Result = await roleManager.CreateAsync(
                        new IdentityRole(Role));
                    if (Result.Succeeded)
                    {
                        Console.WriteLine($"Role {Role} Created Successfully");
                    }
                    else
                    {
                        foreach (var Error in Result.Errors)
                            Console.WriteLine($"Seeding Roles : {Error.Code} - {Error.Description}");
                    }
                }
            }
        }
    }
}
