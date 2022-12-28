﻿using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Frends.AzureBlobStorage.DeleteBlob.Definitions;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;

namespace Frends.AzureBlobStorage.DeleteBlob;

/// <summary>
/// Azure Blob Storage task.
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
    /// Delete a single blob from Azure Blob Storage.
    /// [Documentation](https://tasks.frends.com/tasks/frends-tasks/Frends.AzureBlobStorage.DeleteBlob)
    /// </summary>
    /// <param name="input">Input parameters.</param>
    /// <param name="options">Options parameters</param>
    /// <param name="cancellationToken">Token generated by Frends to stop this task.</param>
    /// <returns>Object { bool Success, string Info }</returns>
    public static async Task<Result> DeleteBlob([PropertyTab] Input input, [PropertyTab] Options options,CancellationToken cancellationToken)
    {
        if (input.ConnectionMethod is ConnectionMethod.OAuth2 && (input.ApplicationID is null || input.ClientSecret is null || input.TenantID is null || input.StorageAccountName is null))
            throw new Exception("Input.StorageAccountName, Input.ClientSecret, Input.ApplicationID and Input.TenantID parameters can't be empty when Input.ConnectionMethod = OAuth.");
        if (string.IsNullOrWhiteSpace(input.ConnectionString) && input.ConnectionMethod is ConnectionMethod.ConnectionString)
            throw new Exception("ConnectionString parameter can't be empty when Input.ConnectionMethod = ConnectionString.");

        try
        {
            var blob = GetBlobClient(input);

            if (!await blob.ExistsAsync(cancellationToken) && !options.ThrowErrorIfBlobDoesNotExists) 
                return new Result (false, $"Blob {input.BlobName} doesn't exists in container {input.ContainerName}.");
            if (!await blob.ExistsAsync(cancellationToken) && options.ThrowErrorIfBlobDoesNotExists)
                throw new Exception($"Blob {input.BlobName} doesn't exists in container {input.ContainerName}.");

            var accessCondition = string.IsNullOrWhiteSpace(options.VerifyETagWhenDeleting)
                ? new BlobRequestConditions { IfMatch = new Azure.ETag(options.VerifyETagWhenDeleting) }
                : null;

            var result = await blob.DeleteIfExistsAsync(
                options.SnapshotDeleteOption.ConvertEnum<DeleteSnapshotsOption>(), accessCondition,
                cancellationToken);

            return new Result(result, $"Blob {input.BlobName} deleted from container {input.ContainerName}.");
        }
        catch (Exception e)
        {
            throw new Exception("DeleteBlobAsync: An error occured while trying to delete blob", e);
        }
    }

    private static BlobClient GetBlobClient(Input input)
    {
        if (input.ConnectionMethod is ConnectionMethod.ConnectionString)
            return new BlobClient(input.ConnectionString, input.ContainerName, input.BlobName);
        else
        {
            var credentials = new ClientSecretCredential(input.TenantID, input.ApplicationID, input.ClientSecret, new ClientSecretCredentialOptions());
            var url = new Uri($"https://{input.StorageAccountName}.blob.core.windows.net/{input.ContainerName}/{input.BlobName}");
            return new BlobClient(url, credentials);
        }
    }

    private static void OnPluginUnloadingRequested(AssemblyLoadContext obj)
    {
        obj.Unloading -= OnPluginUnloadingRequested;
    }
}