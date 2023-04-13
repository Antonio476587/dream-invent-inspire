using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models;

public class Todo
{
    public System.Guid id { get; set; }
    public int Id { get; set; }
    public string? Title { get; set; }
    [DataType(DataType.Date)]
    public DateTime createdAt { get; set; }
    public string? Content { get; set; }
    public decimal Status { get; set; }
}