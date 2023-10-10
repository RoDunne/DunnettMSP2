using DunnettMSP2.Models;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using DunnettMSP2.Services;

namespace DunnettMSP2.ViewModels
{
    public class CourseEditViewModel : BaseViewModel
    {
        public Course CourseToEdit { get; set; }
        public Instructor InstructorToEdit { get; set; }
        public Assessment ObjectiveAssessmentToEdit { get; set; }
        public Assessment PerformanceAssessmentToEdit { get; set; }

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
        public CourseEditViewModel()
        {
            SaveCourseCommand = new Command(SaveCourse);
            CourseToEdit = TransferService.CourseToTransfer;
            InstructorToEdit = TransferService.InstructorToTransfer;
            PerformanceAssessmentToEdit = TransferService.PerformanceAssessmentToTransfer;
            ObjectiveAssessmentToEdit = TransferService.ObjectiveAssessmentToTransfer;

            CourseTitle = CourseToEdit.Title;
            CourseStartDate = CourseToEdit.StartDate;
            CourseEndDate = CourseToEdit.EndDate;
            CourseStatus = CourseToEdit.Status;
            CourseOptionalNotes = CourseToEdit.OptionalNotes;
            CourseNotifications = CourseToEdit.Notifications;

            InstructorName = InstructorToEdit.Name;
            InstructorEmail = InstructorToEdit.Email;
            InstructorPhone = InstructorToEdit.Phone;

            PerformanceAssessmentTitle = PerformanceAssessmentToEdit.Title;
            PerformanceAssessmentDueDate = PerformanceAssessmentToEdit.DueDate;
            PerformanceAssessmentNotification = PerformanceAssessmentToEdit.Notifications;

            ObjectiveAssessmentTitle = ObjectiveAssessmentToEdit.Title;
            ObjectiveAssessmentDueDate = ObjectiveAssessmentToEdit.DueDate;
            ObjectiveAssessmentNotification = ObjectiveAssessmentToEdit.Notifications;
        }

        //**COMMANDS**//
        public ICommand SaveCourseCommand { get; }
        public async void SaveCourse()
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
                //lookup an intructor already existing in the database (relationship note: one instructor can teach many courses)
                //returns -1 if none found
                int instructorId = await DatabaseService.FindInstructorAsync(InstructorName, InstructorEmail, InstructorPhone);

                //an instructor is not found, we must make a new one
                if (instructorId == -1)
                {
                    Instructor instructor = new Instructor
                    {
                        Name = InstructorName,
                        Email = InstructorEmail,
                        Phone = InstructorPhone
                    };
                    instructorId = await DatabaseService.AddInstructorAsync(instructor);
                }
                TransferService.InstructorToTransfer = DatabaseService.GetInstructor(instructorId);



                Course course = new Course
                {
                    Id = CourseToEdit.Id,
                    Title = CourseTitle,
                    StartDate = CourseStartDate,
                    EndDate = CourseEndDate,
                    Status = CourseStatus,
                    OptionalNotes = CourseOptionalNotes,
                    InstructorId = instructorId,
                    TermId = CourseToEdit.TermId,
                    Notifications = CourseNotifications
                };
                await DatabaseService.UpdateCourseAsync(course);
                TransferService.CourseToTransfer = DatabaseService.GetCourse(course.Id);

                /*Now that the course is updated, we must check and delete the "previous" instructor from the database
                  if they are not teaching any courses anymore. If the instructor was not changed, then it will find the current
                  course and not delete him*/
                List<Course> instructorCourses = await DatabaseService.GetCoursesTaughtByInstructor(InstructorToEdit.Id);
                if (instructorCourses.Count == 0)
                {
                    await DatabaseService.RemoveInstructorAsync(InstructorToEdit.Id);
                }

                Assessment objectiveAssessment = new Assessment
                {
                    Id = ObjectiveAssessmentToEdit.Id,
                    CourseId = CourseToEdit.Id,
                    Title = ObjectiveAssessmentTitle,
                    Type = "Objective",
                    DueDate = ObjectiveAssessmentDueDate,
                    Notifications = ObjectiveAssessmentNotification
                };
                await DatabaseService.UpdateAssessmentAsync(objectiveAssessment);
                TransferService.ObjectiveAssessmentToTransfer = DatabaseService.GetAssessment(objectiveAssessment.Id);

                Assessment performanceAssessment = new Assessment
                {
                    Id = PerformanceAssessmentToEdit.Id,
                    CourseId = CourseToEdit.Id,
                    Title = PerformanceAssessmentTitle,
                    Type = "Performance",
                    DueDate = PerformanceAssessmentDueDate,
                    Notifications = PerformanceAssessmentNotification
                };
                await DatabaseService.UpdateAssessmentAsync(performanceAssessment);

                TransferService.PerformanceAssessmentToTransfer = DatabaseService.GetAssessment(performanceAssessment.Id);
                MessagingCenter.Send(this, "DataUpdated");
                await Shell.Current.GoToAsync($"..");
            }
        }
    }
}
