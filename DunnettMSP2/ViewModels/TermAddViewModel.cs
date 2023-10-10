using System;
using System.Windows.Input;
using Xamarin.Forms;
using DunnettMSP2.Services;
using System.Threading.Tasks;
using DunnettMSP2.Models;
using System.Collections.ObjectModel;

namespace DunnettMSP2.ViewModels
{
    public class TermAddViewModel : BaseViewModel
    {
        //**VIEW BOUND PROPERTIES**//
        public string Title
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
        public DateTime StartTime
        {
            get => GetProperty<DateTime>();
            set => SetProperty(value);
        }
        public DateTime EndTime
        {
            get => GetProperty<DateTime>();
            set => SetProperty(value);
        }

        //**CONSTRUCTOR**//
        public TermAddViewModel()
        {
            SaveTermCommand = new Command(SaveTerm);
            EndTime = DateTime.Today;
            StartTime = DateTime.Today;
        }

        //**COMMANDS**//
        public ICommand SaveTermCommand { get; }
        public async void SaveTerm()
        {
            bool error = false;
            string errorMessage = "";

            if (String.IsNullOrWhiteSpace(Title))
            {
                error = true;
                errorMessage += "You must give the term a title. ";
            }
            if (StartTime >= EndTime)
            {
                error = true;
                errorMessage += "The term end date must be after the start date.";
            }

            if (error)
            {
                await Application.Current.MainPage.DisplayAlert("Error", errorMessage, "Ok");
            }
            else
            {
                Term addNewTerm = new Term
                {
                    Title = Title,
                    StartDate = StartTime,
                    EndDate = EndTime
                };

                _ = await DatabaseService.AddTermAsync(addNewTerm);
                MessagingCenter.Send(this, "DataUpdated");
                await Shell.Current.GoToAsync($"..");
            }
        }
    }
}
