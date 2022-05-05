using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Avalonia.Collections;
using Avalonia.NETCoreMVVMApp1.Pages;
using Avalonia.Threading;
using MQTTnet;

namespace Avalonia.NETCoreMVVMApp1.Services
{
    public class Node : BaseViewModel
    {
        public Node( string part, int level, Node? root = null, Node? parent = null )
        {
            Part = part;
            Level = level;
            Parent = parent;
            Root = root ?? this;
            MessagesCount = 1;
            TopicsCount = 0;
            _details = new Lazy<ReceivedApplicationMessageDetailsViewModel>( () =>
                new ReceivedApplicationMessageDetailsViewModel( _message ) );
        }

        public bool Add( string[] topic, int level, MqttApplicationMessage message )
        {
            var changedTopicCount = false;
            if ( topic.IsNullOrEmpty())
            {
                _message = message;
                _details = new Lazy<ReceivedApplicationMessageDetailsViewModel>( () =>
                    new ReceivedApplicationMessageDetailsViewModel( _message ) );
                TopicsCount = 1;
                return false;
            }

            MessagesCount++;
            
            if ( !_children.ContainsKey( topic[ 0 ] ) )
            {
                var node = new Node( topic[0], level,  Root, this );
                _children.TryAdd( topic[ 0 ], node );
                Root.EnqueueUpdate();
                changedTopicCount = true;
            }

            if ( !_children[ topic[ 0 ] ].Add( topic.Skip( 1 ).ToArray(), level + 1, message )
                 && !changedTopicCount ) return changedTopicCount;
            changedTopicCount = true;
            TopicsCount++;
            return changedTopicCount;
        }
        
        public void EnqueueUpdate()
        {
            if ( _updateEnqueued ) return;
            _updateEnqueued = true;
            Dispatcher.UIThread.Post( Update, DispatcherPriority.Background );
        }

        static private void AppendItems( ICollection<Node> list, Node node )
        {
            list.Add( node );
            if ( !node.IsExpanded ) return;
            foreach ( var ch in node._children )
                AppendItems( list, ch.Value );
        }

        private void AppendItems()
        {
            _updateEnqueued = false;
            var list = new AvaloniaList<Node>();
            AppendItems( list, this );
            _visibleChildren = new AvaloniaList<Node>( list );
        }

        public void Update()
        {
            AppendItems();
            Root.RaisePropertyChanged( nameof( Root.VisibleChildren ) );
        }
        
        public void ForceResync() => Root.Update();
        
        private bool _isExpanded;
        public Node? Parent { get; }
        private Node Root { get; }

        private MqttApplicationMessage? _message;
        public string Part { get; }
        public int Level { get; }

        private bool _updateEnqueued;

        private readonly Dictionary<string, Node> _children = new();
        
        private AvaloniaList<Node> _visibleChildren = new();
        
        private Lazy<ReceivedApplicationMessageDetailsViewModel> _details;
        
        private int TopicsCount
        {
            get => GetValue<int>();
            set => SetValue( value );
        }

        private int MessagesCount
        {
            get => GetValue<int>();
            set => SetValue( value );
        }
        
        public bool IsExpanded
        {
            get => _isExpanded; 
            set
            {
                SetAndRaise( ref _isExpanded, value );
                Root.EnqueueUpdate();
            }
        }
        
        public IAvaloniaReadOnlyList<Node> VisibleChildren => _visibleChildren;
        public ReceivedApplicationMessageDetailsViewModel Details => _details.Value;
    }

    static public partial class Xtensions
    {
        static public bool IsNullOrEmpty<T>( this IEnumerable<T> @this ) =>
            @this.IsNull() || @this.IsEmpty();
        
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public bool IsNull<T>( this T @this ) =>
            @this == null;
        
        static public bool IsEmpty<T>( this IEnumerable<T> @this ) =>
            @this.GetEnumerator().MoveNext().IsFalse();
        
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
        static public bool IsFalse( this bool @this ) =>
            @this == false;
    }
}