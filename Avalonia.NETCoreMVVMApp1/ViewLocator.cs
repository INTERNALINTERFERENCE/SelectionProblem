using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using SimpleInjector;

namespace Avalonia.NETCoreMVVMApp1
{
    public class ViewLocator : IDataTemplate
    {
        readonly Container _container;
        public ViewLocator( Container container )
        {
            _container = container ?? throw new ArgumentNullException( nameof( container ) );
        }
        public IControl Build(object data)
        {
            var name = data.GetType().FullName!.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type)!;
            }
            else
            {
                return new TextBlock {Text = "Not Found: " + name};
            }
        }

        public bool Match(object data)
        {
            return data is BaseViewModel;
        }
    }
}