using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendTest2.Data;
using BackendTest2.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BackendTest2
{
    public static class DbMigration
    {
        public static IWebHost MigrateDatabase(this IWebHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                context.Database.Migrate();
                DbMigration.ConfigureIdentity(scope).GetAwaiter().GetResult();
            }

            return webHost;
        }

        private static async Task ConfigureIdentity(IServiceScope scope)
        {
            var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

            var adminsRole = await roleManager.FindByNameAsync(ApplicationRoles.Administrators);
            var studentsRole = await roleManager.FindByNameAsync(ApplicationRoles.Students);
            var teachersRole = await roleManager.FindByNameAsync(ApplicationRoles.Teachers);



            if (adminsRole == null)
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(ApplicationRoles.Administrators));
                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException($"Unable to create {ApplicationRoles.Administrators} role.");
                }

                adminsRole = await roleManager.FindByNameAsync(ApplicationRoles.Administrators);
            }

            if (studentsRole == null)
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(ApplicationRoles.Students));
                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException($"Unable to create {ApplicationRoles.Students} role.");
                }

                studentsRole = await roleManager.FindByNameAsync(ApplicationRoles.Students);
            }


            if (teachersRole == null)
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(ApplicationRoles.Teachers));
                if (!roleResult.Succeeded)
                {
                    throw new InvalidOperationException($"Unable to create {ApplicationRoles.Teachers} role.");
                }

                teachersRole = await roleManager.FindByNameAsync(ApplicationRoles.Teachers);
            }





            var adminUser = await userManager.FindByNameAsync("admin@localhost.local");
            if (adminUser == null)
            {
                var userResult = await userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@localhost.local",
                    Email = "admin@localhost.local"
                }, "AdminPass123!");
                if (!userResult.Succeeded)
                {
                    throw new InvalidOperationException($"Unable to create admin@localhost.local user");
                }

                adminUser = await userManager.FindByNameAsync("admin@localhost.local");
            }

            if (!await userManager.IsInRoleAsync(adminUser, adminsRole.Name))
            {
                await userManager.AddToRoleAsync(adminUser, adminsRole.Name);
            }


            var studentUser = await userManager.FindByNameAsync("student@localhost.local");
            if (studentUser == null)
            {
                var userResult = await userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "student@localhost.local",
                    Email = "student@localhost.local"
                }, "StudentPass123!");
                if (!userResult.Succeeded)
                {
                    throw new InvalidOperationException($"Unable to create student@localhost.local user");
                }

                studentUser = await userManager.FindByNameAsync("student@localhost.local");
            }

            if (!await userManager.IsInRoleAsync(studentUser, studentsRole.Name))
            {
                await userManager.AddToRoleAsync(studentUser, studentsRole.Name);
            }



            var teacherUser = await userManager.FindByNameAsync("teacher@localhost.local");
            if (teacherUser == null)
            {
                var userResult = await userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "teacher@localhost.local",
                    Email = "teacher@localhost.local"
                }, "TeacherPass123!");
                if (!userResult.Succeeded)
                {
                    throw new InvalidOperationException($"Unable to create teacher@localhost.local user");
                }

                teacherUser = await userManager.FindByNameAsync("teacher@localhost.local");
            }

            if (!await userManager.IsInRoleAsync(teacherUser, teachersRole.Name))
            {
                await userManager.AddToRoleAsync(teacherUser, teachersRole.Name);
            }
        }
    }
}
