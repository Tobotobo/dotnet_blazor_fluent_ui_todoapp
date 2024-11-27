namespace TodoApp.Models;

public class TodoServiceInMemory : ITodoService{
    private static readonly List<User> Users = [
        new(Id:"user1", Password:"user1", IsAdmin:false),
        new(Id:"user2", Password:"user2", IsAdmin:false),
        new(Id:"admin", Password:"admin", IsAdmin:true),
    ];
    private static readonly List<Todo> Todos = [];
    private static int LastAssignedTodoId = 0;
    private static readonly object TodosLock = new();

    public User? LoginUser { get; private set; }

    public bool IsLoggedIn { get => LoginUser != null; }

    public Task<User?> Login(string userId, string password) {
        return Task.Run(() => {
            User? user = Users.FirstOrDefault(x => x != null && x.Id == userId && x.Password == password, null);
            LoginUser = user;
            return user;
        });
    }

    public Task Logout() {
        return Task.Run(() => {
            LoginUser = null;
        });
    }

    public Task<Todo?> GetTodo(string todoId) {
        return Task.Run(() => {
            return GetTodoInternal(todoId);
        });
    }

    private Todo? GetTodoInternal(string todoId) {
        if (!IsLoggedIn) return null;
        bool isAdmin = LoginUser!.IsAdmin ?? false;
        lock(TodosLock) {
            Todo? todo = Todos.FirstOrDefault(x => 
                x != null 
                    && x.Id == todoId 
                    && (isAdmin || x.UserId == LoginUser!.Id)
                , null);
            return todo;
        }
    }

    public Task<Todo[]> GetTodos(bool admin = false) {
        return Task.Run(() => {
            if (!IsLoggedIn) return [];
            bool isAdmin = LoginUser!.IsAdmin ?? false;
            lock(TodosLock) {
                Todo[] todos = Todos.Where(x => 
                    x != null 
                        // 管理者ユーザーが管理者で実行した場合は全件、それ以外は自分のタスクのみ返す
                        && ((admin && isAdmin) || x.UserId == LoginUser!.Id)
                    ).ToArray();
                return todos;
            }
        });
    }

    public Task<Todo?> AddTodo(Todo todo) {
        return Task.Run(() => {
            if (!IsLoggedIn) return null;
            bool isAdmin = LoginUser!.IsAdmin ?? false;

            // 存在しないユーザーを指定された場合は登録しない
            if (todo.UserId == null 
                || Users.FirstOrDefault(x => x != null && x.Id == todo.UserId, null) == null) return null;
            
            // 非管理者は自分以外に登録不可
            if (!isAdmin) { 
                if (LoginUser.Id != todo.UserId) {
                    return null;
                }
            }

            // 登録
            lock(TodosLock) {
                int newId = LastAssignedTodoId + 1;
                Todo newTodo = todo with {
                    Id = newId.ToString(),
                    UserId = todo.UserId,
                };
                Todos.Add(newTodo);
                LastAssignedTodoId = newId;
                return newTodo;
            }
        });
    }

    public Task<Todo?> UpdateTodo(Todo todo) {
        return Task.Run(() => {
            if (!IsLoggedIn) return null;
            bool isAdmin = LoginUser!.IsAdmin ?? false;

            // 存在しないユーザーを指定された場合は更新しない
            if (todo.UserId == null 
                || Users.FirstOrDefault(x => x != null && x.Id == todo.UserId, null) == null) return null;
            
            // 非管理者は自分以外を更新不可
            if (!isAdmin) { 
                if (LoginUser.Id != todo.UserId) {
                    return null;
                }
            }

            // 更新
            lock(TodosLock) {
                Todo? oldTodo = GetTodoInternal(todo.Id!);
                if (oldTodo == null) return null;
                Todo newTodo = oldTodo with {
                    Title = todo.Title,
                    IsCompleted = todo.IsCompleted,
                    UserId = todo.UserId
                };
                Todos.Remove(oldTodo);
                Todos.Add(newTodo);
                return newTodo;
            }
        });
    }

    public Task<Todo?> RemoveTodo(Todo todo) {
        return Task.Run(() => {
            if (!IsLoggedIn) return null;
            bool isAdmin = LoginUser!.IsAdmin ?? false;

            // 存在しないユーザーを指定された場合は削除しない
            if (todo.UserId == null 
                || Users.FirstOrDefault(x => x != null && x.Id == todo.UserId, null) == null) return null;
            
            // 非管理者は自分以外は削除不可
            if (!isAdmin) { 
                if (LoginUser.Id != todo.UserId) {
                    return null;
                }
            }

            // 削除
            lock(TodosLock) {
                Todo? oldTodo = GetTodoInternal(todo.Id!);
                if (oldTodo == null) return null;
                Todos.Remove(oldTodo);
                return oldTodo;
            }
        });
    }
}