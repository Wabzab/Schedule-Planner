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

using ModulesLibrary;

namespace SchedulePlanner
{
    /// <summary>
    /// Interaction logic for AddModulePage.xaml
    /// </summary>
    public partial class AddModulePage : Page
    {
        // Initialise variables
        SemesterManager managementPage;

        public AddModulePage(SemesterManager managementPageIn)
        {
            InitializeComponent();
            // Assign variables
            managementPage = managementPageIn;
        }

        // Handle submission of module click
        private void submitModule_Click(object sender, RoutedEventArgs e)
        {
            // Check fields are valid
            if (modName.Text.Equals(""))
            {
                errorText.Text = "Module name must be entered!";
                return;
            }

            if (modCode.Text.Equals(""))
            {
                errorText.Text = "Module code must be entered!";
                return;
            }

            if (modCredits.Text.Equals("") || !double.TryParse(modCredits.Text, out _))
            {
                errorText.Text = "Module credits must be entered and must be a number!";
                return;
            }

            if (modHours.Text.Equals("") || !double.TryParse(modHours.Text, out _))
            {
                errorText.Text = "Module hours must be entered and must be a number!";
                return;
            }

            // Create new module object
            Module module = new Module(modCode.Text, modName.Text, double.Parse(modCredits.Text), double.Parse(modHours.Text));
            // Access public method in MainWindow.xaml.cs
            managementPage.addModule(module);
            // Return to prev page
            this.NavigationService.GoBack();
        }

        // Handle cancel click
        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            // Return to prev page
            this.NavigationService.GoBack();
        }
    }
}
