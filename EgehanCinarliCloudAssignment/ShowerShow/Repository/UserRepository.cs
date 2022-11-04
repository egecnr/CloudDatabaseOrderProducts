using AutoMapper;
using Azure.Storage.Queues;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Extensions;
using UserAndOrdersFunction.DAL;
using UserAndOrdersFunction.DTO;
using UserAndOrdersFunction.Model;
using UserAndOrdersFunction.Models;
using UserAndOrdersFunction.Repository.Interface;
using UserAndOrdersFunction.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace UserAndOrdersFunction.Repository
{
    public class UserRepository:IUserRepository
    {
        private DatabaseContext dbContext;
   

        public UserRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task AddUserToQueue(CreateUserDTO userDTO)
        {            
                string qName = Environment.GetEnvironmentVariable("CreateUserQueue");
                string connString = Environment.GetEnvironmentVariable("AzureWebJobsStorage");
                QueueClientOptions clientOpt = new QueueClientOptions() { MessageEncoding = QueueMessageEncoding.Base64 };

                QueueClient qClient = new QueueClient(connString, qName, clientOpt);
                var jsonOpt = new JsonSerializerOptions() { WriteIndented = true };
                string userJson = JsonSerializer.Serialize<CreateUserDTO>(userDTO, jsonOpt);
                await qClient.SendMessageAsync(userJson);                            
        }
        public async Task CreateUser(CreateUserDTO userDTO)
        {
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<CreateUserDTO, User>()));
            User fullUser = mapper.Map<User>(userDTO);
            fullUser.Password = PasswordHasher.HashPassword(fullUser.Password);
            dbContext.Users?.Add(fullUser);
            await dbContext.SaveChangesAsync();
        }
        public async Task<GetUserDTO> GetUserById(Guid userId)
        {
            await dbContext.SaveChangesAsync();
            User user = dbContext.Users.FirstOrDefault(u => u.Id == userId);
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<User, GetUserDTO>()));
            GetUserDTO userDTO = mapper.Map<GetUserDTO>(user);
            return userDTO;
        }
        public async Task<IEnumerable<GetUserDTO>> GetUsersByName(string userName)
        {
            List<User> usersWithName = dbContext.Users.Where(u => u.UserName.ToLower().StartsWith(userName.ToLower())).ToList();
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<User, GetUserDTO>()));
            List<GetUserDTO> dtos = ConvertGetDtos(usersWithName);
            return dtos;
        }
        public async Task<bool> CheckIfUserExist(Guid userId)
        {
            await dbContext.SaveChangesAsync();
            if (dbContext.Users.Count(x => x.Id == userId) > 0)
                return true;
            else
                return false;
        }
        public async Task<bool> CheckIfUserNameExist(string userName)
        {
            await dbContext.SaveChangesAsync();
            if (dbContext.Users.Count(x => x.UserName.ToLower() == userName.ToLower()) > 0)
                return true;
            else
                return false;
        }
        public async Task<bool> CheckIfEmailExist(string email)
        {
            await dbContext.SaveChangesAsync();
            if (dbContext.Users.Count(x => x.EmailAddress == email)>0)
                return true;
            else
                return false;
        }
  
        private List<GetUserDTO> ConvertGetDtos(List<User> users)
        {
            Mapper mapper = AutoMapperUtil.ReturnMapper(new MapperConfiguration(con => con.CreateMap<User, GetUserDTO>()));
            List<GetUserDTO> userdtos = new List<GetUserDTO>();

            users.ForEach(delegate (User u) {
                userdtos.Add(mapper.Map<GetUserDTO>(u));
            });
            return userdtos;
        }

        
    }
}
