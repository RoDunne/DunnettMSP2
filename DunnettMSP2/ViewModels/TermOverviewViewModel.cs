using DunnettMSP2.Models;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Windows.Input;
using DunnettMSP2.Views;
using DunnettMSP2.Services;
using System.Collections.Generic;
using System.Linq;

namespace DunnettMSP2.ViewModels
{
    public class TermOverviewViewModel : BaseViewModel
    {
        //**VIEW BOUND PROPERTIES**//
        public ObservableCollection<Term> Terms
        {
            get => GetProperty<ObservableCollection<Term>>();
            set => SetProperty(value);
        }

        //**CONSTRUCTOR**//
        public TermOverviewViewModel()
        {
            DeleteTermCommand = new Command(DeleteTerm);
            AddTermCommand = new Command(AddTermButton);
            TermTappedCommand = new Command(TermTapped);

            Terms = new ObservableCollection<Term>(DatabaseService.GetTermList());

            NotificationService.HandleNotifications();

            MessagingCenter.Subscribe<TermAddViewModel>(this, "DataUpdated", async (sender) =>
            {
                // Load data from the database to update the ObservableCollection
                Terms = new ObservableCollection<Term>(await DatabaseService.GetTermListAsync());
            });

            MessagingCenter.Subscribe<TermEditViewModel>(this, "DataUpdated", async (sender) =>
            {
                // Load data from the database to update the ObservableCollection
                Terms = new ObservableCollection<Term>(await DatabaseService.GetTermListAsync());
            });

            MessagingCenter.Subscribe<UtilitiesViewModel>(this, "DataUpdated", async (sender) =>
            {
                // Load data from the database to update the ObservableCollection
                Terms = new ObservableCollection<Term>(await DatabaseService.GetTermListAsync());
            });
        }

        //**COMMANDS**//
        public ICommand DeleteTermCommand { get; }
        public ICommand AddTermCommand { get; }
        public ICommand TermTappedCommand { get; }

        async void TermTapped(object o)
        {
            Term termToDetail = o as Term;

            TransferService.TermToTransfer = termToDetail;
            string route = $"{nameof(TermDetailsPage)}";
            await Shell.Current.GoToAsync(route);
        }
        async void DeleteTerm(object o)
        {
            Term termToRemove = o as Term;
            bool answerYes = await Application.Current.MainPage.DisplayAlert("Delete Confirmation", "Are you sure you want to delete?", "Yes", "No");
            if (answerYes)
            {
                //delete the courses from the term first
                var termCoursesIEnum = await DatabaseService.GetCoursesForTermAsync(termToRemove.Id);
                List<Course> termCoursesList = termCoursesIEnum.ToList();
                foreach (Course c in termCoursesList)
                {
                    await DatabaseService.RemoveCourseAssessmentsAsync(c.Id);
                    await DatabaseService.RemoveCourseAsync(c.Id);

                    //if the instructor teaches no other courses, remove them from the database
                    List<Course> instructorCourses = await DatabaseService.GetCoursesTaughtByInstructor(c.InstructorId);
                    if (instructorCourses.Count == 0)
                    {
                        await DatabaseService.RemoveInstructorAsync(c.InstructorId);
                    }
                }

                //finally delete the term
                await DatabaseService.RemoveTermAsync(termToRemove.Id);
                Terms = new ObservableCollection<Term>(await DatabaseService.GetTermListAsync());
            }
        }
        async void AddTermButton()
        {
            var route = $"{nameof(TermAddPage)}";
            await Shell.Current.GoToAsync(route);
        }
    }
}
