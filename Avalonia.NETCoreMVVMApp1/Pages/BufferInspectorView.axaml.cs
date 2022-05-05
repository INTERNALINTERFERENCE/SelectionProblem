using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Avalonia.NETCoreMVVMApp1.Pages
{
    public class BufferInspectorView : UserControl
    {
        public BufferInspectorView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}