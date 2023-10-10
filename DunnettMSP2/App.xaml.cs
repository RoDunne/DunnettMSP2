using DunnettMSP2.Services;
using Xamarin.Forms;
using DunnettMSP2.ViewModels;
using DunnettMSP2.Views;

namespace DunnettMSP2
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            
            if (SettingService.FirstRun)
            {
                DatabaseService.LoadSampleData();
                SettingService.FirstRun = false;
            }

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
