using DunnettMSP2.Models;
using System;
using Xamarin.Forms;
using DunnettMSP2.Services;
using System.Windows.Input;
using Xamarin.Essentials;
using DunnettMSP2.Views;

namespace DunnettMSP2.ViewModels
{
    public class CourseDetailsViewModel : BaseViewModel
    {
        public Course CourseDetail { get; set; }
        public Instructor InstructorDetail { get; set; }
        public Assessment PerformanceAssessmentDetail { get; set; }
        public Assessment ObjectiveAssessmentDetail { get; set; }


        //**VIEW BOUND PROPERTIES**//
        public string CourseTitle
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
        public string CourseStartDate
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
        public string CourseEndDate
        {
            get => GetProperty<string>();
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
        public string CourseNotificationText
        {
            get => GetProperty<string>();
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
        public string PerformanceAssessmentDueDate
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
        public string PerformanceAssessmentNotificationText
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        public string ObjectiveAssessmentTitle
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
        public string ObjectiveAssessmentDueDate
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
        public string ObjectiveAssessmentNotificationText
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }


        //**CONSTRUCTOR**//
        public CourseDetailsViewModel()
        {
            EditCourseCommand = new Command(EditCoursePressed);
            ShareNotesCommand = new Command(ShareNotes);

            //get data from the transfer service
            CourseDetail = TransferService.CourseToTransfer;
            InstructorDetail = DatabaseService.GetInstructor(CourseDetail.InstructorId);
            PerformanceAssessmentDetail = DatabaseService.GetPerformanceAssessment(CourseDetail.Id);
            ObjectiveAssessmentDetail = DatabaseService.GetObjectiveAssessment(CourseDetail.Id);
            
            SetPageProperties();

            //refresh data
            MessagingCenter.Subscribe<CourseEditViewModel>(this, "DataUpdated", (sender) =>
            {
                //refresh data
                CourseDetail = TransferService.CourseToTransfer;
                InstructorDetail = DatabaseService.GetInstructor(CourseDetail.InstructorId);
                PerformanceAssessmentDetail = DatabaseService.GetPerformanceAssessment(CourseDetail.Id);
                ObjectiveAssessmentDetail = DatabaseService.GetObjectiveAssessment(CourseDetail.Id);
                
                SetPageProperties();

            });
        }

        //Sets the page properties to the data passed into the ViewModel
        private void SetPageProperties()
        {
            CourseTitle = CourseDetail.Title;
            CourseStartDate = $"Course Start:  {CourseDetail.StartDate.ToString("M/d/yyyy")}";
            CourseEndDate = $"Course End:  {CourseDetail.EndDate.ToString("M/d/yyyy")}";
            CourseStatus = $"Status:  {CourseDetail.Status}";
            CourseOptionalNotes = CourseDetail.OptionalNotes;
            CourseNotificationText = CourseDetail.Notifications ? "Start/End Notifications-- ON" : "Start/End Notifications-- OFF";

            InstructorName = InstructorDetail.Name;
            InstructorEmail = InstructorDetail.Email;
            InstructorPhone = InstructorDetail.Phone;

            PerformanceAssessmentTitle = PerformanceAssessmentDetail.Title;
            PerformanceAssessmentDueDate = PerformanceAssessmentDetail.DueDate.ToString("M/d/yyyy");
            PerformanceAssessmentNotificationText = PerformanceAssessmentDetail.Notifications ? "DueDate Notifications-- ON" : "DueDate Notifications-- OFF";

            ObjectiveAssessmentTitle = ObjectiveAssessmentDetail.Title;
            ObjectiveAssessmentDueDate = ObjectiveAssessmentDetail.DueDate.ToString("M/d/yyyy");
            ObjectiveAssessmentNotificationText = ObjectiveAssessmentDetail.Notifications ? "DueDate Notifications-- ON" : "DueDate Notifications-- OFF";
        }


        //**COMMANDS**//
        public ICommand EditCourseCommand { get; }
        public ICommand ShareNotesCommand { get; }

        public async void EditCoursePressed()
        {
            TransferService.CourseToTransfer = CourseDetail;
            TransferService.InstructorToTransfer = InstructorDetail;
            TransferService.ObjectiveAssessmentToTransfer = ObjectiveAssessmentDetail;
            TransferService.PerformanceAssessmentToTransfer = PerformanceAssessmentDetail;

            var route = $"{nameof(CourseEditPage)}";
            await Shell.Current.GoToAsync(route);
        }

        public async void ShareNotes()
        {
            if (CourseOptionalNotes == null)
            {
                await Application.Current.MainPage.DisplayAlert("", "You do not have any notes to share.", "Ok");
            }
            else
            {
                await Share.RequestAsync(new ShareTextRequest
                {
                    Text = CourseOptionalNotes,
                    Title = "Share Text"
                });
            }
        }
    }
}
