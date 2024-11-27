namespace TodoApp.Models;

public record User(
    string? Id,
    string? Password,
    bool? IsAdmin
);

// public class User {
//     public string? Id { get; set; }
//     public string? Password { get; set; }
//     public bool? IsAdmin { get; set; }
// }