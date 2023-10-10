using DunnettMSP2.Services;
using System.Windows.Input;
using Xamarin.Forms;
using DunnettMSP2.Views;

namespace DunnettMSP2.ViewModels
{
    public class UtilitiesViewModel : BaseViewModel
    {
        //**CONSTRUCTOR**//
        public UtilitiesViewModel()
        {
            ClearDatabaseCommand = new Command(ClearDatabase);
            LoadSampleDataCommand = new Command(LoadSampleData);
        }

        //**COMMANDS**//
        public ICommand ClearDatabaseCommand { get; }
        public ICommand LoadSampleDataCommand { get; }


        public async void ClearDatabase()
        {
            bool answerYes = await Application.Current.MainPage.DisplayAlert("Clear Confirmation", "Are you sure you want to clear the database?", "Yes", "No");
            if (answerYes)
            {
                await DatabaseService.ClearDatabaseAsync();
                MessagingCenter.Send(this, "DataUpdated");
                await Application.Current.MainPage.DisplayAlert("", "Database cleared.", "Ok");
            }

        }
        public async void LoadSampleData()
        {
            bool answerYes = await Application.Current.MainPage.DisplayAlert("Load Sample Data", "To prevent duplicate data, clear the database first or delete the old sample data.", "Add Sample Data", "Cancel");
            if (answerYes)
            {
                await DatabaseService.LoadSampleDataAsync();
                MessagingCenter.Send(this, "DataUpdated");
                await Application.Current.MainPage.DisplayAlert("", "Sample data added.", "Ok");
            }

        }
    }

}
