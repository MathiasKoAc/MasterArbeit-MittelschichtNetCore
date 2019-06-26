using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TopicTrennerAPI.Models
{
    public class SessionRun
    {
        public int ID { get; set; }
        public Session Session { get; set; }
        public int SessionID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime StopTime { get; set; }
        public bool Active { get; set; }

        /*private bool _active;
        public bool Active
        {
            get
            {
                return _active;
            }
            set
            {
                if(value && value != _active)
                {
                    //activieren

                }
                else if (!value && value != _active)
                {
                    //deactivieren

                }
                _active = value;      
            }
        }
        */
    }
}
