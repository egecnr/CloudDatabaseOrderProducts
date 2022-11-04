using Microsoft.Azure.Functions.Worker.Http;
using UserAndOrdersFunction.Repository.Interface;
using System;
using System.IO;
using System.Threading.Tasks;

namespace UserAndOrdersFunction.Service
{
    public class BlobStorageService : IBlobStorageService
    {
        private IBlobStorageRepository blobStorageRepository;
        private IUserService userService;

        public BlobStorageService(IBlobStorageRepository blobStorageRepository,IUserService userService)
        {
            this.blobStorageRepository = blobStorageRepository;
            this.userService = userService;
        }

        public async Task DeleteProfilePicture(Guid userId)
        {
            if (!await userService.CheckIfUserExist(userId))
                throw new ArgumentException("The user does not exist or is inactive.");
            await blobStorageRepository.DeleteProfilePicture(userId);
        }

        public async Task<HttpResponseData> GetProfilePictureOfUser(HttpResponseData response, Guid userId)
        {
            if (!await userService.CheckIfUserExist(userId))
                throw new ArgumentException("The user does not exist or is inactive.");

            return await blobStorageRepository.GetProfilePictureOfUser(response, userId);
        }

      

        public async Task UploadProfilePicture(Stream requestBody, Guid userId)
        {
          /*  if (!await userService.CheckIfUserExist(userId))
                throw new ArgumentException("The user does not exist or is inactive.");*/

            await blobStorageRepository.UploadProfilePicture(requestBody, userId);
        }

   
    }
}
