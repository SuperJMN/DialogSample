using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;
using Zafiro.Avalonia.Dialogs;
using Zafiro.CSharpFunctionalExtensions;
using Zafiro.UI;

namespace AvaloniaApplication3.ViewModels;

public partial class MainViewModel : ReactiveObject
{
    private readonly IDialog dialogService;

    public MainViewModel(IDialog dialogService, INotificationService notificationService)
    {
        this.dialogService = dialogService;

        Sync = StoppableCommand.Create(() =>
        {
            return Observable
                .FromAsync(RequestSettings)
                .Values()
                .SelectMany(GetMetrics);
        }, Maybe<IObservable<bool>>.None);

        Sync.StartReactive.HandleErrorsWith(notificationService);
        CurrentMetrics = Sync.StartReactive.Successes();
    }

    private static IObservable<Result<Metrics>> GetMetrics(ConnectionSettings connectionSettings)
    {
        var service = new MetricsService(connectionSettings);

        return Observable
            .Timer(TimeSpan.Zero, TimeSpan.FromSeconds(5))
            .SelectMany(_ => Observable.FromAsync(() => service.GetMetrics()))
            .Repeat();
    }

    public IObservable<Metrics> CurrentMetrics { get; }

    public StoppableCommand<Unit, Result<Metrics>> Sync { get; }
    
    private Task<Maybe<ConnectionSettings>> RequestSettings()
    {
        return dialogService
            .ShowAndGetResult(
                viewModel: new ConnectionDialogViewModel(), 
                title: "Connection Settings", 
                canSubmit: vm => vm.IsValid(), 
                getResult: x => new ConnectionSettings(x.Host, x.Port!.Value));
    }
}

internal class MetricsService
{
    public ConnectionSettings Settings { get; }

    public MetricsService(ConnectionSettings settings)
    {
        Settings = settings;
    }

    public async Task<Result<Metrics>> GetMetrics()
    {
        return new Metrics(new Random().Next());
    }
}

public record Metrics(int Random);