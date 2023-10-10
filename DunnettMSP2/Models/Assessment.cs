using SQLite;
using System;

namespace DunnettMSP2.Models
{
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int CourseId { get; set; }       //Foreign key from the Course table
        public string Type { get; set; }
        public string Title { get; set; }
        public DateTime DueDate { get; set; }
        public bool Notifications { get; set; }
    }
}
