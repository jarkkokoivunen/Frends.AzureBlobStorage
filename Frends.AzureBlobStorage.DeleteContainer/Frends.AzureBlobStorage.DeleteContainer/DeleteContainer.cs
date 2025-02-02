﻿using Azure.Identity;
using Azure.Storage.Blobs;
using Frends.AzureBlobStorage.DeleteContainer.Definitions;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;

namespace Frends.AzureBlobStorage.DeleteContainer;

/// <summary>
/// Azure Blob Storage Task.
/// </summary>
public class AzureBlobStorage
{

    /// For mem cleanup.
    static AzureBlobStorage()
    {
        var currentAssembly = Assembly.GetExecutingAssembly();
        var currentContext = AssemblyLoadContext.GetLoadContext(currentAssembly);
        if (currentContext != null)
            currentContext.Unloading += OnPluginUnloadingRequested;
    }

    /// <summary>
    /// Deletes a container from Azure blob storage.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.AzureBlobStorage.DeleteContainer)
    /// </summary>
    /// <param name="input">Information about the container destination.</param>
    /// <param name="options">Options regarding the error handling.</param>
    /// <param name="cancellationToken">Token generated by Frends to stop this task.</param>
    /// <returns>Object { string ContainerWasDeleted, string Message }</returns>
    public static async Task<Result> DeleteContainer([PropertyTab] Input input, [PropertyTab] Options options, CancellationToken cancellationToken)
    {
        if (input.ConnectionMethod is ConnectionMethod.OAuth2 && (input.ApplicationID is null || input.ClientSecret is null || input.TenantID is null || input.StorageAccountName is null))
            throw new ArgumentNullException("Input.StorageAccountName, Input.ClientSecret, Input.ApplicationID and Input.TenantID parameters can't be empty when Input.ConnectionMethod = OAuth.");
        if (string.IsNullOrWhiteSpace(input.ConnectionString) && input.ConnectionMethod is ConnectionMethod.ConnectionString)
            throw new ArgumentNullException("ConnectionString parameter can't be empty when Input.ConnectionMethod = ConnectionString.");
        if(string.IsNullOrWhiteSpace(input.ContainerName))
            throw new ArgumentNullException("ContainerName parameter can't be empty.");

        try
        {
            var container = GetBlobContainer(input);
        
            if (!await container.ExistsAsync(cancellationToken) && !options.ThrowErrorIfContainerDoesNotExists) 
                return new Result(false, "Container not found.");
            else if (!await container.ExistsAsync(cancellationToken) && options.ThrowErrorIfContainerDoesNotExists) 
                throw new Exception("DeleteContainer error: Container not found.");

            var result = await container.DeleteIfExistsAsync(null, cancellationToken);
            return new Result(result, "Container deleted successfully.");
        }
        catch (Exception e)
        {
            throw new Exception("DeleteContaine: Error occured while trying to delete blob container.", e);
        }
    }

    private static BlobContainerClient GetBlobContainer(Input input)
    {
        try
        {
            BlobServiceClient client;

            if (input.ConnectionMethod is ConnectionMethod.ConnectionString)
                client = new BlobServiceClient(input.ConnectionString);
            else
            {
                var credentials = new ClientSecretCredential(input.TenantID, input.ApplicationID, input.ClientSecret, new ClientSecretCredentialOptions());
                client = new BlobServiceClient(new Uri($"https://{input.StorageAccountName}.blob.core.windows.net"), credentials);
            }

            return client.GetBlobContainerClient(input.ContainerName);
        }
        catch (Exception ex)
        {
            throw new Exception(@$"GetBlobContainer error {ex}");
        }
    }

    private static void OnPluginUnloadingRequested(AssemblyLoadContext obj)
    {
        obj.Unloading -= OnPluginUnloadingRequested;
    }
}