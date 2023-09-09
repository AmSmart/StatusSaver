using StausSaver.Maui.Services;
using StausSaver.Maui.ViewModels;

namespace StausSaver.Maui.Pages;

public partial class AppShell : Shell
{
    public AppShell(ShellViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}