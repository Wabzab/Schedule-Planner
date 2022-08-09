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

namespace Schedule_Planner
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        DatePicker startDate;
        DatePicker endDate;
        int numWeeks;
        NavigationService ns;

        public StartPage()
        {
            InitializeComponent();
            startDate = startDatePicker;
            endDate = endDatePicker;
            ns = NavigationService.GetNavigationService(this);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
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

            // Get the number of weeks and navigate to main page
            numWeeks = (int)((TimeSpan)(endDate.SelectedDate - startDate.SelectedDate)).TotalDays / 7;
            SemesterManager sm = new SemesterManager(numWeeks);
            this.NavigationService.Navigate(sm);
        }
    }
}
