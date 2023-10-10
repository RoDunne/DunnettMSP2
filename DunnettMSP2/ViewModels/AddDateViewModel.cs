using DunnettMSP2.Models;
using DunnettMSP2.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace DunnettMSP2.ViewModels
{
    public class AddDateViewModel : BaseViewModel
    {
        //**VIEW BOUND PROPERTIES**//
        public string RomanticDateName
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
        public string RomanticDateLocation
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
        public DateTime RomanticDateDate
        {
            get => GetProperty<DateTime>();
            set => SetProperty(value);
        }
        public TimeSpan RomanticDateTime
        {
            get => GetProperty<TimeSpan>();
            set => SetProperty(value);
        }
        public string RomanticDatePhone
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        //**CONSTRUCTOR**//
        public AddDateViewModel()
        {
            RomanticDateDate = DateTime.Today;
            RomanticDateTime = TimeSpan.FromHours(12);
            SaveDateCommand = new Command(SaveDate);
        }

        //**COMMANDS**//
        public ICommand SaveDateCommand { get; }
        private async void SaveDate()
        {
            if (String.IsNullOrWhiteSpace(RomanticDateLocation) || String.IsNullOrWhiteSpace(RomanticDateName))
            {
                string errorMsg = "You must enter a date location and your date's name.";
                await Application.Current.MainPage.DisplayAlert("Error", errorMsg, "Ok");
            }
            else
            {
                var newDate = new RomanticDate
                {
                    StartDateAndTime = RomanticDateDate + RomanticDateTime,
                    Location = RomanticDateLocation,
                    Name = RomanticDateName,
                    PhoneNumber = RomanticDatePhone
                };

                bool noError = true;
                try
                {
                    _ = await DatabaseService.AddRomanticDateAsync(newDate);
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"Could not save to database: {ex}", "Ok");
                    noError = false;
                }

                if (noError)
                {
                    MessagingCenter.Send(this, "DataUpdated");
                    await Shell.Current.GoToAsync($"..");
                }
            }
        }
    }
}
