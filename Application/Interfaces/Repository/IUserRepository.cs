using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Repository
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetUsers();
        public Task<User> GetUserById(int id);
        Task CreateUser(User user);
        Task<bool> EmailExistis(string email);
        Task UpdateUser(int id, User user);
        Task DeleteUser(int id);
    }
}
