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
        //containers and connection strings
        private string producPicContainerString = Environment.GetEnvironmentVariable("ContainerProductPictures");
        private string connection = Environment.GetEnvironmentVariable("AzureWebJobsStorage");

       // Cloud blob instances
        private CloudStorageAccount account;
        private CloudBlobClient blobClient;
        private CloudBlobContainer profilePicContainer;

        public BlobStorageRepository()
        {
            // initialize account & containers
            account = CloudStorageAccount.Parse(connection);
            blobClient = account.CreateCloudBlobClient();
            profilePicContainer = blobClient.GetContainerReference(producPicContainerString);

        }
        public async Task DeleteProfilePicture(Guid productId)
        {
            // get the picture blob
            CloudBlockBlob blockBlob = profilePicContainer.GetBlockBlobReference(productId.ToString() + ".png");
         
            if (!await profilePicContainer.ExistsAsync() || !await blockBlob.ExistsAsync()) //check for validity
                throw new Exception("The specified container does not exist.");

            await blockBlob.DeleteIfExistsAsync(); //delete picture blob
        }

        public async Task<HttpResponseData> GetProfilePictureOfUser(HttpResponseData response, Guid productId)
        {

            //get the picture blob
            CloudBlockBlob blockBlob = profilePicContainer.GetBlockBlobReference(productId.ToString() + ".png");

            if (!await profilePicContainer.ExistsAsync())
                throw new Exception("The specified container does not exist.");

            // if the user does not have a picture, assign it the default picture
            if (!await blockBlob.ExistsAsync())
            {
                blockBlob = profilePicContainer.GetBlockBlobReference("defaultpicture.png");
                if (!await blockBlob?.ExistsAsync()) //if there is no default picture, throw exception
                    throw new Exception("Unable to load profile picture.");

            }
            return GetDownloadResponseData(response, blockBlob, "image/jpeg").Result;
        }

        public async Task UploadProfilePicture(Stream requestBody, Guid userId)
        {
            // parse a Stream to a form
            var parsedFormBody = MultipartFormDataParser.ParseAsync(requestBody);
            // get the form's first file
            var file = parsedFormBody.Result.Files[0];

            //create the container if it doesn't exist
            await profilePicContainer.CreateIfNotExistsAsync();

            // create the picture blob
            CloudBlockBlob blockBlob = profilePicContainer.GetBlockBlobReference(userId.ToString() + ".png");
            // make sure the blob content type is the same as the input file
            blockBlob.Properties.ContentType = file.ContentType;

            // check if the input file is an image | content-type e.g: 'image/jpeg'
            if (!blockBlob.Properties.ContentType.Contains("image"))
                throw new BadImageFormatException("You must input an image file.");

            await blockBlob.UploadFromStreamAsync(file.Data); //upload image to container
        }

        public async Task<HttpResponseData> GetDownloadResponseData(HttpResponseData responseData, CloudBlockBlob blockBlob, string ContentType)
        {
            // returns a response data containing the attachment file (audio/image), and the appropiate headers
            using (MemoryStream ms = new MemoryStream())
            {
                //download the blob contents into the stream
                await blockBlob.DownloadToStreamAsync(ms);
                byte[] content = ms.ToArray();
                responseData.WriteBytes(content); //write the content (audio/image) as bytes
                //set specific headers
                responseData.Headers.Add("Content-Type", ContentType);
                responseData.Headers.Add("Accept-Ranges", $"bytes"); // allow bytes
                //content info
                responseData.Headers.Add("Content-Disposition", $"attachment; filename={blockBlob.Name}; filename*=UTF-8'{blockBlob.Name}");
            }
            return responseData;
        }
    }
}
