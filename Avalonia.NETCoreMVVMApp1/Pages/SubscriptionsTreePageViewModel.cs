using System;
using System.Text;
using System.Threading.Tasks;
using Avalonia.NETCoreMVVMApp1.Services;
using Avalonia.Threading;
using MQTTnet;
using MQTTnet.Client.Receiving;
using ReactiveUI;

namespace Avalonia.NETCoreMVVMApp1.Pages
{
    public class SubscriptionsTreePageViewModel : BaseViewModel, IMqttApplicationMessageReceivedHandler
    {
        public SubscriptionsTreePageViewModel( MqttClientService mqttClientService )
        {
            _mqttClientService = mqttClientService ?? throw new ArgumentNullException( nameof( mqttClientService ) );
            
            mqttClientService.RegisterApplicationMessageReceivedHandler( this );

            var root = new Node( "Host", 0 )
            {
                IsExpanded = false,
            };

            root.ForceResync();

            TreeRoot = root;
            
            Payload = string.Empty;

            Task.Run(Connect);
        }
        
        public Task HandleApplicationMessageReceivedAsync( MqttApplicationMessageReceivedEventArgs eventArgs )
        {
            return Dispatcher.UIThread.InvokeAsync(() =>
            {
                TreeRoot.Update();
                TreeRoot.Add( eventArgs.ApplicationMessage.Topic.Split( '/' ),
                    1, eventArgs.ApplicationMessage );
            } );
        }

        public async Task Connect()
        {
            try
            {
                 await _mqttClientService.Connect();
            }
            catch (Exception e)
            {
                Console.WriteLine( e.Message );
            }
        }

        public void ChangeExpandedValue() => 
            _selectedItem!.IsExpanded = !_selectedItem.IsExpanded;
        
        
        private readonly MqttClientService _mqttClientService;
        
        private Node? _selectedItem;
        private Node? _selectedItemUpdate;
        private DateTime _dateUpdated;
        private Node TreeRoot { get; }
        
        public Node? SelectedItem
        {
            get => _selectedItemUpdate;
            set
            {
                _selectedItemUpdate = value;
                if ( value != null && ( value != _selectedItem 
                                        || _dateUpdated != _selectedItem.Details.Timestamp ) )
                {
                    _selectedItem = value;
                    SetAndRaise( ref _selectedItemUpdate, null );
                    SetAndRaise( ref _selectedItemUpdate, value );
                    _dateUpdated = value.Details.Timestamp;
                }
            }
        }
        public string? Title
        {
            get => GetValue<string>();
            set => SetValue( value );
        }
       
        public string? Topic
        {
            get => GetValue<string>();
            set => SetValue( value );
        }

        public string? Payload
        {
            get => GetValue<string>();
            set => SetValue( value );
        }

        public byte[] GeneratePayload()
        {
            return Encoding.UTF8.GetBytes( Payload! );
        }
    }
}