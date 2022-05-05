using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Avalonia.NETCoreMVVMApp1.Pages
{
    public sealed class BufferInspectorViewModel : BaseViewModel
    {
        byte[]? _buffer;
        string? _utf8Content;
        string? _header;
        string? _payload;

        public byte[]? Buffer
        {
            get => _buffer;
            private set
            {
                _buffer = value;
                OnPropertyChanged();
            }
        }

        public void Dump( byte[]? buffer )
        {
            Buffer = buffer;

            Utf8Content = Encoding.UTF8.GetString( _buffer! );

            try
            {
                var json = JObject.Parse( Encoding.UTF8.GetString( _buffer! ) );
                Payload = JsonConvert.SerializeObject( json[ nameof( Payload ) ], Formatting.Indented );
                Header = JsonConvert.SerializeObject( json[ nameof( Header ) ], Formatting.Indented );
            }
            catch ( Exception )
            {
                Payload = Utf8Content;
            }
        }

        public string? Utf8Content
        {
            get => _utf8Content;
            set
            {
                _utf8Content = value;
                OnPropertyChanged();
            }
        }

        public string? Payload
        {
            get => _payload;
            set
            {
                _payload = value;
                OnPropertyChanged();
            }
        }

        public string? Header
        {
            get => _header;
            set
            {
                _header = value;
                OnPropertyChanged();
            }
        }
    }
}