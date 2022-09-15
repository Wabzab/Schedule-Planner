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
// Custom Library for Modules
using ModulesLibrary;

namespace SchedulePlanner
{
    /// <summary>
    /// Interaction logic for SemesterManager.xaml
    /// </summary>
    public partial class SemesterManager : Page
    {

        // Create class library for modules - X
        // Functionality for adding/removing modules - X
        // Smack this project in the face - XXX

        int numWeeks;

        // Generic collection of modules
        List<Module> moduleList = new List<Module>() { new Module("PROG6212", "Programming 2B", 15, 5), new Module("CLDV6212", "Cloud Development 1B", 15, 5) };

        public SemesterManager(int numWeeksIn)
        {
            InitializeComponent();
            numWeeks = numWeeksIn;
            weeksText.Text = String.Format("Weeks: {0}", numWeeks);

            // LINQ query to select an existing type from moduleList //
            var modules = moduleList.Select(
                x => new Module(x.code, x.name, x.credits, x.hours)
                    {
                        name = x.name,
                        code = x.code,
                        selfHoursLeft = x.selfHoursLeft,
                        selfstudyHours = x.selfstudyHours
                    }
                );
            // --- End of LINQ query -- //
            foreach (Module m in modules)
            {
                displayModule(m);
            }
        }

        // --- Button Click events --- //

        // Handle removal of modules
        private void rmvClick(object sender, RoutedEventArgs e)
        {
            if(moduleListBox.SelectedItem != null)
            {
                int moduleIndex = moduleListBox.SelectedIndex;
                moduleListBox.Items.RemoveAt(moduleIndex);
                moduleList.RemoveAt(moduleIndex);
            }
        }

        // Handle addition of modules
        private void addClick(object sender, RoutedEventArgs e)
        {
            AddModulePage amPage = new AddModulePage(this);
            this.NavigationService.Navigate(amPage);
        }

        // Handle addition of self studying
        private void addStudy(object sender, RoutedEventArgs e)
        {
            if (moduleListBox.SelectedItem != null)
            {
                AddStudyPage asPage = new AddStudyPage(this, moduleListBox.SelectedIndex);
                this.NavigationService.Navigate(asPage);
            }
        }

        // --- Helper Functions --- //

        public void addModule(Module module)
        {
            // Adds module to moduleList and displays it
            moduleList.Add(module);
            displayModule(module);
        }

        public void updateModule(int index, double hours)
        {
            // Updates an existing module
            Module module = moduleList[index];
            module.selfHoursLeft -= hours;
            ListBoxItem li = (ListBoxItem)moduleListBox.Items.GetItemAt(index);
            li.Content = String.Format("{0} - {1} | Study hours: {2} / {3}", module.name, module.code, (int)module.selfHoursLeft, (int)module.selfstudyHours);
        }

        private void displayModule(Module module)
        {
            // Logic for displaying a module
            module.calculateSelfstudy(numWeeks);
            ListBoxItem listBoxItem = new ListBoxItem();
            listBoxItem.Content = String.Format("{0} - {1} | Study hours: {2} / {3}", module.name, module.code, (int)module.selfHoursLeft, (int)module.selfstudyHours);
            moduleListBox.Items.Add(listBoxItem);
        }
    }
}
