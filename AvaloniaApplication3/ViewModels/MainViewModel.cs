using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ReactiveUI;
using ReactiveUI.SourceGenerators;
using ReactiveUI.Validation.Extensions;
using Zafiro.Avalonia.Dialogs;
using Zafiro.CSharpFunctionalExtensions;
using Zafiro.Reactive;

namespace AvaloniaApplication3.ViewModels;

public partial class MainViewModel : ReactiveObject
{
    private readonly IDialog dialogService;

    [ObservableAsProperty]
    private ImConnectedViewModel? imConnectedViewModel;

    public MainViewModel(IDialog dialogService)
    {
        this.dialogService = dialogService;

        Connect = ReactiveCommand.CreateFromTask(ShowConnectionDialog, this.WhenAnyValue(x => x.ImConnectedViewModel).Null());
        imConnectedViewModelHelper = Connect.Values().ToProperty(this, x => x.ImConnectedViewModel);
        CanConnect = Connect.CanExecute;
    }

    public IObservable<bool> CanConnect { get; }

    public ReactiveCommand<Unit, Maybe<ImConnectedViewModel>> Connect { get; }

    private Task<Maybe<ImConnectedViewModel>> ShowConnectionDialog()
    {
        return dialogService
            .ShowAndGetResult(new ConnectionDialogViewModel(), "Provide settings first, bitch", vm => vm.IsValid(), x => new ConnectionSettings(x.Host, x.Port!.Value))
            .Map(settings => new ImConnectedViewModel(settings));
    }
}