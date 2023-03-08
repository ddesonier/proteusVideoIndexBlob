using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

internal class GetURIs
{

    private string connectionString;
    public GetURIs(string connectionString)
    {
        this.connectionString = connectionString;
    }
    public string getString() { return connectionString; }

    public async Task<List<string>> GetContainersList()
    {

        BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
        //Console.WriteLine("Connection in GetURI:   " + connectionString);

        string prefix = "";
        int segmentSize = 2;
        List<string> containerList = new List<string>();

        try
        {
            // Call the listing operation and enumerate the result segment.
            var resultSegment =
                blobServiceClient.GetBlobContainersAsync(BlobContainerTraits.Metadata, prefix, default)
                .AsPages(default, segmentSize);
            //Console.WriteLine("In Try");
            await foreach (Azure.Page<BlobContainerItem> containerPage in resultSegment)
            {
                //Console.WriteLine("containerPage.Values.Count: " + containerPage.Values.Count);
                foreach (BlobContainerItem containerItem in containerPage.Values)
                {
                    //Console.WriteLine("containerItem.Name: " + containerItem.Name);
                    containerList.Add(containerItem.Name);
                }
            }

        }
        catch (RequestFailedException e)
        {
            Console.WriteLine(e.Message);
            Console.ReadLine();
            throw;
        }
        String[] str = containerList.ToArray();
        return containerList;
    }

    public List<string> GetVideoURIs(string containerName)
    {

        BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);

        List<string> videoURIList = new List<string>();




        foreach (BlobItem blob in blobContainerClient.GetBlobs())
        {
            string urlblob = containerName + "/" + blob.Name;
            //Console.WriteLine("New One:  " + blobContainerClient.GetBlobBaseClientCore(blob.Name));
            videoURIList.Add(urlblob);
            //Console.WriteLine(urlblob);
        }

        return videoURIList;
    }
}
