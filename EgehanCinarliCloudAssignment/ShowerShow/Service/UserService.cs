using AutoMapper;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using UserAndOrdersFunction.DTO;
using UserAndOrdersFunction.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserAndOrdersFunction.Service
{
    public class UserService : IUserService
    {
        private IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task AddUserToQueue(CreateUserDTO user)
        {
            if (await userRepository.CheckIfEmailExist(user.EmailAddress))
            {
                throw new Exception("This email address is already being used");
            }
            else if (await userRepository.CheckIfUserNameExist(user.UserName))
            {
                throw new Exception("This username is already being used");
            }
            else
            {
                await userRepository.AddUserToQueue(user);
            }
        }
        public async Task<GetUserDTO> GetUserById(Guid Id)
        {
            if (await userRepository.CheckIfUserExist(Id))
            {
                return await userRepository.GetUserById(Id);
            }
            else
            {
                throw new Exception("User does not exist");
            }
        }
        public async Task CreateUser(CreateUserDTO userDTO)
        {
            await userRepository.CreateUser(userDTO);
        }
        public async Task<IEnumerable<GetUserDTO>> GetUsersByName(string userName)
        {
            return await userRepository.GetUsersByName(userName);
        }

        public async Task<bool> CheckIfUserExist(Guid userId)
        {
            return await userRepository.CheckIfUserExist(userId);
        }
    }
}
