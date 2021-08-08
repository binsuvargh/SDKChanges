using Azure.Identity;
using Azure.Storage.Blobs;
using System;

namespace bob_container
{
    class AzureSettings
    {
        public static BlobClient getBlobClient(string blobName)
        {
            return getCloudClient().GetBlobClient($"virtualDirectory/{blobName}");
        }
        public static BlobContainerClient getCloudClient()
        {
            var storageName = "sa12410e1dv01";
            var contName = "testcontainer";
            var credential = new DefaultAzureCredential();
            var containerClient = new BlobContainerClient(new Uri($"https://{storageName}.blob.core.windows.net/{contName}"), credential);
            return containerClient;
        }
    }
}
