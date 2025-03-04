using Application.DTOs;
using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<bool> CreateUser(UserDTO userDTO)
        {
            //Check if email existis
            var response = await _userRepository.EmailExistis(userDTO.Email);
            if (!response)
            {
                var user = _mapper.Map<User>(userDTO);
                await _userRepository.CreateUser(user);
                return true;
            }
            return false;
        }

        public async Task DeleteUser(int id)
        {
            await _userRepository.DeleteUser(id);
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            var user = await _userRepository.GetUserById(id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            var users = await _userRepository.GetUsers();
            return _mapper.Map<List<UserDTO>>(users);
        }

        public async Task<bool> UpdateUser(int id, UserDTO userDTO)
        {
            //Check if user existis
            var userFound = await _userRepository.GetUserById(id);
            if (!string.IsNullOrEmpty(userFound.Email))
            {
                bool isEmailExists = false;
                //check if email is different from original and chekc if email exists
                if (userFound.Email != userDTO.Email)
                {
                    isEmailExists = await _userRepository.EmailExistis(userDTO.Email);
                }
                if (!isEmailExists)
                {
                    var user = _mapper.Map<User>(userDTO);
                    await _userRepository.UpdateUser(id, user);
                    return true;
                }
            }
            return false;
        }
    }
}
