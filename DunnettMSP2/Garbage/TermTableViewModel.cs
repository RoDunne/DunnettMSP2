using DunnettMSP2.Models;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using DunnettMSP2.Services;

namespace DunnettMSP2.ViewModels
{
    public class TermTableViewModel : BaseViewModel
    {
        //**VIEW BOUND PROPERTIES**//
        public ObservableCollection<Term> Terms
        {
            get => GetProperty<ObservableCollection<Term>>();
            set => SetProperty(value);
        }

        //**CONSTRUCTOR**//
        public TermTableViewModel()
        {
            Terms = new ObservableCollection<Term>(DatabaseService.GetTermList());
        }
    }
}
