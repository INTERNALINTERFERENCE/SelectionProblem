
using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.NETCoreMVVMApp1.Services;
using SimpleInjector;

namespace Avalonia.NETCoreMVVMApp1
{
    public class App : Application
    {
        readonly Container _container;
        static Window? _mainWindow;

        public App()
        {
            _container = new Container();
            _container.Options.ResolveUnregisteredConcreteTypes = true;
            _container.RegisterSingleton<MqttClientService>();

            var viewLocator = new ViewLocator( _container );
            DataTemplates.Add( viewLocator );
        }
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
       
        public override void OnFrameworkInitializationCompleted()
        {
            if ( ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop )
            {
                _mainWindow = new MainWindowView
                {
                    DataContext = _container.GetInstance<MainWindowViewModel>()
                };

                desktop.MainWindow = _mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}