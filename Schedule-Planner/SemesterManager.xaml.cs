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
    /// Interaction logic for SemesterManager.xaml
    /// </summary>
    public partial class SemesterManager : Page
    {

        // Create class library for modules
        // Functionality for adding/removing modules
        // Smack this project in the face

        int numWeeks;

        // Change to generic collection of modules
        string[] items = { "PROG", "CLDV", "SOEN", "SAND", "MAPC", "NWEG", "DBAS" };

        public SemesterManager(int numWeeksIn)
        {
            InitializeComponent();
            numWeeks = numWeeksIn;
            foreach (string item in items)
            {
                ListBoxItem listBoxItem = new ListBoxItem();
                listBoxItem.Content = item;
                moduleList.Items.Add(listBoxItem);
            }
        }
    }
}
