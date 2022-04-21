﻿namespace Frends.AzureBlobStorage.ReadBlob.Definitions
{
    /// <summary>
    ///     Returns encoded blob content.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Encoded blob content.
        /// </summary>
        /// <example>"line1\r\nline2\r\nline3"</example>
        public string Content { get; private set; }

        internal Result(string content)
        {
            Content = content;
        }
    }
}

