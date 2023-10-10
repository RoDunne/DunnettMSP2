using DunnettMSP2.Models;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using DunnettMSP2.Services;

namespace DunnettMSP2.ViewModels
{
    public class InstructorTableViewModel : BaseViewModel
    {
        //**VIEW BOUND PROPERTIES**//
        public ObservableCollection<Instructor> Instructors
        {
            get => GetProperty<ObservableCollection<Instructor>>();
            set => SetProperty(value);
        }

        //**CONSTRUCTOR**//
        public InstructorTableViewModel()
        {
            Instructors = new ObservableCollection<Instructor>(DatabaseService.GetAllInstructors());
        }

    }
}