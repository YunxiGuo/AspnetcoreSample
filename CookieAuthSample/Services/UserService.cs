using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CookieAuthSample.Services.Dtos;
using EntityFrameWorkCore.Entities;
using EntityFrameWorkCore.Repository;

namespace CookieAuthSample.Services
{
    public class UserService : IUserService
    {
        private UserDbContext _dbContext;

        public UserService(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateUser(CreateUserInput input)
        {
            await _dbContext.Users.AddAsync(new User
            {
                Email = input.Email,
                IdentityType = input.IdentityType,
                Password = input.Password,
                Name = input.Name
            });
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<User>> GetUsers()
        {
            throw new System.NotImplementedException();
        }

        public async Task<User> GetUser(int id)
        {
            throw new System.NotImplementedException();
        }

        public async Task DeleteUser(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}