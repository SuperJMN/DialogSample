using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using AvaloniaApplication3.ViewModels;
using AvaloniaApplication3.Views;
using Zafiro.Avalonia.Dialogs;
using Zafiro.Avalonia.Mixins;

namespace AvaloniaApplication3;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        this.Connect(() => new MainView(), control => new MainViewModel(DialogService.Create()), () => new MainWindow());

        base.OnFrameworkInitializationCompleted();
    }
}
