using Builder.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Builder.MQ
{
    public class MQProps : NotifyPropertyChangedBase, IEquatable<MQProps>
    {
        string _queueManagerName;
        string _queueName;
        string _channelName;
        string _hostName;
        int? _port;

        public string QueueManagerName
        {
            get => _queueManagerName;
            set => SetProperty(ref _queueManagerName, value);
        }

        public string QueueName
        {
            get => _queueName;
            set => SetProperty(ref _queueName, value);
        }

        public string ChannelName
        {
            get => _channelName;
            set => SetProperty(ref _channelName, value);
        }

        public string Hostname
        {
            get => _hostName;
            set => SetProperty(ref _hostName, value);
        }

        public int? Port
        {
            get => _port;
            set => SetProperty(ref _port, value);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as MQProps);
        }

        public bool Equals(MQProps other)
        {
            return other != null &&
                   QueueManagerName == other.QueueManagerName &&
                   QueueName == other.QueueName &&
                   ChannelName == other.ChannelName &&
                   Hostname == other.Hostname &&
                   EqualityComparer<int?>.Default.Equals(Port, other.Port);
        }

        public override int GetHashCode()
        {
            var hashCode = -1748027768;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(QueueManagerName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(QueueName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ChannelName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Hostname);
            hashCode = hashCode * -1521134295 + EqualityComparer<int?>.Default.GetHashCode(Port);
            return hashCode;
        }
    }
}
