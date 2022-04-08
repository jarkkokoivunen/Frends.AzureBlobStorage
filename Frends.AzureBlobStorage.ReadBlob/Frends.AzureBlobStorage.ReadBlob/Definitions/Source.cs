﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
#pragma warning disable 1591
namespace Frends.AzureBlobStorage.ReadBlob.Definitions
{
    public class Source
    {
        /// <summary>
        ///     The base URI for the storage account.
        ///     Use either URI and SAS Token or Connection string.
        /// </summary>
        [DefaultValue("https://xx.blob.xx.xx.net/")]
        [DisplayFormat(DataFormatString = "Text")]
        public string Uri { get; set; }

        /// <summary>
        ///     A shared access signature. Grants restricted access rights to Azure Storage resources when combined with URI.
        /// </summary>
        [PasswordPropertyText]
        [DisplayFormat(DataFormatString = "Text")]
        public string SasToken { get; set; }

        /// <summary>
        ///     Connection string to Azure storage.
        ///     Use either URI and SAS Token or Connection string.
        /// </summary>
        [PasswordPropertyText]
        [DisplayFormat(DataFormatString = "Text")]
        public string ConnectionString { get; set; }

        /// <summary>
        ///     Name of the azure blob storage container from where blob data is located.
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string ContainerName { get; set; }

        /// <summary>
        ///    Name of the blob which content is read.
        /// </summary>
        [DisplayFormat(DataFormatString = "Text")]
        public string BlobName { get; set; }

        /// <summary>
        ///     Encoding name in which blob content is read.
        /// </summary>
        [DefaultValue("UTF8")]
        [DisplayFormat(DataFormatString = "Text")]
        public Encode Encoding { get; set; }
    }
}
