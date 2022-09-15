using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SchedulePlanner
{
    /// <summary>
    /// Interaction logic for AddStudyPage.xaml
    /// </summary>
    public partial class AddStudyPage : Page
    {
        // Initialise variables
        SemesterManager managementPage;
        int selectedIndex;

        public AddStudyPage(SemesterManager managementPage, int selectedIndex)
        {
            InitializeComponent();
            // Assign variables
            this.managementPage = managementPage;
            this.selectedIndex = selectedIndex;
        }

        // Handle submission of self study
        private void submitStudy_Click(object sender, RoutedEventArgs e)
        {
            // Check fields are valid
            errorText.Text = "";
            if (studyDate.SelectedDate == null)
            {
                errorText.Text = "Date must have a value!";
                return;
            }

            if (studyHours.Text.Equals(""))
            {
                errorText.Text = "Hours must have a value!";
                return;
            }

            if (!double.TryParse(studyHours.Text, out _))
            {
                errorText.Text = "Hours must be a number!";
                return;
            }

            // Check day is in current week
            var cal = System.Globalization.DateTimeFormatInfo.CurrentInfo.Calendar;
            var today = DateTime.Now;
            var studyDay = (DateTime)studyDate.SelectedDate;

            var d1 = today.Date.AddDays(-1 * (int)cal.GetDayOfWeek(today));
            var d2 = studyDay.Date.AddDays(-1 * (int)cal.GetDayOfWeek(studyDay));
            if (d1 == d2)
            {
                managementPage.updateModule(selectedIndex, double.Parse(studyHours.Text));
            }

            // Return to prev page
            this.NavigationService.GoBack();
        }

        // Handle cancel button click
        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            // Return to prev page
            this.NavigationService.GoBack();
        }
    }
}
