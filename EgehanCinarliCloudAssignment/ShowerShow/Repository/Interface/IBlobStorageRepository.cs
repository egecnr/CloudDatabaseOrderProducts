using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Threading.Tasks;
using System.IO;

namespace UserAndOrdersFunction.Repository.Interface
{
    public interface IBlobStorageRepository
    {
        public Task UploadProfilePicture(Stream requestBody, Guid userId);
        public Task DeleteProfilePicture(Guid userId);
        public Task<HttpResponseData> GetProfilePictureOfUser(HttpResponseData response, Guid userId);      
    }
}
