using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DunnettMSP2.Models
{
    public class PlannedEvent
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public virtual string Title { get; set; }
        public DateTime StartDateAndTime { get; set; }
        public string Location { get; set; }

        public virtual string ToStringEvent()
        {
            string theString = $"{Title} on {StartDateAndTime:d} at {StartDateAndTime:t} at {Location}";
            return theString;
        }
    }
}
