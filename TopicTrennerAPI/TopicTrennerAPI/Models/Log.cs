using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopicTrennerAPI.Models
{
    public class Log
    {
        public int ID { get; set; }
        public SessionRun SessionRun { get; set; }
        public int SessionRunID { get; set; }
        public string Topic { get; set; }
        public string Message { get; set; }

        public void SetMessageUft8Byte(byte[] message)
        {
            Message = System.Text.Encoding.UTF8.GetString(message);
        }
    }
}
