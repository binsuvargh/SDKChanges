using Azure.Identity;
using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bob_container
{
    class AzureSettings
    {
        public static BlobClient getBlobClient(string blobName)
        {
            return getCloudClient().GetBlobClient(blobName);
        }
        public static BlobContainerClient getCloudClient()
        {
            var storageName = "sa12410e1dv05";
            var contName = "demoblob";
            var credential = new DefaultAzureCredential();
            var test = credential.GetType();
            var containerClient = new BlobContainerClient(new Uri($"https://{storageName}.blob.core.windows.net/{contName}"), credential);
            return containerClient;
        }
    }
}
