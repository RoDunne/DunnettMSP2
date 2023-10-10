using DunnettMSP2.Models;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using DunnettMSP2.Services;

namespace DunnettMSP2.ViewModels
{
    public class CourseTableViewModel : BaseViewModel
    {
        //**VIEW BOUND PROPERTIES**//
        public ObservableCollection<Course> Courses
        {
            get => GetProperty<ObservableCollection<Course>>();
            set => SetProperty(value);
        }

        //**CONSTRUCTOR**//
        public CourseTableViewModel()
        {
            Courses = new ObservableCollection<Course>(DatabaseService.GetAllCourses());
        }

    }
}
