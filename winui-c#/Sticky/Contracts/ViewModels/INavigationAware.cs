namespace Sticky.Contracts.ViewModels;

public interface INavigationAware {
  void OnNavigatedTo(object parameter);

  void OnNavigatedFrom();
}
