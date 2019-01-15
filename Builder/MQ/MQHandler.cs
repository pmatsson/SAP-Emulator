using IBM.WMQ;
using System;
using System.Collections;
using System.Threading.Tasks;

namespace Builder.MQ
{
    public class MQHandler
    {
        private MQQueueManager _queueManager;
        private string _queue;

        public MQHandler()
        {
            MQEnvironment.UserId = "emulator";
            MQEnvironment.Password = "Abcd1234";
        }

        public bool IsConnected()
        {
            return _queueManager != null && _queueManager.IsConnected;
        }

        public bool Connect(MQProps props)
        {
            _queue = props.QueueName;

            Hashtable propshash = new Hashtable();
            propshash.Add(MQC.HOST_NAME_PROPERTY, props.Hostname);
            propshash.Add(MQC.PORT_PROPERTY, props.Port);
            propshash.Add(MQC.CHANNEL_PROPERTY, props.ChannelName);
            propshash.Add(MQC.USER_ID_PROPERTY, MQEnvironment.UserId);
            propshash.Add(MQC.PASSWORD_PROPERTY, MQEnvironment.Password);

            try
            {
                _queueManager = new MQQueueManager(props.QueueManagerName, propshash);
            }
            catch (MQException exp)
            {
                return false;
            }

            return _queueManager.IsConnected;
        }

        public bool Connect(string queueManagerName, string queueName, string channelName, string hostname, int port)
        {
            _queue = queueName;
            Hashtable props = new Hashtable();
            props.Add(MQC.HOST_NAME_PROPERTY, hostname);
            props.Add(MQC.PORT_PROPERTY, port);
            props.Add(MQC.CHANNEL_PROPERTY, channelName);
            props.Add(MQC.USER_ID_PROPERTY, MQEnvironment.UserId);
            props.Add(MQC.PASSWORD_PROPERTY, MQEnvironment.Password);


            try
            {
                _queueManager = new MQQueueManager(queueManagerName, props);

            }
            catch (MQException exp)
            {
                return false;
            }
            return _queueManager.IsConnected;
        }

        public void Disconnect()
        {
            if (_queueManager != null &&_queueManager.IsConnected)
            {
                _queueManager.Disconnect();
            }
        }

        public bool Write(string message)
        {
            try
            {
                var queue = _queueManager.AccessQueue(_queue, MQC.MQOO_OUTPUT + MQC.MQOO_FAIL_IF_QUIESCING);

                var queueMessage = new MQMessage();
                queueMessage.WriteString(message);
                queueMessage.Format = MQC.MQFMT_STRING;

                queue.Put(queueMessage, new MQPutMessageOptions());


            }

            catch (MQException MQexp)
            {
                return false;
            }

            catch (Exception exp)

            {
                return false;
            }

            return true;
        }



        public bool Read(ref string result)
        {

            result = "";

            try
            {
                var queue = _queueManager.AccessQueue(_queue, MQC.MQOO_INPUT_AS_Q_DEF + MQC.MQOO_FAIL_IF_QUIESCING);
                var queueMessage = new MQMessage();
                queueMessage.Format = MQC.MQFMT_STRING;

                queue.Get(queueMessage, new MQGetMessageOptions());

                result = queueMessage.ReadString(queueMessage.MessageLength);
            }

            catch (MQException MQexp)
            {

                result = "Exception : " + MQexp.Message;
                return false;

            }

            catch (Exception exp)
            {

                result = "Exception: " + exp.Message;
                return false;

            }

            return true;
        }
    }
}
