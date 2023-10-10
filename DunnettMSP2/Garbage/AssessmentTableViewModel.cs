using DunnettMSP2.Models;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using DunnettMSP2.Services;

namespace DunnettMSP2.ViewModels
{
    public class AssessmentTableViewModel : BaseViewModel
    {
        //**PAGE BOUND PROPERTIES**//
        public ObservableCollection<Assessment> Assessments
        {
            get => GetProperty<ObservableCollection<Assessment>>();
            set => SetProperty(value);
        }


        //**CONSTRUCTOR**//
        public AssessmentTableViewModel()
        {
            Assessments = new ObservableCollection<Assessment>(DatabaseService.GetAllAssessments());
        }
    }
}
