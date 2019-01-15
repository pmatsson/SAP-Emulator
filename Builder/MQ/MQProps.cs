using Builder.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Builder.MQ
{
    public class MQProps : NotifyPropertyChangedBase
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
    }
}
