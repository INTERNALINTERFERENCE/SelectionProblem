using System;
using MQTTnet;

namespace Avalonia.NETCoreMVVMApp1.Pages
{
    public class ReceivedApplicationMessageDetailsViewModel : BaseViewModel
    {
        public ReceivedApplicationMessageDetailsViewModel( MqttApplicationMessage? message )
        {
            Timestamp = DateTime.Now;
            if ( message != null )
            {
                Topic = message.Topic;
                PayloadLength = message.Payload?.Length ?? 0;
                QualityOfServiceLevel = $"{(int)message.QualityOfServiceLevel} ({message.QualityOfServiceLevel})";

                PayloadInspector = new BufferInspectorViewModel();
                PayloadInspector.Dump( message.Payload ?? Array.Empty<byte>() );
            }

            Header = PayloadInspector?.Header;

            if ( Header == null ) return;
        }
        
        public string? Header { get; }
        public DateTime Timestamp { get; }
        
        public string Topic
        {
            get;
        }

        public int PayloadLength
        {
            get;
        }

        public string? QualityOfServiceLevel
        {
            get;
        }

        public BufferInspectorViewModel? PayloadInspector { get; }
    }
}