using DunnettMSP2.Models;
using Xamarin.Forms;
using System;
using System.Windows.Input;
using DunnettMSP2.Services;

namespace DunnettMSP2.ViewModels
{
    public class TermEditViewModel : BaseViewModel
    {
        public Term TermToEdit { get; set; }

        //**VIEW BOUND PROPERTIES**//
        public string TermTitle
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }
        public DateTime TermStart
        {
            get => GetProperty<DateTime>();
            set => SetProperty(value);
        }
        public DateTime TermEnd
        {
            get => GetProperty<DateTime>();
            set => SetProperty(value);
        }

        //**CONSTRUCTOR**//
        public TermEditViewModel()
        {
            SaveTermCommand = new Command(SaveTerm);
            TermToEdit = TransferService.TermToTransfer;
            TermTitle = TermToEdit.Title;
            TermStart = TermToEdit.StartDate;
            TermEnd = TermToEdit.EndDate;
        }

        //**COMMANDS**//
        public ICommand SaveTermCommand { get; }
        public async void SaveTerm()
        {
            bool error = false;
            string errorMessage = "";

            if (String.IsNullOrWhiteSpace(TermTitle))
            {
                error = true;
                errorMessage += "You must give the term a title. ";
            }
            if (TermStart >= TermEnd)
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
                Term updateTerm = new Term
                {
                    Id = TermToEdit.Id,
                    Title = TermTitle,
                    StartDate = TermStart,
                    EndDate = TermEnd
                };

                await DatabaseService.UpdateTermAsync(updateTerm);
                TransferService.TermToTransfer = DatabaseService.GetTerm(updateTerm.Id);
                MessagingCenter.Send(this, "DataUpdated");
                await Shell.Current.GoToAsync($"..");
            }
        }
    }
}
