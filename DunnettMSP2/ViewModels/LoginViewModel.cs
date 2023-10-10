using System;
using System.Collections.Generic;
using System.Text;
using DunnettMSP2.ViewModels;
using DunnettMSP2.Services;
using DunnettMSP2.Models;
using System.Windows.Input;
using Xamarin.Forms;
using DunnettMSP2.Views;
using System.Threading.Tasks;

namespace DunnettMSP2.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {

        //**VIEW BOUND PROPERTIES**//
        public string Username
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
        public string Password
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
        public bool FirstLogin
        {
            get => GetProperty<bool>();
            set => SetProperty(value);
        }
        public string InfoLabel
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
        public string SaveButtonText
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        //**CONSTRUCTOR**//
        public LoginViewModel()
        {
            if (SettingService.FirstRun || SettingService.Username == "")
            {
                FirstLogin = true;
                InfoLabel = "Please create a username and password";
                SaveButtonText = "Save";
            }
            else
            {
                if (SettingService.RequirePasswordOnStartup)
                {
                    FirstLogin = false;
                    Username = SettingService.Username;
                    InfoLabel = "Please enter your password to login.";
                    SaveButtonText = "Login";
                }
                else
                {
                    string route = $"//{nameof(TermOverviewPage)}";
                    _ = Shell.Current.GoToAsync(route);
                }
            }

            SaveCommand = new Command(SaveAsync);

        }

        //**COMMANDS**//
        public ICommand SaveCommand { get; }
        public async void SaveAsync()
        {
            if (FirstLogin)
            {
                if (String.IsNullOrWhiteSpace(Username) || String.IsNullOrWhiteSpace(Password))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "You must enter a username and password.", "Ok");
                }
                else
                {
                    SettingService.Username = Username;
                    SettingService.Password = Password;
                    string route = $"//{nameof(TermOverviewPage)}";
                    await Shell.Current.GoToAsync(route);
                }
            }
            else
            {
                if (Password != SettingService.Password)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "The password was incorrect. Please try again.", "Ok");
                }
                else
                {
                    string route = $"//{nameof(TermOverviewPage)}";
                    await Shell.Current.GoToAsync(route);
                }
            }
        }
    }
}
