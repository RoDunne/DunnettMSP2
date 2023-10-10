using DunnettMSP2.Models;
using DunnettMSP2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace DunnettMSP2.ViewModels
{
    public class CourseSearchViewModel : BaseViewModel
    {
        //**VIEW BOUND PROPERTIES**//
        public ObservableCollection<Course> Courses
        {
            get => GetProperty<ObservableCollection<Course>>();
            set => SetProperty(value);
        }
        public string SearchField
        {
            get => GetProperty<string>();
            set => SetProperty(value);
        }


        //**CONSTRUCTOR**//
        public CourseSearchViewModel()
        {
            SearchCommand = new Command(Search);
        }


        //**COMMANDS**//
        public ICommand SearchCommand { get; }
        public async void Search()
        {
            if (Courses != null)
            {
                Courses.Clear();
                Courses = null;
            }
            if (String.IsNullOrWhiteSpace(SearchField))
            {

            }
            else
            {
                var search = await DatabaseService.GetCoursesFromSearchAsync(SearchField);
                Courses = new ObservableCollection<Course>(search);
            }
        }
    }
}
