﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Frends.AzureBlobStorage.UploadBlob.Definitions;

/// <summary>
/// Source parameters.
/// </summary>
public class Source
{
    /// <summary>
    /// Source type.
    /// </summary>
    /// <example>UploadSourceType.File</example>
    [DefaultValue(UploadSourceType.File)]
    public UploadSourceType SourceType { get; set; }

    /// <summary>
    /// Upload all files from the given directory.
    /// </summary>
    /// <example>c:\temp</example>
    [UIHint(nameof(SourceType), "", UploadSourceType.Directory)]
    [DisplayFormat(DataFormatString = "Text")]
    public string SourceDirectory { get; set; }

    /// <summary>
    /// The search string is used to match against the names of files in the path. 
    /// This parameter can contain a combination of valid literal path and wildcard (* and ?) characters, but it doesn't support regular expressions
    /// </summary>
    /// <example>*.*, Search*.*, *.xml</example>
    [UIHint(nameof(SourceType), "", UploadSourceType.Directory)]
    [DisplayFormat(DataFormatString = "Text")]
    public string SearchPattern { get; set; }

    /// <summary>
    /// This file will be uploaded as a new blob or appended into the target blob in the Append process.
    /// </summary>
    /// <example>c:\temp\testfile.txt</example>
    [UIHint(nameof(SourceType), "", UploadSourceType.File)]
    [DisplayFormat(DataFormatString = "Text")]
    public string SourceFile { get; set; }

    /// <summary>
    /// Name of the blob. If left empty, the blob's name will be the same as the source file.
    /// </summary>
    /// <example>Renamed.txt</example>
    [UIHint(nameof(SourceType), "", UploadSourceType.File)]
    [DisplayFormat(DataFormatString = "Text")]
    public string BlobName { get; set; }

    /// <summary>
    /// Name of the blob folder. If left empty, blob folder's name will be the same as source directory (e.g., 'Example' in C:\temp\Example).
    /// </summary>
    /// <example>ExampleDir</example>
    [UIHint(nameof(SourceType), "", UploadSourceType.Directory)]
    [DisplayFormat(DataFormatString = "Text")]
    public string BlobFolderName { get; set; }

    /// <summary>
    /// Use a stream to read the file's content
    /// </summary>
    /// <example>false</example>
    [UIHint(nameof(SourceType), "", UploadSourceType.File)]
    [DefaultValue(false)]
    public bool ContentsOnly { get; set; }

    /// <summary>
    /// Gzip compression only works when transferring stream content (see Source.ContentsOnly). 
    /// Note that it could be a good idea to rename the blob using Destination.BlobName (e.g., renaming 'examplefile.txt' to 'examplefile.gz') so that the blob won't be named as 'examplefile.txt.gz
    /// </summary>
    /// <example>false</example>
    [UIHint(nameof(SourceType), "", UploadSourceType.File)]
    [DefaultValue(false)]
    [DisplayName("Gzip compression")]
    public bool Compress { get; set; }

    /// <summary>
    /// Tags for the block or append blob.
    /// </summary>
    /// <example>{name, value}</example>
    public Tag[] Tags { get; set; }
}

/// <summary>
/// Tag parameters.
/// </summary>
public class Tag
{
    /// <summary>
    /// Name of the tag.
    /// </summary>
    /// <example>Name</example>
    [DisplayFormat(DataFormatString = "Text")]
    public string Name { get; set; }

    /// <summary>
    /// Value of the tag.
    /// </summary>
    /// <example>Value</example>
    [DisplayFormat(DataFormatString = "Text")]
    public string Value { get; set; }
}