using Microsoft.FluentUI.AspNetCore.Components;
using TodoApp.Components;
using TodoApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddFluentUIComponents();

TodoServiceInMemory.Users.AddRange([
    new User(Id:"user1", Password:"user1", IsAdmin:false),
    new User(Id:"user2", Password:"user2", IsAdmin:false),
    new User(Id:"admin", Password:"admin", IsAdmin:true),
]);
TodoServiceInMemory.Todos.AddRange([
    new Todo(Id:"1", Title:"タスクAを完了する", IsCompleted:true, UserId:"admin"),
    new Todo(Id:"2", Title:"ミーティングの準備", IsCompleted:true, UserId:"admin"),
    new Todo(Id:"3", Title:"新規プロジェクトの計画", IsCompleted:true, UserId:"user1"),
    new Todo(Id:"4", Title:"メールの返信", IsCompleted:false, UserId:"user1"),
    new Todo(Id:"5", Title:"ドキュメントのレビュー", IsCompleted:false, UserId:"user1"),
    new Todo(Id:"6", Title:"コードのリファクタリング", IsCompleted:true, UserId:"user2"),
    new Todo(Id:"7", Title:"進捗報告を作成", IsCompleted:true, UserId:"user1"),
    new Todo(Id:"8", Title:"データ分析の確認", IsCompleted:true, UserId:"user2"),
    new Todo(Id:"9", Title:"スケジュールの調整", IsCompleted:true, UserId:"user1"),
    new Todo(Id:"10", Title:"テストケースの作成", IsCompleted:false, UserId:"user2"),
    new Todo(Id:"11", Title:"デザイン案の確認", IsCompleted:true, UserId:"user1"),
    new Todo(Id:"12", Title:"予算案のチェック", IsCompleted:true, UserId:"user2"),
    new Todo(Id:"13", Title:"プロモーション戦略の立案", IsCompleted:true, UserId:"user1"),
    new Todo(Id:"14", Title:"製品仕様の確認", IsCompleted:true, UserId:"admin"),
    new Todo(Id:"15", Title:"システムアップデート", IsCompleted:false, UserId:"user1"),
    new Todo(Id:"16", Title:"サーバーメンテナンス", IsCompleted:true, UserId:"admin"),
    new Todo(Id:"17", Title:"ユーザー調査の実施", IsCompleted:true, UserId:"user2"),
    new Todo(Id:"18", Title:"報告書の提出", IsCompleted:false, UserId:"user1"),
    new Todo(Id:"19", Title:"社内勉強会の企画", IsCompleted:true, UserId:"admin"),
    new Todo(Id:"20", Title:"タスク一覧の整理", IsCompleted:false, UserId:"user1"),
    new Todo(Id:"21", Title:"バックアップの実施", IsCompleted:true, UserId:"admin"),
    new Todo(Id:"22", Title:"エラーログの解析", IsCompleted:true, UserId:"admin"),
    new Todo(Id:"23", Title:"プレゼン資料の作成", IsCompleted:true, UserId:"admin"),
    new Todo(Id:"24", Title:"顧客からのフィードバック確認", IsCompleted:true, UserId:"admin"),
    new Todo(Id:"25", Title:"SNS投稿の準備", IsCompleted:false, UserId:"user2"),
    new Todo(Id:"26", Title:"契約書の見直し", IsCompleted:false, UserId:"user1"),
    new Todo(Id:"27", Title:"市場調査の結果分析", IsCompleted:true, UserId:"user1"),
    new Todo(Id:"28", Title:"新機能の提案", IsCompleted:false, UserId:"admin"),
    new Todo(Id:"29", Title:"会議室の予約", IsCompleted:true, UserId:"admin"),
    new Todo(Id:"30", Title:"シフト表の作成", IsCompleted:true, UserId:"user2"),
]);
TodoServiceInMemory.LastAssignedTodoId = 3;
builder.Services.AddScoped<ITodoService, TodoServiceInMemory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
