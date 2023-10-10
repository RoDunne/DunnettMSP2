using System;
using System.Windows.Input;
using Xamarin.Forms;
using DunnettMSP2.Services;
using DunnettMSP2.Models;

namespace DunnettMSP2.ViewModels
{
    public class CourseAddViewModel : BaseViewModel
    {
        public Term TermToAddTo { get; set; }

        //**VIEW BOUND PROPERTIES**//
        public string CourseTitle
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
        public DateTime CourseStartDate
        {
            get => GetProperty<DateTime>();
            set => SetProperty(value);
        }
        public DateTime CourseEndDate
        {
            get => GetProperty<DateTime>();
            set => SetProperty(value);
        }
        public string CourseStatus
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
        public string CourseOptionalNotes
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
        public bool CourseNotifications
        {
            get => GetProperty<bool>();
            set => SetProperty(value);
        }

        public string InstructorName
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
        public string InstructorEmail
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
        public string InstructorPhone
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        public string PerformanceAssessmentTitle
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
        public DateTime PerformanceAssessmentDueDate
        {
            get => GetProperty<DateTime>();
            set => SetProperty(value);
        }
        public bool PerformanceAssessmentNotification
        {
            get => GetProperty<bool>();
            set => SetProperty(value);
        }

        public string ObjectiveAssessmentTitle
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
        public DateTime ObjectiveAssessmentDueDate
        {
            get => GetProperty<DateTime>();
            set => SetProperty(value);
        }
        public bool ObjectiveAssessmentNotification
        {
            get => GetProperty<bool>();
            set => SetProperty(value);
        }


        //**CONSTRUCTOR**//
        public CourseAddViewModel()
        {
            SaveCourseCommand = new Command(SaveCourseButton);
            TermToAddTo = TransferService.TermToTransfer;
            CourseStartDate = DateTime.Today;
            CourseEndDate = DateTime.Today;
            PerformanceAssessmentDueDate = DateTime.Today;
            ObjectiveAssessmentDueDate = DateTime.Today;
        }


        //**COMMANDS**//
        public ICommand SaveCourseCommand { get; }
        public async void SaveCourseButton()
        {
            bool errorPresent = false;
            string errorMessage = "";

            if (String.IsNullOrWhiteSpace(CourseTitle))
            {
                errorPresent = true;
                errorMessage += "The course must have a title. ";
            }
            else if (CourseStartDate >= CourseEndDate)
            {
                errorPresent = true;
                errorMessage += "The start date must be before the end date. ";
            }
            else if (String.IsNullOrEmpty(CourseStatus))
            {
                errorPresent = true;
                errorMessage += "You must select a status. ";
            }
            else if (String.IsNullOrWhiteSpace(InstructorName))
            {
                errorPresent = true;
                errorMessage += "You must add an instructor name. ";
            }
            else if (String.IsNullOrWhiteSpace(InstructorEmail))
            {
                errorPresent = true;
                errorMessage += "You must add an instructor email. ";
            }
            else if (String.IsNullOrWhiteSpace(InstructorPhone))
            {
                errorPresent = true;
                errorMessage += "You must add an instructor phone number. ";
            }
            else if (String.IsNullOrWhiteSpace(PerformanceAssessmentTitle))
            {
                errorPresent = true;
                errorMessage += "You must give the performance assessment a title. ";
            }
            else if (String.IsNullOrWhiteSpace(ObjectiveAssessmentTitle))
            {
                errorPresent = true;
                errorMessage += "You must give the objective assessment a title. ";
            }

            if (errorPresent)
            {
                await Application.Current.MainPage.DisplayAlert("Error", errorMessage, "OK");
            }
            else
            {
                int instructorId = await DatabaseService.FindInstructorAsync(InstructorName, InstructorEmail, InstructorPhone);
                if (instructorId == -1)
                {
                    //create new instructor and get the Id
                    Instructor instructor = new Instructor
                    {
                        Name = InstructorName,
                        Email = InstructorEmail,
                        Phone = InstructorPhone
                    };
                    instructorId = await DatabaseService.AddInstructorAsync(instructor);
                }

                Course course = new Course
                {
                    Title = CourseTitle,
                    StartDate = CourseStartDate,
                    EndDate = CourseEndDate,
                    Status = CourseStatus,
                    OptionalNotes = CourseOptionalNotes,
                    InstructorId = instructorId,
                    TermId = TermToAddTo.Id,
                    Notifications = CourseNotifications
                };
                int courseId = await DatabaseService.AddCourseAsync(course);

                //need to add assessments, assessment needs a courseId
                var objectiveAssessment = new Assessment
                {
                    CourseId = courseId,
                    Title = ObjectiveAssessmentTitle,
                    Type = "Objective",
                    DueDate = ObjectiveAssessmentDueDate,
                    Notifications = ObjectiveAssessmentNotification
                };
                await DatabaseService.AddAssessmentAsync(objectiveAssessment);

                var performanceAssessment = new Assessment
                {
                    CourseId = courseId,
                    Title = PerformanceAssessmentTitle,
                    Type = "Performance",
                    DueDate = PerformanceAssessmentDueDate,
                    Notifications = PerformanceAssessmentNotification
                };
                await DatabaseService.AddAssessmentAsync(performanceAssessment);

                MessagingCenter.Send(this, "DataUpdated");
                await Shell.Current.GoToAsync($"..");
            }

        }
    }
}
