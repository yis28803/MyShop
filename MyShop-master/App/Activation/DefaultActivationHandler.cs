using App.Contracts.Services;
using App.ViewModels;
using System.Configuration;
using Microsoft.UI.Xaml;

namespace App.Activation;

public class DefaultActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    private readonly INavigationService _navigationService;

    public DefaultActivationHandler(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
    {
        // None of the ActivationHandlers has handled the activation.
        return _navigationService.Frame?.Content == null;
    }

    protected async override Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        var lastPage = ConfigurationManager.AppSettings["lastPage"];
        if (lastPage != null && lastPage!="")
        {
            _navigationService.NavigateTo(lastPage, args.Arguments);
        }
        else
        {
            _navigationService.NavigateTo(typeof(DashboardViewModel).FullName!, args.Arguments);
        }

        await Task.CompletedTask;
    }
}
