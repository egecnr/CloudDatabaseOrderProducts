using UserAndOrdersFunction.Repository.Interface;
using System;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker.Http;
using HttpMultipartParser;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

namespace UserAndOrdersFunction.Repository
{
    public class BlobStorageRepository : IBlobStorageRepository
    {
        //Some of the methods are from the cloud project.
        private string producPicContainerString = Environment.GetEnvironmentVariable("ContainerProductPictures");
        private string connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

     
        private CloudStorageAccount account;
        private CloudBlobClient blobClient;
        private CloudBlobContainer productPictureContainer;

        public BlobStorageRepository()
        {
            
            account = CloudStorageAccount.Parse(connection);
            blobClient = account.CreateCloudBlobClient();
            productPictureContainer = blobClient.GetContainerReference(producPicContainerString);

        }
        public async Task DeleteProductPictureInBlob(Guid productId)
        {
         
            CloudBlockBlob blockBlob = productPictureContainer.GetBlockBlobReference(productId.ToString() + ".png");
         
            if (!await productPictureContainer.ExistsAsync() || !await blockBlob.ExistsAsync()) 
                throw new Exception("The specified container does not exist.");

            await blockBlob.DeleteIfExistsAsync(); 
        }

        public async Task<HttpResponseData> GetProductPictureByProductId(HttpResponseData response, Guid productId)
        {

            CloudBlockBlob blockBlob = productPictureContainer.GetBlockBlobReference(productId.ToString() + ".png");

            if (!await productPictureContainer.ExistsAsync())
                throw new Exception("Container doesn't exist");

            if (!await blockBlob.ExistsAsync())
            {
                blockBlob = productPictureContainer.GetBlockBlobReference("defaultpicture.png");
                if (!await blockBlob?.ExistsAsync()) 
                    throw new Exception("Can not load the picture");

            }
            return GetDownloadResponseData(response, blockBlob, "image/jpeg").Result;
        }

        public async Task CreateProductPictureInBlob(Stream requestBody, Guid productId)
        {
          
            var parsedFormBody = MultipartFormDataParser.ParseAsync(requestBody);
            
            var file = parsedFormBody.Result.Files[0];

            
            await productPictureContainer.CreateIfNotExistsAsync();

            
            CloudBlockBlob blockBlob = productPictureContainer.GetBlockBlobReference(productId.ToString() + ".png");
           
            blockBlob.Properties.ContentType = file.ContentType;

            
            if (!blockBlob.Properties.ContentType.Contains("image"))
                throw new BadImageFormatException("You must input an image file.");

            await blockBlob.UploadFromStreamAsync(file.Data); 
        }

        public async Task<HttpResponseData> GetDownloadResponseData(HttpResponseData responseData, CloudBlockBlob blockBlob, string ContentType)
        {
           
            using (MemoryStream ms = new MemoryStream())
            {
         
                await blockBlob.DownloadToStreamAsync(ms);
                byte[] content = ms.ToArray();
                responseData.WriteBytes(content);
                
                responseData.Headers.Add("Content-Type", ContentType);
                responseData.Headers.Add("Accept-Ranges", $"bytes");
               
                responseData.Headers.Add("Content-Disposition", $"attachment; filename={blockBlob.Name}; filename*=UTF-8'{blockBlob.Name}");
            }
            return responseData;
        }
    }
}
