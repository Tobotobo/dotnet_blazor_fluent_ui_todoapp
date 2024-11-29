using TodoApp.Models;
using FluentAssertions;

namespace TodoApp.Tests.Models;

public class TodoServiceInMemoryTests
{
    TodoServiceInMemory todoService = default!;

    [SetUp]
    public void Setup()
    {
        if (TodoServiceInMemory.Users.Count == 0) {
            TodoServiceInMemory.Users.AddRange([
                new(Id:"user1", Password:"user1", IsAdmin:false),
                new(Id:"user2", Password:"user2", IsAdmin:false),
                new(Id:"admin", Password:"admin", IsAdmin:true),
            ]);
        }
        todoService = new TodoServiceInMemory();

    }

    [TestCase("user1", "user1", false)]
    [TestCase("user2", "user2", false)]
    [TestCase("admin", "admin", true)]
    public async Task Login_ログインできること(string userId, string password, bool isAdmin)
    {
        User? user = await todoService.Login(userId, password);
        user.Should()
            .NotBeNull()
            .And.Match<User>(x => x.Id == userId)
            .And.Match<User>(x => x.IsAdmin == isAdmin);
        todoService.IsLoggedIn.Should().BeTrue();
        todoService.LoginUser.Should().Be(user);
    }

    [TestCase("", "")]
    [TestCase("user1", "")]
    [TestCase("", "user1")]
    [TestCase("user1", "user2")]
    [TestCase("user2", "user1")]
    public async Task Login_ログインできないこと(string userId, string password)
    {
        User? user = await todoService.Login(userId, password);
        user.Should().BeNull();
        todoService.IsLoggedIn.Should().BeFalse();
        todoService.LoginUser.Should().BeNull();
    }

    [Test]
    public async Task Logout_ログアウトされること() {
        await todoService.Login("user1", "user1");
        await todoService.Logout();
        todoService.IsLoggedIn.Should().BeFalse();
        todoService.LoginUser.Should().BeNull();
    }

    [TestCase("user1", "user1", "あいうえお１２３４５", false)]
    [TestCase("admin", "admin", "aiueo12345", true)]
    public async Task AddTodo_自分に追加できること(string userId, string password, string? title, bool? isCompleted)
    {
        await todoService.Login(userId, password);
        Todo? newTodo = await todoService.AddTodo(new(
            Title:title,
            IsCompleted:isCompleted,
            UserId:userId
        ));
        newTodo.Should()
            .NotBeNull()
            .And.Match<Todo>(x => x.Id != null)
            .And.Match<Todo>(x => x.Title == title)
            .And.Match<Todo>(x => x.IsCompleted == isCompleted)
            .And.Match<Todo>(x => x.UserId == userId);
        Todo? todo = await todoService.GetTodo(newTodo!.Id!);
        todo.Should().Be(newTodo);
    }

    [TestCase("admin", "admin", "aiueo12345", true, "user1")]
    public async Task AddTodo_管理者は他人に追加できること(string userId, string password, string? title, bool? isCompleted, string ownerUserId)
    {
        await todoService.Login(userId, password);
        Todo? newTodo = await todoService.AddTodo(new(
            Title:title,
            IsCompleted:isCompleted,
            UserId: ownerUserId
        ));
        newTodo.Should()
            .NotBeNull()
            .And.Match<Todo>(x => x.Id != null)
            .And.Match<Todo>(x => x.Title == title)
            .And.Match<Todo>(x => x.IsCompleted == isCompleted)
            .And.Match<Todo>(x => x.UserId == ownerUserId);
        Todo? todo = await todoService.GetTodo(newTodo!.Id!);
        todo.Should().Be(newTodo);
    }

    [TestCase("admin", "admin", "aiueo12345", true, "user3")]
    public async Task AddTodo_管理者でも存在しないIdには追加できないこと(string userId, string password, string? title, bool? isCompleted, string ownerUserId)
    {
        await todoService.Login(userId, password);
        Todo? newTodo = await todoService.AddTodo(new(
            Title:title,
            IsCompleted:isCompleted,
            UserId: ownerUserId
        ));
        newTodo.Should().BeNull();
    }

    [TestCase("user1", "user1", "1", "aaaaaa", true)]
    [TestCase("admin", "admin", "2", "iiiiii", false)]
    public async Task UpdateTodo_自分のタスクを更新できること(string userId, string password, string taskId, string? title, bool? isCompleted)
    {
        await todoService.Login("admin", "admin");
        foreach(var todo in new Todo[]{
            new(Title:"", IsCompleted:false, UserId:"user1"),
            new(Title:"aaaa", IsCompleted:true, UserId:"admin"),
        }) {
            await todoService.AddTodo(todo);
        }
        await todoService.Logout();
        
        await todoService.Login(userId, password);
        Todo? newTodo = await todoService.UpdateTodo(new(
            Id:taskId,
            Title:title,
            IsCompleted:isCompleted,
            UserId: userId
        ));
        newTodo.Should()
            .NotBeNull()
            .And.Match<Todo>(x => x.Id != null)
            .And.Match<Todo>(x => x.Title == title)
            .And.Match<Todo>(x => x.IsCompleted == isCompleted)
            .And.Match<Todo>(x => x.UserId == userId);
        {
            Todo? todo = await todoService.GetTodo(newTodo!.Id!);
            todo.Should().Be(newTodo);
        }
    }

}