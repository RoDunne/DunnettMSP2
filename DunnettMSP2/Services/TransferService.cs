using DunnettMSP2.Models;

namespace DunnettMSP2.Services
{
    //This class is used to store data to be transferred between pages
    public static class TransferService
    {
        public static Term TermToTransfer { get; set; }
        public static Course CourseToTransfer { get; set; }
        public static Instructor InstructorToTransfer { get; set; }
        public static Assessment PerformanceAssessmentToTransfer { get; set; }
        public static Assessment ObjectiveAssessmentToTransfer { get; set; }
    }
}
