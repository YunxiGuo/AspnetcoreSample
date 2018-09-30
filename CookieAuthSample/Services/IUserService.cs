using System.Collections.Generic;
using System.Threading.Tasks;
using CookieAuthSample.Services.Dtos;
using EntityFrameWorkCore.Entities;

namespace CookieAuthSample.Services
{
    public interface IUserService
    {
        Task CreateUser(CreateUserInput input);

        Task<List<User>> GetUsers();

        Task<User> GetUser(int id);

        Task DeleteUser(int id);
    }
}