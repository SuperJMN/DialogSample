using System;
using ReactiveUI.SourceGenerators;
using ReactiveUI.Validation.Extensions;
using ReactiveUI.Validation.Helpers;

namespace AvaloniaApplication3.ViewModels;

public partial class ConnectionDialogViewModel : ReactiveValidationObject
{
    [Reactive] private string host = "";
    [Reactive] private int? port = 1000;

    public ConnectionDialogViewModel()
    {
        this.ValidationRule(x => x.Host, x => Uri.CheckHostName(x) != UriHostNameType.Unknown, "Invalid host");
        this.ValidationRule(x => x.Port, x => x is > 0 and < ushort.MaxValue, "Invalid port");
    }
}