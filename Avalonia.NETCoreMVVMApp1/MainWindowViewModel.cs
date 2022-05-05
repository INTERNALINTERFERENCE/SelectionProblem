using System.Collections.ObjectModel;
using Avalonia.NETCoreMVVMApp1.Pages;

namespace Avalonia.NETCoreMVVMApp1
{
    public class MainWindowViewModel: BaseViewModel
    {
        public MainWindowViewModel(
            SubscriptionsTreePageViewModel subscriptionsTreePageViewModel )
        {
            Pages.Add( subscriptionsTreePageViewModel );
        }

        public ObservableCollection<BaseViewModel> Pages { get; } = new();

    }
}