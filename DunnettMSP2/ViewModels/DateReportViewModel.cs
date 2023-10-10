using System;
using System.Collections.ObjectModel;
using DunnettMSP2.Models;
using DunnettMSP2.Services;

namespace DunnettMSP2.ViewModels
{
    public class DateReportViewModel : BaseViewModel
    {
        //**VIEW BOUND PROPERTIES**//
        public ObservableCollection<RomanticDate> RomanticDates
        {
            get => GetProperty<ObservableCollection<RomanticDate>>();
            set => SetProperty(value);
        }
        public DateTime ReportTime
        {
            get => GetProperty<DateTime>();
            set => SetProperty(value);
        }
        public string ReportTimeString
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }

        //**CONSTRUCTOR**//
        public DateReportViewModel()
        {
            RomanticDates = new ObservableCollection<RomanticDate>(DatabaseService.GetAllRomanticDates());
            ReportTime = DateTime.Now;
            ReportTimeString = $"Report Generated On: {ReportTime:g}";
        }
    }
}
