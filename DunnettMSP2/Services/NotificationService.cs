using System.Collections.Generic;
using DunnettMSP2.Models;
using Plugin.LocalNotifications;
using Xamarin.Forms;

namespace DunnettMSP2.Services
{
    public static class NotificationService
    {
        public static bool HandleNotifications()
        {
            bool IsNotifications = false;

            if (SettingService.AllowNotifications)
            {
                //get a list of notification events ocurring today from the database 
                List<Assessment> assessmentsDueToday = DatabaseService.GetAssessmentsDueToday();
                List<Course> coursesStartingToday = DatabaseService.GetCoursesThatBeginToday();
                List<Course> coursesEndingToday = DatabaseService.GetCoursesThatEndToday();
                List<RomanticDate> datesToday = DatabaseService.GetDatesToday();

                //use a notification Id so each notification is unique and not overwritten
                int notifyId = 0;

                try
                {
                    foreach (Assessment a in assessmentsDueToday)
                    {
                        IsNotifications = true;
                        CrossLocalNotifications.Current.Show("Assessment Alert", $"{a.Title} is due today!", notifyId++);
                    }
                    foreach (Course c in coursesStartingToday)
                    {
                        IsNotifications = true;
                        CrossLocalNotifications.Current.Show("Course Alert", $"{c.Title} starts today!", notifyId++);
                    }
                    foreach (Course c in coursesEndingToday)
                    {
                        IsNotifications = true;
                        CrossLocalNotifications.Current.Show("Course Alert", $"{c.Title} ends today!", notifyId++);
                    }
                    foreach (RomanticDate r in datesToday)
                    {
                        IsNotifications = true;
                        CrossLocalNotifications.Current.Show("Date Alert", $"{r.ToStringEvent()}!", notifyId++);
                    }
                }
                catch (System.Exception ex)
                {
                    Application.Current.MainPage.DisplayAlert("Error", $"There was an error showing notifications: {ex}", "Okay");

                }
            }

            return IsNotifications;
        }
    }
}
