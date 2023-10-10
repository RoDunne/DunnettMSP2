using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace DunnettMSP2.Models
{
    public class RomanticDate : PlannedEvent
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public override string Title
        {
            get => $"Date with {Name}";
            set { }
        }

        public override string ToStringEvent()
        {
            string theString = $"Date with {Name} on {StartDateAndTime:d} at {StartDateAndTime:t} at {Location}";
            return theString;
        }
    }
}
