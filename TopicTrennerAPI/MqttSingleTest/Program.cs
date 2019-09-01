using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MqttSingleTest
{
    class Program
    {
        private static MqttClient client;
        private static string brokerIp = "192.168.0.2";
        private static string brokerUser = "mqtt";
        private static string brokerPwd = "HomeSmart";

        static void Main(string[] args)
        {
            Console.WriteLine("MQTT Tick Run");

            client = new MqttClient(brokerIp);

            // register to message received 
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

            string clientId = Guid.NewGuid().ToString();
            byte conacct = client.Connect(clientId, brokerUser, brokerPwd);

            string[] subtopics = new string[] {"single/test", "single/release"};
            client.Subscribe(subtopics, new byte[] { 0, 0});

            List<string> topics = new List<string>();
            topics.Add("single/test");
            fire(20, topics);
        }

        private static void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            long ticks = DateTime.Now.Ticks;
            Console.WriteLine(new StringBuilder("Ticks:").Append(ticks).Append(" Received: ").Append(Encoding.UTF8.GetString(e.Message)).Append(" Topic: ").Append(e.Topic));
            if (long.TryParse(Encoding.UTF8.GetString(e.Message), out long oldTick))
            {
                Console.WriteLine(new StringBuilder(e.Topic).Append(" Diff Ticks:").Append(ticks-oldTick));
                Console.WriteLine(new StringBuilder(e.Topic).Append(" Diff  Milli Sec:").Append((ticks - oldTick)/10000));
            }

        }

        private static void fire(int loopCount, List<string> topics)
        {
            for (int i = 0; i < loopCount; i++)
            {
                foreach (string top in topics)
                {
                    client.Publish(top, System.Text.Encoding.UTF8.GetBytes(""+ DateTime.Now.Ticks));
                    Thread.Sleep(50);
                }
                Console.WriteLine("Send " + i + " * " + topics.Count);
            }
        }
    }
}
