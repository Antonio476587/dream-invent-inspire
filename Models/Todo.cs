using System.ComponentModel.DataAnnotations;
using dii.storage.Attributes;
using dii.storage.Models.Interfaces;
using MessagePack;
using Microsoft.Azure.Cosmos;

namespace TodoApp.Models;

public class Todo : IDiiEntity
{		
	/// <summary>
	/// The Todo ID and partitionKey
	/// </summary>
    [PartitionKey(typeof(PartitionKey))]
    [Id]
    public string id { get; set; } = Guid.NewGuid().ToString();
    /// <summary>
	/// The Todo Title
	/// </summary>
	[Searchable("Tittle")]
    public string? Title { get; set; }
    /// <summary>
	/// When the Todo has been created
	/// </summary>
    [DataType(System.ComponentModel.DataAnnotations.DataType.Date)]
	[Searchable("CreatedAt")]
    public DateTime CreatedAt { get; set; }
    [Compress(0)]	
    /// <summary>
	/// The Todo Content
	/// </summary>
    public string? Content { get; set; }
    /// <summary>
	/// The Todo Status
	/// </summary>
    [Searchable("Status")]
    public int Status { get; set; }
		
    [IgnoreMember]
    public Version SchemaVersion => new(1, 0);

    public string? DataVersion { get; set; }
}