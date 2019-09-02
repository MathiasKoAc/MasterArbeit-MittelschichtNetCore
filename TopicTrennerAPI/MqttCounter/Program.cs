using System;
using System.Text;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MqttCounter
{
    class Program
    {
        private static MqttClient client;
        //private static string brokerIp = "192.168.0.2";
        private static string brokerIp = "192.168.0.148";
        //private static string brokerUser = "mqtt";
        private static string brokerUser = "";
        //private static string brokerPwd = "HomeSmart";
        private static string brokerPwd = "";

        private static int counter = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("MQTT Tick Counter");

            client = new MqttClient(brokerIp);

            // register to message received 
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

            string clientId = Guid.NewGuid().ToString();
            byte conacct = client.Connect(clientId, brokerUser, brokerPwd);

            string[] subtopics = new string[] { "single/release" };
            client.Subscribe(subtopics, new byte[] { 0 });
        }

        private static void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            long ticks = DateTime.Now.Ticks;
            Console.WriteLine(new StringBuilder("Id:").Append(counter).Append(" Ticks:").Append(ticks).Append(" Received:").Append(Encoding.UTF8.GetString(e.Message)).Append(" Topic:").Append(e.Topic));
            if (long.TryParse(Encoding.UTF8.GetString(e.Message), out long oldTick))
            {
                Console.WriteLine(new StringBuilder("A-Id:").Append(counter).Append(e.Topic).Append(" Diff-Ticks:").Append(ticks - oldTick));
                Console.WriteLine(new StringBuilder("B-Id:").Append(counter).Append(e.Topic).Append(" Diff-Milli-Sec:").Append((ticks - oldTick) / 10000));
            }
            counter++;
        }
    }
}
