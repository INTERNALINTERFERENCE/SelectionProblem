using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Publishing;
using MQTTnet.Client.Receiving;
using MQTTnet.Client.Subscribing;
using MQTTnet.Client.Unsubscribing;
using MQTTnet.Diagnostics.Logger;
using MQTTnet.Diagnostics.PacketInspection;
using MQTTnet.Exceptions;
using MQTTnet.Formatter;

namespace Avalonia.NETCoreMVVMApp1.Services
{
    public class MqttClientService : IMqttApplicationMessageReceivedHandler, IMqttPacketInspector
    {
        readonly List<IMqttApplicationMessageReceivedHandler> _applicationMessageReceivedHandlers = new ();
        private IMqttClient? _mqttClient;
        
        public async Task Connect()
        {
            if ( _mqttClient != null )
            {
                await _mqttClient.DisconnectAsync();
                _mqttClient.Dispose();
            }

            _mqttClient = new MqttFactory().CreateMqttClient();

            _mqttClient.UseApplicationMessageReceivedHandler( this );
            
            var clientOptionsBuilder = new MqttClientOptionsBuilder()
                .WithCommunicationTimeout( TimeSpan.FromSeconds(10) )
                .WithProtocolVersion( MqttProtocolVersion.V311 )
                .WithClientId( "LogViewer-b9481e80-a9a2-4354-ab60-38d030fcbcad" )
                .WithTcpServer("test.mosquitto.org", 1883)
                .WithKeepAlivePeriod( TimeSpan.FromSeconds( int.MaxValue ) );
            
            await _mqttClient.ConnectAsync( clientOptionsBuilder.Build() );
            await Subscribe();
        }
        
        public async Task Subscribe( )
        {
            var topicFilter = new MqttTopicFilterBuilder()
                .WithTopic( "/#" )
                .Build();

            var subscribeOptions = new MqttClientSubscribeOptionsBuilder()
                .WithTopicFilter( topicFilter )
                .Build();

            await _mqttClient.SubscribeAsync( subscribeOptions );
        }
        
        public void RegisterApplicationMessageReceivedHandler( IMqttApplicationMessageReceivedHandler handler )
        {
            if ( handler == null ) throw new ArgumentNullException( nameof( handler ) );

            _applicationMessageReceivedHandlers.Add( handler );
        }
        
        public async Task HandleApplicationMessageReceivedAsync( MqttApplicationMessageReceivedEventArgs eventArgs )
        {
            if ( eventArgs == null ) throw new ArgumentNullException( nameof( eventArgs ) );

            foreach ( var handler in _applicationMessageReceivedHandlers )
            {
                await handler.HandleApplicationMessageReceivedAsync( eventArgs );
            }
        }


        public void ProcessMqttPacket(ProcessMqttPacketContext context)
        {
            throw new NotImplementedException();
        }
    }
   
}