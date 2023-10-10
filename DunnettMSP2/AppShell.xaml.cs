
using DunnettMSP2.Views;
using Xamarin.Forms;

namespace DunnettMSP2
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();

            //page navigation registration
            Routing.RegisterRoute(nameof(TermAddPage), typeof(TermAddPage));
            Routing.RegisterRoute(nameof(TermDetailsPage), typeof(TermDetailsPage));
            Routing.RegisterRoute(nameof(CourseAddPage), typeof(CourseAddPage));
            Routing.RegisterRoute(nameof(CourseDetailsPage), typeof(CourseDetailsPage));
            Routing.RegisterRoute(nameof(TermEditPage), typeof(TermEditPage));
            Routing.RegisterRoute(nameof(CourseEditPage), typeof(CourseEditPage));
            Routing.RegisterRoute(nameof(CourseSearchPage), typeof(CourseSearchPage));
            Routing.RegisterRoute(nameof(AddDatePage), typeof(AddDatePage));
            Routing.RegisterRoute(nameof(DateReportPage), typeof(DateReportPage));
        }
    }
}
