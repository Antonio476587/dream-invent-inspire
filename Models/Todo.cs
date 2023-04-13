using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models;

public class Todo
{
    public System.Guid id { get; set; }
    public string? Title { get; set; }
    [DataType(DataType.Date)]
    public DateTime CreatedAt { get; set; }
    public string? Content { get; set; }
    public int Status { get; set; }
}