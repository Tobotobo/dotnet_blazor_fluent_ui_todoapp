namespace TodoApp.Models;

public interface ITodoService {
    User? LoginUser { get; }
    bool IsLoggedIn { get; }
    Task<User?> Login(string userId, string password);
    Task Logout();
    Task<Todo?> GetTodo(string todoId);
    Task<Todo[]> GetTodos(bool admin = false);
    Task<Todo?> AddTodo(Todo todo);
    Task<Todo?> UpdateTodo(Todo todo);
    Task<Todo?> RemoveTodo(Todo todo);
}