using System;
using System.Collections.Generic;
using System.Threading;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MqttTester
{
    class Program
    {
        private static MqttClient client;
        private static string brokerIp = "192.168.0.2";
        private static string brokerUser = "mqtt";
        private static string brokerPwd = "HomeSmart";

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            client = new MqttClient(brokerIp);

            // register to message received 
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

            string clientId = Guid.NewGuid().ToString();
            byte conacct = client.Connect(clientId, brokerUser, brokerPwd);

            List<string> topics = new List<string>();
            topics.Add("a/a");
            topics.Add("c/c");
            topics.Add("e/e");
            topics.Add("g/g");
            topics.Add("i/i");
            topics.Add("k/k");
            topics.Add("m/m");
            topics.Add("o/o");
            topics.Add("p/p");
            topics.Add("s/p");

            fire(100, topics);
        }

        private static void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {

        }

        private static void fire(int loopCount, List<string> topics)
        {
            for(int i = 0; i < loopCount; i++)
            {
                foreach(string top in topics)
                {
                    client.Publish(top, System.Text.Encoding.UTF8.GetBytes("d"));
                    Thread.Sleep(2000);
                }
                Console.WriteLine("Send " + i + " * " + topics.Count);
            }
        }
    }
}
