using System;
using System.IO;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure;
using Azure.Storage.Blobs.Specialized;

namespace bob_container
{
    class Program
    {
        static void Main(string[] args)
        {
           // Upload("azurepdf.pdf");

            //Copy("blobicon.jfif");

            //Download("blobicon.jfif");

           Delete("blobicon.jfif");

            Console.ReadLine();
        }

        static void Upload(
            string fileName)

        { 
            Console.WriteLine("Uploading file...");

                //testing
                var downloadPath = Path.Combine("H:\\LocalBlobs", fileName);
                FileStream file = new FileStream(downloadPath, FileMode.Open, FileAccess.Read);

                var memoryStream = new MemoryStream();
                file.CopyTo(memoryStream);
                memoryStream.ToArray();
                memoryStream.Seek(0, SeekOrigin.Begin);

                try
                {
                    var blobClient = AzureSettings.getBlobClient(fileName);
                    blobClient.Upload(memoryStream);
                }
                catch (RequestFailedException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                Console.WriteLine("uploaded");
        }
       
        static void Download(
           string policyPacketKey)
        {
          
            Console.WriteLine("\nDownloading blob");

            var blobClient = AzureSettings.getBlobClient(policyPacketKey);

            var outstream = new MemoryStream();
             blobClient.DownloadTo(outstream);


            //testing
            var downloadPath = Path.Combine("H:\\LocalBlobs", policyPacketKey);
            FileStream file = new FileStream(downloadPath, FileMode.Create, FileAccess.Write);
            outstream.WriteTo(file);

            Console.WriteLine("Downloaded");
        }

        static void Copy(
           string fileName)
        {

            Console.WriteLine("\nCopying blob");

            try 
            { 
                var sourceBlob = AzureSettings.getBlobClient(fileName);
                if (sourceBlob.Exists())
                {
                    // Lease the source blob 
                    BlobLeaseClient lease = sourceBlob.GetBlobLeaseClient();

                    // Specifying -1 for the lease interval creates an infinite lease.
                    lease.AcquireAsync(TimeSpan.FromSeconds(-1));

                    // Get the source blob's properties
                    BlobProperties sourceProperties = sourceBlob.GetProperties();

                    // Get a destination blob
                    BlobClient destBlob =
                        AzureSettings.getBlobClient($"New-{fileName}");

                    // Start the copy operation.
                     destBlob.StartCopyFromUri(sourceBlob.Uri);

                    // Update the source blob's properties.
                    sourceProperties = sourceBlob.GetProperties();

                    if (sourceProperties.LeaseState == LeaseState.Leased)
                    {
                        // Break the lease on the source blob.
                        lease.Break();
                    }
                }
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                throw;
            }
            Console.WriteLine("Copied");
        }

        static void Delete(string blobName)
        {
            Console.WriteLine("\nDeleting blob");
            try
            {
                var sourceBlob = AzureSettings.getBlobClient(blobName);
                sourceBlob.DeleteIfExists(DeleteSnapshotsOption.IncludeSnapshots);
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();
                throw;
            }
            Console.WriteLine("Deleted");
        }
    }
}
