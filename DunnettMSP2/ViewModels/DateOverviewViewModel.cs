using System.Collections.ObjectModel;
using System.Windows.Input;
using DunnettMSP2.Models;
using DunnettMSP2.Services;
using DunnettMSP2.Views;
using Xamarin.Forms;

namespace DunnettMSP2.ViewModels
{
    public class DateOverviewViewModel : BaseViewModel
    {
        //**VIEW BOUND PROPERTIES**//
        public ObservableCollection<RomanticDate> RomanticDates
        {
            get => GetProperty<ObservableCollection<RomanticDate>>();
            set => SetProperty(value);
        }

        //**CONSTRUCTOR**//
        public DateOverviewViewModel()
        {
            MessagingCenter.Subscribe<UtilitiesViewModel>(this, "DataUpdated", (sender) =>
            {
                Refresh();
            });
            MessagingCenter.Subscribe<AddDateViewModel>(this, "DataUpdated", (sender) =>
            {
                Refresh();
            });
            AddDateCommand = new Command(AddDate);
            DeleteDateCommand = new Command(DeleteDate);

            Refresh();
        }

        //**METHODS**//
        public void Refresh()
        {
            RomanticDates = new ObservableCollection<RomanticDate>(DatabaseService.GetAllRomanticDates());
        }

        //**COMMANDS**//
        public ICommand AddDateCommand { get; }
        public ICommand DeleteDateCommand { get; }
 
        private async void AddDate()
        {
            //navigate to add event page
            string route = $"{nameof(AddDatePage)}";
            await Shell.Current.GoToAsync(route);
        }
        private async void DeleteDate(object obj)
        {
            RomanticDate dateToRemove = obj as RomanticDate;

            bool answerYes = await Application.Current.MainPage.DisplayAlert("Delete Confirmation", "Are you sure you want to delete?", "Yes", "No");
            if (answerYes)
            {
                await DatabaseService.RemoveRomanticDateAsync(dateToRemove.Id);
                Refresh();
            }
        }

    }
}
