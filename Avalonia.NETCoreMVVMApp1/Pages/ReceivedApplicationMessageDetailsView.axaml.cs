using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Avalonia.NETCoreMVVMApp1.Pages
{
    public class ReceivedApplicationMessageDetailsView : UserControl
    {
        public ReceivedApplicationMessageDetailsView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}