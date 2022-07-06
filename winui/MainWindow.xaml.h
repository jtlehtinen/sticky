#pragma once

#include "MainWindow.g.h"

namespace winrt::Sticky::implementation {
  struct MainWindow : MainWindowT<MainWindow> {
    MainWindow();

    int32_t MyProperty();
    void MyProperty(int32_t value);

    void myButton_Click(Windows::Foundation::IInspectable const& sender, Microsoft::UI::Xaml::RoutedEventArgs const& args);
  };
}  // namespace winrt::Sticky::implementation

namespace winrt::Sticky::factory_implementation {
  struct MainWindow : MainWindowT<MainWindow, implementation::MainWindow> {
  };
}  // namespace winrt::Sticky::factory_implementation
