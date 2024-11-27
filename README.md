# dotnet_blazor_fluent_ui_todoapp

## ※開発中

## 概要
* Microsoft Fluent UI Blazor components を使って簡単な TODO アプリを作成する
* ログイン画面があり、自分が登録した TODO のみ閲覧・編集することができる
* 管理ユーザーは、他の人が登録した TODO も閲覧・編集することができる

Microsoft Fluent UI Blazor components  
https://github.com/microsoft/fluentui-blazor  
Microsoft 製品（Office 365、Microsoft Teams など）で使用される Fluent Design System に基づいた UI コンポーネントを Blazor で利用できるようにした Microsoft 公式のライブラリ  

## 詳細

### テンプレートのインストール
```
dotnet new install Microsoft.FluentUI.AspNetCore.Templates
```

### プロジェクトの作成
```
dotnet new gitignore
dotnet new sln -n TodoApp
dotnet new fluentblazor --interactivity Server --all-interactive true --no-https -n TodoApp
dotnet sln add TodoApp
dotnet new nunit -n TodoApp.Tests
dotnet add TodoApp.Tests package FluentAssertions
dotnet sln add TodoApp.Tests
dotnet add TodoApp.Tests reference TodoApp
```

Fluent Assertions  
https://fluentassertions.com/  
https://github.com/fluentassertions/fluentassertions  
Fluent Assertions は、TDD または BDD スタイルの単体テストの予想される結果をより自然に指定できるようにする .NET 拡張メソッドのセットです。  
※ややこしいですが Fluent UI とは全く関係ありません。