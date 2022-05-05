using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Avalonia.NETCoreMVVMApp1
{
    public class MainWindowView : Window
    {
        public static MainWindowView Instance { get; private set; } = default!;
        public MainWindowView()
        {
            Instance = this;
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}