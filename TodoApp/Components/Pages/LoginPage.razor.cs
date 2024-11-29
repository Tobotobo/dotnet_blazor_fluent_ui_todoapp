using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using TodoApp.Components.Dialogs;
using TodoApp.Models;

namespace TodoApp.Components.Pages;

public partial class LoginPage: ComponentBase
{
    [Inject] ITodoService todoService { get; init; } = null!;
    [Inject] IDialogService dialogService { get; init; } = null!;
    [Inject] IToastService toastService { get; init;} = null!;

    private string? UserId { get; set; }
    private string? Password { get; set; }

    private async Task Login() {
        await todoService.Login(UserId ?? "", Password ?? "");
        if (todoService.IsLoggedIn) {
            toastService.ShowToast(ToastIntent.Info, "ログインしました", 0);
        } else {
            toastService.ShowToast(ToastIntent.Error, "ログインに失敗しました", 0);
        }
    }
}