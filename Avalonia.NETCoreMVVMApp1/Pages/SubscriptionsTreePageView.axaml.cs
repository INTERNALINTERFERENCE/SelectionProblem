using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Avalonia.NETCoreMVVMApp1.Pages
{
    public class SubscriptionsTreePageView : UserControl
    {
        public SubscriptionsTreePageView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}