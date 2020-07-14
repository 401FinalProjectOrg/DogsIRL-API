using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace DogsIRL_API.Models
{
    public class Blob
    {
        public CloudStorageAccount CloudStorageAccount { get; set; }
        public CloudBlobClient CloudBlobClient { get; set; }

        public Blob(IConfiguration configuration)
        {
            var storageCreds = new StorageCredentials(configuration["Storage-Account-Name"], configuration["BlobKey"]);
            CloudStorageAccount = new CloudStorageAccount(storageCreds, true);
            CloudBlobClient = CloudStorageAccount.CreateCloudBlobClient();
        }

        public async Task<CloudBlobContainer> GetContainer(string containerName)
        {
            CloudBlobContainer cbc = CloudBlobClient.GetContainerReference(containerName);
            await cbc.CreateIfNotExistsAsync();
            await cbc.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Container
            });
            return cbc;
        }

        public async Task<CloudBlob> GetBlob(string imgName, string containerName)
        {
            var container = await GetContainer(containerName);
            CloudBlob blob = container.GetBlobReference(imgName);
            return blob;
        }

        public async Task UploadFile(string containerName, string fileName, string filePath)
        {
            var container = await GetContainer(containerName);
            var blobFile = container.GetBlockBlobReference(fileName);
            await blobFile.UploadFromFileAsync(filePath);
        }

        public async Task DeleteBlob(string containerName, string fileName)
        {
            var container = await GetContainer(containerName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            await blockBlob.DeleteAsync();
        }
    }
}
