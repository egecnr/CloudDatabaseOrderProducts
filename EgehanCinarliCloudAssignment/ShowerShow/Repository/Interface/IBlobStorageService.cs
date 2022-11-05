using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Threading.Tasks;
using System.IO;

namespace UserAndOrdersFunction.Repository.Interface
{
    public interface IBlobStorageService
    {
        public Task CreateProductPictureInBlob(Stream requestBody, Guid userId);
        public Task DeleteProductPictureInBlob(Guid userId);
        public Task<HttpResponseData> GetProductPictureByProductId(HttpResponseData response, Guid userId);
    }
}
