using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using TodoApp.Components.Dialogs;
using TodoApp.Models;

namespace TodoApp.Components.Pages;

public partial class TodoPage: ComponentBase
{
    [Inject] ITodoService todoService { get; init; } = null!;
    [Inject] IDialogService dialogService { get; init; } = null!;
    [Inject] IToastService toastService { get; init;} = null!;

    private IQueryable<Todo>? todos;

    protected override async Task OnInitializedAsync()
    {
        await todoService.Login("user1", "user1");
        await LoadTodos();
    }

    private async Task LoadTodos() {
        todos = (await todoService.GetTodos())
            .AsQueryable();
    }

    private async Task UpdateTodo(Todo? todo) 
    {
        // ヘッダーをクリックした際も実行され null になる 
        if (todo == null) {
            return;
        }

        // 最新の情報を取得
        var curTodo = await todoService.GetTodo(todo.Id!);
        if (curTodo == null) {
            // 取得できなかった場合は既に削除されている
            toastService.ShowToast(ToastIntent.Error, "対象のレコードが見つかりません。", 0);
            await LoadTodos();
            return;
        }

        // 更新画面表示
        DialogParameters parameters = new()
        {
            Title = $"更新",
            PrimaryAction = "更新",
            SecondaryAction = "キャンセル",
            Modal = true,
            PreventDismissOnOverlayClick = true, // true = ダイアログの外側をクリックしても閉じない
            TrapFocus = true, // true = ダイアログがフォーカスをトラップする（false にするとタブ移動でダイアログ外にまでフォーカスが移動する）
            ShowDismiss = false, // false = ヘッダーの閉じるボタンを非表示
            PreventScroll = true, // true = ダイアログの外側へのスクロールを防止
        };
        var dialog = await dialogService.ShowDialogAsync<TodoUpdateDialog>(new DialogData<Todo>() { Value = curTodo }, parameters);
        var result = await dialog.Result;
        if (result.Cancelled)
        {
            return;
        }
        
        // 更新内容をDBに反映
        var newTodo = (result.Data as DialogData<Todo>)!.Value!;
        await todoService.UpdateTodo(newTodo);
        await LoadTodos();
        toastService.ShowToast(ToastIntent.Success, "更新しました", 3000);
    }
}