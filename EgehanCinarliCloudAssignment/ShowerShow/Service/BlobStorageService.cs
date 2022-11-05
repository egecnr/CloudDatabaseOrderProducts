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
        private IProductService productService;

        public BlobStorageService(IBlobStorageRepository blobStorageRepository,IProductService productService)
        {
            this.blobStorageRepository = blobStorageRepository;
            this.productService = productService;
        }

        public async Task DeleteProductPictureInBlob(Guid productId)
        {
            if(await productService.CheckIfProductExist(productId))
            {
                await blobStorageRepository.DeleteProductPictureInBlob(productId);

            }
            else
            {
                throw new Exception($"Product with product id {productId} does not exist");
            
            }
        }

        public async Task<HttpResponseData> GetProductPictureByProductId(HttpResponseData response, Guid productId)
        {
            if (await productService.CheckIfProductExist(productId))
            {
                return await blobStorageRepository.GetProductPictureByProductId(response, productId);
            }
            else
            {
                throw new Exception($"Product with product id {productId} does not exist");

            }
        }    
        public async Task CreateProductPictureInBlob(Stream requestBody, Guid productId)
        {        
            await blobStorageRepository.CreateProductPictureInBlob(requestBody, productId);
        }

   
    }
}
