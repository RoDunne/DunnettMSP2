using SQLite;
using System;

namespace DunnettMSP2.Models
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int TermId { get; set; }         //Foreign key from the Term table
        public int InstructorId { get; set; }   //Foreign key from the Instructors table
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string OptionalNotes { get; set; }
        public bool Notifications { get; set; }
    }
}
