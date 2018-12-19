using IBM.WMQ;
using System;
using System.Collections;

namespace Builder.MQ
{
    public class MQHandler
    {
        private MQQueueManager _queueManager;
        private string _queue;

        public MQHandler()
        {


        }

        public bool Connect(string queueManagerName, string queueName, string channelName, string hostname, int port)
        {
            MQEnvironment.Channel = channelName;
            MQEnvironment.Hostname = hostname;
            MQEnvironment.Port = port;
            MQEnvironment.UserId = "emulator";
            MQEnvironment.Password = "Abcd1234";
            _queue = queueName;

            Hashtable props = new Hashtable();
            props.Add(MQC.HOST_NAME_PROPERTY, MQEnvironment.Hostname);
            props.Add(MQC.PORT_PROPERTY, MQEnvironment.Port);
            props.Add(MQC.CHANNEL_PROPERTY, MQEnvironment.Channel);
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

            return true;
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
