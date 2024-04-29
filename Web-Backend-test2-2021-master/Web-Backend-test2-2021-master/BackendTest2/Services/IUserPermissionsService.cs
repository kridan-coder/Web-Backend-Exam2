using System;
using System.Threading.Tasks;

namespace BackendTest2.Services
{
    public interface IUserPermissionsService
    {
        Task<String> GetCurrentUserId();
    }
}