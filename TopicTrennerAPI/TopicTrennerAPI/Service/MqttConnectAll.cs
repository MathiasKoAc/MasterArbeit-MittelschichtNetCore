using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TopicTrennerAPI.Interfaces;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace TopicTrennerAPI.Service
{
    public class MqttConnectAll : IMqttConnector
    {
        private volatile bool isActive = true;

        private IServerConfig _serverConfig;
        private MqttClient client;
        private List<IMqttTopicReceiver> ListTopicReceiver;

        private BlockingCollection<MqttMsgPublishEventArgs> receiverQueue;
        private BlockingCollection<MqttMsgPublishEventArgs> senderQueue;

        public MqttConnectAll(IServerConfig serverConfig)
        {
            Start(serverConfig);
        }

        public void Start(IServerConfig serverConfig)
        {
            _serverConfig = serverConfig;
            this.Start();
        }

        public void Start()
        {
            ListTopicReceiver = new List<IMqttTopicReceiver>();

            InitAndStartInnerCoroutines();

            try
            {
                isActive = true;
                // create client instance 
                client = new MqttClient(_serverConfig.GetServerIp());

                // register to message received 
                client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

                string clientId = Guid.NewGuid().ToString();
                byte conacct = client.Connect(clientId, _serverConfig.GetUsername(), _serverConfig.GetPassword());

                if (conacct != MqttMsgConnack.CONN_ACCEPTED)
                {
                    this.StopInnerCoroutines();
                    Console.WriteLine("ERROR: MqttConnector can't Connect to the Broker. Check _serverConfig IP, Username, Password.");
                    throw new Exception("ERROR: MqttConnector can't Connect to the Broker. Check _serverConfig IP, Username, Password.");
                }
            }
            catch (Exception)
            {
                this.StopInnerCoroutines();
                string errorstr = new StringBuilder("ERROR: MqttConnector can't Connect to the Broker on IP: ").Append(_serverConfig.GetServerIp()).ToString();
                Console.WriteLine(errorstr);
                throw new Exception(errorstr);
            }

        }

        /**
         * The Methode client_MqttMsgPublishReceived is called, if a MqttMessage arrieved and put it into the receiverQueue
         * client_MqttMsgPublishReceived seperats the thread which puts the message in the queue from working the message
         */
        void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            Console.WriteLine(new StringBuilder("Received: ").Append(System.Text.Encoding.UTF8.GetString(e.Message)).Append(" Topic: ").Append(e.Topic));
            receiverQueue.Add(e);//Enqueue(e);
        }

        /**
         * Inits and starts all InnerCoroutines
         */
        private void InitAndStartInnerCoroutines()
        {
            //start of the concurrency (Nebenläufig) for receiving Messages
            receiverQueue = new BlockingCollection<MqttMsgPublishEventArgs>();
            //start of the concurrency (Nebenläufig) for sending Messages
            senderQueue = new BlockingCollection<MqttMsgPublishEventArgs>();
            StartInnerCoroutines();
        }

        /**
         * Starts all InnerCoroutines
         */
        private void StartInnerCoroutines()
        {
            //start of the concurrency (Nebenläufig) for receiving Messages
            Task.Run(WorkReceiverQueueAndCall);

            //start of the concurrency (Nebenläufig) for sending Messages
            Task.Run(WorkSenderQueueAndSend);
        }

        /**
         * Stops all InnerCoroutines
         */
        private void StopInnerCoroutines()
        {
            isActive = false;
        }

        /**
         * workReveiverQueueAndCall is a Inner Coroutine, which loops over the ReceivingQueue to call the Subscriper in this App
         * WorkSenderQueueAndSend seperats the thread which puts the message in the queue from working the message
         */
        private void WorkReceiverQueueAndCall()
        {
            while (isActive)
            {
                MqttMsgPublishEventArgs e = receiverQueue.Take();

                if (ListTopicReceiver != null)
                {
                    foreach (IMqttTopicReceiver receiver in ListTopicReceiver)
                    {
                        receiver.OnReceivedMessage(e.Topic, e.Message);
                    }
                }
            }
        }

        /**
         * WorkSenderQueueAndSend is a Inner Coroutine, which loops over the senderQueue to send a Message if there is one
         * WorkSenderQueueAndSend seperats the call to send from the sending its self
         */
        private void WorkSenderQueueAndSend()
        {
            while (isActive)
            {
                MqttMsgPublishEventArgs mqttMessage = senderQueue.Take();
                client.Publish(mqttMessage.Topic, mqttMessage.Message, mqttMessage.QosLevel, mqttMessage.Retain);
                Console.WriteLine(new StringBuilder("SEND# Topic: ").Append(mqttMessage.Topic));
            }
        }

        /**
         * AddTopicReceiver added a receiver of an topic to the system. The receiver will autmaticaly called, if the topic gets a new message
         */
        public void AddTopicReceiver(string topic, IMqttTopicReceiver receiver, EnumMqttQualityOfService MqttQOS_Level = EnumMqttQualityOfService.QOS_LEVEL_EXACTLY_ONCE)
        {
            AddTopicReceiver(topic, receiver, (byte)MqttQOS_Level);
        }

        /**
         * AddTopicReceiver added a receiver of an topic to the system. The receiver will autmaticaly called, if the topic gets a new message
         */
        public void AddTopicReceiver(string topic, IMqttTopicReceiver receiver, byte MqttQOS_Level)
        {
            this.ListTopicReceiver.Add(receiver);
            client.Subscribe(new string[] { topic }, new byte[] { MqttQOS_Level });
        }

        /**
         * PublishMessage enqueues the Message of a Topic in the Queue and the message will automaticaly, concurrently sended
         */
        public void PublishMessage(string topic, string Message, EnumMqttQualityOfService MqttQOS_Level = EnumMqttQualityOfService.QOS_LEVEL_EXACTLY_ONCE, bool retain = false)
        {
            this.PublishMessage(topic, Message, (byte)MqttQOS_Level, retain);
        }

        public void PublishMessage(string topic, string Message, byte MqttQOS_Level, bool retain = false)
        {
            this.PublishMessage(topic, System.Text.Encoding.UTF8.GetBytes(Message), MqttQOS_Level, retain);
        }

        public void PublishMessage(string topic, byte[] Message, byte MqttQOS_Level, bool retain = false)
        {
            Console.WriteLine("sending init");
            this.senderQueue.Add(new MqttMsgPublishEventArgs(topic, Message, false, MqttQOS_Level, retain));
        }

        public string Hello()
        {
            throw new NotImplementedException();
        }
    }
}
