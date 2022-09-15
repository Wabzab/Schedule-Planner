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
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        // Initialise variables
        DatePicker startDate;
        DatePicker endDate;
        int numWeeks;
        NavigationService ns;

        public StartPage()
        {
            InitializeComponent();
            // Assign variables
            startDate = startDatePicker;
            endDate = endDatePicker;
            ns = NavigationService.GetNavigationService(this);
        }

        // Handle button click
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Check fields are valid
            if (startDate.SelectedDate == null | endDate.SelectedDate == null)
            {
                errorText.Text = "All fields must have a value!";
                return;
            }

            if (startDate.SelectedDate >= endDate.SelectedDate)
            {
                errorText.Text = "Start date must be before the end date!";
                return;
            }

            // Get number of weeks and navigate to main page
#pragma warning disable CS8629 // Nullable value type may be null.
            numWeeks = (int)((TimeSpan)(endDate.SelectedDate - startDate.SelectedDate)).TotalDays / 7;
#pragma warning restore CS8629 // Nullable value type may be null.
            SemesterManager sm = new SemesterManager(numWeeks);
            this.NavigationService.Navigate(sm);
        }
    }
}
