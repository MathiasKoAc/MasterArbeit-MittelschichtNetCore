using System;
using System.Collections.Generic;

namespace TopicTrennerAPI.Models
{
    public class SessionRun
    {
        public int ID { get; set; }
        public string Beschreibung { get; set; }
        public Session Session { get; set; }
        public int SessionID { get; set; }
        public List<Log> Logs { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime StopTime { get; set; }
        public bool Active { get; set; }
        public TimeDiff TimeDiff {get; set;}

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
