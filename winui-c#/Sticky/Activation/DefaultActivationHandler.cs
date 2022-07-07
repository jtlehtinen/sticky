using System;
using System.Threading.Tasks;

using Microsoft.UI.Xaml;

using Sticky.Contracts.Services;
using Sticky.ViewModels;

namespace Sticky.Activation;

public class DefaultActivationHandler : ActivationHandler<LaunchActivatedEventArgs> {
  private readonly INavigationService _navigationService;

  public DefaultActivationHandler(INavigationService navigationService) {
    _navigationService = navigationService;
  }

  protected override bool CanHandleInternal(LaunchActivatedEventArgs args) {
    // None of the ActivationHandlers has handled the activation.

    if (_navigationService.Frame == null) return false; // @TODO: Remove this line...

    return _navigationService.Frame.Content == null;
  }

  protected async override Task HandleInternalAsync(LaunchActivatedEventArgs args) {
    _navigationService.NavigateTo(typeof(MainViewModel).FullName, args.Arguments);

    await Task.CompletedTask;
  }
}
