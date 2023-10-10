using DunnettMSP2.Services;
using DunnettMSP2.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DunnettMSP2.ViewModels
{
    public class UserSettingsViewModel : BaseViewModel
    {
        //**VIEW BOUND PROPERTIES**//
        public bool RequirePasswordOnStartupCheckbox
        {
            get => SettingService.RequirePasswordOnStartup;
            set => SettingService.RequirePasswordOnStartup = value;
        }
        public bool AllowNotificationsCheckbox
        {
            get => SettingService.AllowNotifications;
            set => SettingService.AllowNotifications = value;
        }

        //**CONSTRUCTOR**//
        public UserSettingsViewModel()
        {
            RequirePasswordOnStartupCheckbox = SettingService.RequirePasswordOnStartup;
            AllowNotificationsCheckbox = SettingService.AllowNotifications;

            CourseSearchCommand = new Command(CourseSearch);
            CheckNotificationsCommand = new Command(CheckNotifications);
            DateReportCommand = new Command(DateReport);
        }

        //**COMMANDS**//
        public ICommand CourseSearchCommand { get; }
        public ICommand CheckNotificationsCommand { get; }
        public ICommand DateReportCommand { get; }
        private async void CourseSearch()
        {
            var route = $"{nameof(CourseSearchPage)}";
            await Shell.Current.GoToAsync(route);
        }
        private async void CheckNotifications()
        {
            if (AllowNotificationsCheckbox)
            {
                bool thereAreNotifications = NotificationService.HandleNotifications();
                if (!thereAreNotifications)
                {
                    await Application.Current.MainPage.DisplayAlert("Notifications", "You have no notifications for today.", "Ok");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Notifications are not enabled.", "Ok");
            }
        }
        private async void DateReport()
        {
            var route = $"{nameof(DateReportPage)}";
            await Shell.Current.GoToAsync(route);
        }


    }
}
