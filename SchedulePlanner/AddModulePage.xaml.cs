using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SchedulePlanner
{
    /// <summary>
    /// Interaction logic for AddModulePage.xaml
    /// </summary>
    public partial class AddModulePage : Page
    {
        // Initialise variables
        SemesterManager managementPage;
        int userID;
        int weeks;

        public AddModulePage(SemesterManager managementPageIn, int userId, int weeks)
        {
            InitializeComponent();
            // Assign variables
            managementPage = managementPageIn;
            userID = userId;
            this.weeks = weeks;
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
            module.calculateSelfstudy(weeks);
            // Access public method in MainWindow.xaml.cs
            managementPage.addModule(module);

            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.connection_string))
            {
                con.Open();
                try
                {
                    string sSQL = "INSERT INTO [tbl_userDetails] " +
                                  "([userID], [moduleName], [moduleCode], [moduleCredit], [moduleHours], [studyHours], [studyHoursRemain])" +
                                  "VALUES ('" + userID + "', '"+ module.name + "', '"+ module.code +"', '"+ (int)module.credits +"', '"+ (int)module.hours + "', '"+ (int)module.selfstudyHours + "', '"+ (int)module.selfHoursLeft +"')";
                    
                    SqlCommand cmd = new SqlCommand(sSQL, con);
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.InsertCommand = cmd;
                    adapter.InsertCommand.ExecuteNonQuery();
                    cmd.Dispose();
                    con.Close();
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("Error adding new module!");
                }
            }

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
