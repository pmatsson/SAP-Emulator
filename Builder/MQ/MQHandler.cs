using IBM.WMQ;
using NLog;
using System;
using System.Collections;

namespace MQChatter.MQ
{
    public class MQHandler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
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

            Hashtable propshash = new Hashtable
            {
                { MQC.HOST_NAME_PROPERTY, props.Hostname },
                { MQC.PORT_PROPERTY, props.Port },
                { MQC.CHANNEL_PROPERTY, props.ChannelName },
                { MQC.USER_ID_PROPERTY, MQEnvironment.UserId },
                { MQC.PASSWORD_PROPERTY, MQEnvironment.Password }
            };

            try
            {
                _queueManager = new MQQueueManager(props.QueueManagerName, propshash);
            }
            catch (MQException)
            {
                return false;
            }

            return IsConnected();
        }

        public bool Connect(string queueManagerName, string queueName, string channelName, string hostname, int port)
        {
            _queue = queueName;
            Hashtable props = new Hashtable
            {
                { MQC.HOST_NAME_PROPERTY, hostname },
                { MQC.PORT_PROPERTY, port },
                { MQC.CHANNEL_PROPERTY, channelName },
                { MQC.USER_ID_PROPERTY, MQEnvironment.UserId },
                { MQC.PASSWORD_PROPERTY, MQEnvironment.Password }
            };

            try
            {
                _queueManager = new MQQueueManager(queueManagerName, props);
            }
            catch (MQException)
            {
                //return false;
            }
            return _queueManager.IsConnected;
        }

        public void Disconnect()
        {
            if (_queueManager != null && _queueManager.IsConnected)
            {
                _queueManager.Disconnect();
            }
        }

        public bool Write(string message)
        {
            try
            {
                MQQueue queue = _queueManager.AccessQueue(_queue, MQC.MQOO_OUTPUT + MQC.MQOO_FAIL_IF_QUIESCING);
                MQMessage queueMessage = new MQMessage();
                queueMessage.WriteString(message);
                queueMessage.Format = MQC.MQFMT_STRING;
                queue.Put(queueMessage, new MQPutMessageOptions());
            }
            catch (Exception ex)
            {
                logger.Warn(ex, "Unable to send message to queue");
                return false;
            }
            return true;
        }

        public bool Read(ref string result)
        {
            result = "";
            try
            {
                MQQueue queue = _queueManager.AccessQueue(_queue, MQC.MQOO_INPUT_AS_Q_DEF +
                                                              MQC.MQOO_FAIL_IF_QUIESCING +
                                                              MQC.MQOO_INQUIRE);

                if (queue.CurrentDepth > 0)
                {
                    MQMessage queueMessage = new MQMessage
                    {
                        Format = MQC.MQFMT_STRING
                    };
                    queue.Get(queueMessage, new MQGetMessageOptions());
                    result = queueMessage.ReadString(queueMessage.MessageLength);
                    return true;
                }
            }
            catch (MQException mqex)
            {
                result = "MQ exception : " + mqex.Message;
            }
            catch (Exception ex)
            {
                result = "Exception: " + ex.Message;
            }

            return false;
        }

        public bool Peek(ref string result)
        {
            result = "";
            try
            {
                MQQueue queue = _queueManager.AccessQueue(_queue, MQC.MQOO_BROWSE | MQC.MQOO_FAIL_IF_QUIESCING);
                MQMessage queueMessage = new MQMessage
                {
                    Format = MQC.MQFMT_STRING
                };
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