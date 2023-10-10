using DunnettMSP2.Models;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System;
using System.Windows.Input;
using DunnettMSP2.Views;
using DunnettMSP2.Services;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace DunnettMSP2.ViewModels
{
    public class TermDetailsViewModel : BaseViewModel
    {
        public Term DetailTerm { get; set; }

        //**VIEW BOUND PROPERTIES**//
        public string TermTitle
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
        public string TermStart
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
        public string TermEnd
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
        public ObservableCollection<Course> Courses
        {
            get => GetProperty<ObservableCollection<Course>>();
            set => SetProperty(value);
        }

        //**CONSTRUCTOR**//
        public TermDetailsViewModel()
        {
            DeleteCourseCommand = new Command(DeleteCourseButton);
            EditTermCommand = new Command(EditTermButton);
            AddCourseCommand = new Command(AddCourseButton);
            CourseTappedCommand = new Command(CourseTapped);

            DetailTerm = TransferService.TermToTransfer;
            TermTitle = $"{DetailTerm.Title}";
            TermStart = $"Start:  {DetailTerm.StartDate:M/d/yyyy}";
            TermEnd = $"End:  {DetailTerm.EndDate:M/d/yyyy}";
            Courses = new ObservableCollection<Course>(DatabaseService.GetCoursesForTerm(DetailTerm.Id));

            //Listen for data updated events to refresh data views
            MessagingCenter.Subscribe<CourseAddViewModel>(this, "DataUpdated", async (sender) =>
            {
                Courses = new ObservableCollection<Course>(await DatabaseService.GetCoursesForTermAsync(DetailTerm.Id));
            });
            MessagingCenter.Subscribe<CourseEditViewModel>(this, "DataUpdated", async (sender) =>
            {
                Courses = new ObservableCollection<Course>(await DatabaseService.GetCoursesForTermAsync(DetailTerm.Id));
            });
            MessagingCenter.Subscribe<TermEditViewModel>(this, "DataUpdated", async (sender) =>
            {
                Debug.WriteLine("message DataUpated recieved");
                DetailTerm = TransferService.TermToTransfer;
                TermTitle = $"{DetailTerm.Title}";
                TermStart = $"Start:  {DetailTerm.StartDate:M/d/yyyy}" ;
                TermEnd = $"End:  {DetailTerm.EndDate:M/d/yyyy}";
                Courses = new ObservableCollection<Course>(await DatabaseService.GetCoursesForTermAsync(DetailTerm.Id));
            });
        }

        //**COMMANDS**//
        public ICommand DeleteCourseCommand { get; }
        public ICommand EditTermCommand { get; }
        public ICommand AddCourseCommand { get; }
        public ICommand CourseTappedCommand { get; }

        async void DeleteCourseButton(object o)
        {
            Course courseToRemove = o as Course;
            bool confirmation = await Application.Current.MainPage.DisplayAlert("Delete Confirmation", "Are you sure you want to delete?", "Yes", "No");
            if (confirmation)
            {
                await DatabaseService.RemoveCourseAssessmentsAsync(courseToRemove.Id);
                await DatabaseService.RemoveCourseAsync(courseToRemove.Id);
                Courses = new ObservableCollection<Course>(await DatabaseService.GetCoursesForTermAsync(DetailTerm.Id));

                //if the instructor teaches no other courses, remove them from the database
                List<Course> instructorCourses = await DatabaseService.GetCoursesTaughtByInstructor(courseToRemove.InstructorId);
                if (instructorCourses.Count == 0)
                {
                    await DatabaseService.RemoveInstructorAsync(courseToRemove.InstructorId);
                }
            }
        }
        async void EditTermButton()
        {
            TransferService.TermToTransfer = DetailTerm;
            string route = $"{nameof(TermEditPage)}";
            await Shell.Current.GoToAsync(route);
        }
        async void AddCourseButton()
        {
            if (Courses.Count >= 6)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "You cannot add more than six courses to a term!", "OK");
            }
            else
            {
                TransferService.TermToTransfer = DetailTerm;
                var route = $"{nameof(CourseAddPage)}";
                await Shell.Current.GoToAsync(route);
            }

        }
        async void CourseTapped(object o)
        {
            Course course = o as Course;

            TransferService.CourseToTransfer = course;
            string route = $"{nameof(CourseDetailsPage)}";
            await Shell.Current.GoToAsync(route);
        }

    }
}
