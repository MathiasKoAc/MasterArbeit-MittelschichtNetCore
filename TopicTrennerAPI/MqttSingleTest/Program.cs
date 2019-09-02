using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace MqttSingleTest
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

        private static int anzMessages = 500;
        private static int milliSecondsSleep = 5;
        
        private static int counter = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("MQTT Tick Run");

            client = new MqttClient(brokerIp);

            // register to message received 
            //client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

            string clientId = Guid.NewGuid().ToString();
            byte conacct = client.Connect(clientId, brokerUser, brokerPwd);

            //string[] subtopics = new string[] {"single/release"};
            //client.Subscribe(subtopics, new byte[] { 0});

            List<string> topics = new List<string>();
            topics.Add("single/test");
            fire(anzMessages, topics);
        }

        private static void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            long ticks = DateTime.Now.Ticks;
            Console.WriteLine(new StringBuilder("Ticks:").Append(ticks).Append(" Received: ").Append(Encoding.UTF8.GetString(e.Message)).Append(" Topic: ").Append(e.Topic));
            if (long.TryParse(Encoding.UTF8.GetString(e.Message), out long oldTick))
            {
                Console.WriteLine(new StringBuilder(e.Topic).Append("Ticks: ").Append(ticks).Append(" Diff Ticks:").Append(ticks-oldTick));
                Console.WriteLine(new StringBuilder(e.Topic).Append("Ticks:: ").Append(ticks).Append(" Diff  Milli Sec:").Append((ticks - oldTick)/10000));
            }
            counter++;
        }

        private static void fire(int loopCount, List<string> topics)
        {
            for (int i = 0; i < loopCount; i++)
            {
                foreach (string top in topics)
                {
                    Task.Run(() => client.Publish(top, System.Text.Encoding.UTF8.GetBytes(""+ DateTime.Now.Ticks)));
                    Thread.Sleep(milliSecondsSleep);
                }
                //Console.WriteLine("Send " + i + " * " + topics.Count);
            }
        }
    }
}
