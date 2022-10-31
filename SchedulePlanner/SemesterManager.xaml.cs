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
using System.Diagnostics;
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
        int userID;
        string user;
        List<Module> moduleList = new List<Module>();

        public SemesterManager(int numWeeksIn, int Id, string username)
        {
            InitializeComponent();
            numWeeks = numWeeksIn;
            userID = Id;
            user = username;
            userBox.Text = String.Format("User: {0}", user);
            weeksText.Text = String.Format("Weeks: {0}", numWeeks);

            // Fetch modules stored in db
            QueryModules(userID);
        }

        // --- Button Click events --- //

        // Handle removal of modules
        private void rmvClick(object sender, RoutedEventArgs e)
        {
            if (moduleListBox.SelectedItem != null)
            {
                int moduleIndex = moduleListBox.SelectedIndex;
                Module module = moduleList[moduleIndex];
                moduleListBox.Items.RemoveAt(moduleIndex);
                moduleList.RemoveAt(moduleIndex);
                // remove from db
                using (SqlConnection con = new SqlConnection(Properties.Settings.Default.connection_string))
                {
                    con.Open();
                    try
                    {
                        string sql = "DELETE FROM [tbl_userDetails] " +
                                     "WHERE [moduleName] = '" + module.name + "' AND [moduleCode] = '" + module.code + "'";
                        SqlCommand command = new SqlCommand(sql, con);
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        adapter.DeleteCommand = command;
                        adapter.DeleteCommand.ExecuteNonQuery();
                        command.Dispose();
                        con.Close();
                    }
                    catch
                    {
                        System.Diagnostics.Debug.WriteLine("Error removing module!");
                    }
                }
            }
        }

        // Handle addition of modules
        private void addClick(object sender, RoutedEventArgs e)
        {
            AddModulePage amPage = new AddModulePage(this, userID, numWeeks);
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

            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.connection_string))
            {
                con.Open();
                try
                {
                    module.selfHoursLeft = Math.Max(module.selfHoursLeft - hours, 0);
                    string sql = "UPDATE [tbl_userDetails] " +
                                 "SET [studyHoursRemain] = '" + module.selfHoursLeft + "'" +
                                 "WHERE [moduleName] = '" + module.name + "' AND [moduleCode] = '" + module.code + "'";
                    SqlCommand command = new SqlCommand(sql, con);
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.UpdateCommand = command;
                    adapter.UpdateCommand.ExecuteNonQuery();
                    command.Dispose();
                    con.Close();
                } catch
                {
                    Debug.WriteLine("Error updating module!");
                }
                }

            ListBoxItem li = (ListBoxItem)moduleListBox.Items.GetItemAt(index);
            li.Content = String.Format("{0} - {1} | Study hours: {2} / {3}", module.name, module.code, (int)module.selfHoursLeft, (int)module.selfstudyHours);
        }

        private void displayModule(Module module)
        {
            // Logic for displaying a module
            ListBoxItem listBoxItem = new ListBoxItem();
            listBoxItem.Content = String.Format("{0} - {1} | Study hours: {2} / {3}", module.name, module.code, (int)module.selfHoursLeft, (int)module.selfstudyHours);
            moduleListBox.Items.Add(listBoxItem);
        }

        private void QueryModules(int Id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Properties.Settings.Default.connection_string))
                {
                    con.Open();
                    string sql = "select [moduleName], [moduleCode], [moduleCredit], [moduleHours], [studyHours], [studyHoursRemain] " +
                                 "from [tbl_userDetails] " +
                                 "where [userID] = '" + Id + "'";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {

                        SqlDataReader dr = cmd.ExecuteReader();
                        
                        while (dr.Read())
                        {
                            Debug.WriteLine(dr.FieldCount);
                            string name = dr.GetString(0);
                            string code = dr.GetString(1);
                            int credit = dr.GetInt32(2);
                            int hours = dr.GetInt32(3);
                            int studyHours = dr.GetInt32(4);
                            int selfHoursLeft = dr.GetInt32(5);
                            Module m = new Module(code, name, credit, hours);
                            m.selfstudyHours = studyHours;
                            m.selfHoursLeft = selfHoursLeft;
                            addModule(m);
                        }
                    }
                }
            } catch
            {
                Debug.WriteLine("Error fetching modules!");
            }
        }
    }
}
