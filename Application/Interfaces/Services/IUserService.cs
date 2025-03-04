using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetUsers();
        public Task<UserDTO> GetUserById(int id);

        Task<bool> CreateUser(UserDTO userDTO);
        Task<bool> UpdateUser(int id,UserDTO userDTO);
        Task DeleteUser(int id);
    }
}
