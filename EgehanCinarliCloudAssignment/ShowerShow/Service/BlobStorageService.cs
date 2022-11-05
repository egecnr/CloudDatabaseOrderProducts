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

        public async Task DeleteProductPictureInBlob(Guid productId)
        {
            if (!await userService.CheckIfUserExist(productId))
                throw new ArgumentException("The user does not exist or is inactive.");
            await blobStorageRepository.DeleteProductPictureInBlob(productId);
        }

        public async Task<HttpResponseData> GetProductPictureByProductId(HttpResponseData response, Guid productId)
        {
            if (!await userService.CheckIfUserExist(productId))
                throw new ArgumentException("The user does not exist or is inactive.");

            return await blobStorageRepository.GetProductPictureByProductId(response, productId);
        }

      

        public async Task CreateProductPictureInBlob(Stream requestBody, Guid productId)
        {        
            await blobStorageRepository.CreateProductPictureInBlob(requestBody, productId);
        }

   
    }
}
