using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendTest2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace BackendTest2.Services
{
    public class UserPermissionsService : IUserPermissionsService
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;

        public UserPermissionsService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        private HttpContext HttpContext => this.httpContextAccessor.HttpContext;
        public async Task<String> GetCurrentUserId()
        {
            var user = await this.userManager.GetUserAsync(this.HttpContext.User);
            return user.Id;
        }
    }
}
