using UserAndOrdersFunction.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserAndOrdersFunction.Repository.Interface
{
    public interface IUserService
    {
        Task<bool> CheckIfUserExist(Guid userId);
        Task<IEnumerable<GetUserDTO>> GetUsersByName(string userName);
        Task CreateUser(CreateUserDTO userDTO);
        Task AddUserToQueue(CreateUserDTO user);
        Task<GetUserDTO> GetUserById(Guid Id);

    }
}