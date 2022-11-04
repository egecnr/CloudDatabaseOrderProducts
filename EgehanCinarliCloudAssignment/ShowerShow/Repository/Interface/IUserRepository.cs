using UserAndOrdersFunction.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserAndOrdersFunction.Repository.Interface
{
    public interface IUserRepository
    {

         Task<bool> CheckIfUserExist(Guid userId);
         Task<bool> CheckIfEmailExist(string email);
         Task<bool> CheckIfUserNameExist(string userName);
         Task<IEnumerable<GetUserDTO>> GetUsersByName(string userName);
         Task CreateUser(CreateUserDTO userDTO);
         Task AddUserToQueue(CreateUserDTO user);
         Task<GetUserDTO> GetUserById(Guid Id);
    }
}
