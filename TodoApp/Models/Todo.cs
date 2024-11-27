namespace TodoApp.Models;

public record Todo(
    string? Id = null,
    string? Title = null,
    bool? IsCompleted = null,
    string? UserId = null
);

// public class Todo {
//     public string? Id { get; set; }
//     public string? Title { get; set; }
//     public bool? IsCompleted { get; set; }
//     public string? UserId { get; set; }
// }