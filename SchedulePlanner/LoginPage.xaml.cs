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
using System.Data.SqlClient;
using System.Data;
using SchedulePlanner.Properties;
using ModulesLibrary;
using System.Diagnostics;

namespace SchedulePlanner
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        TextBox username;
        TextBox password;
        NavigationService ns;
        RsaEncryption rsaEncryption;

        public LoginPage()
        {
            InitializeComponent();
            username = usernameBox;
            password = passwordBox;
            ns = NavigationService.GetNavigationService(this);
            rsaEncryption = new RsaEncryption();
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.connection_string))
            {
                con.Open();
                try
                {
                    // AND [password] = '" + password.Text + "'
                    string sSQL = "SELECT * FROM [tbl_users] WHERE [username] = '" + username.Text + "'";
                    using (SqlCommand cmd = new SqlCommand(sSQL, con))
                    {

                        SqlDataReader dr = cmd.ExecuteReader();
                        while (dr.Read())
                        {
                            int id = dr.GetInt32(0);
                            string user = dr.GetString(1);
                            string pass = dr.GetString(2);
                            var sd = dr[3];
                            var ed = dr[4];
                            Debug.WriteLine(pass + " || " + password.Text);
                            if (password.Text == rsaEncryption.Decrypt(pass))
                            {
                                if (sd is not System.DBNull && ed is not System.DBNull)
                                {
                                    DateTime start = (DateTime)sd;
                                    DateTime end = (DateTime)ed;
                                    int numWeeks = (int)((TimeSpan)(end - start)).TotalDays / 7;
                                    int cols = dr.FieldCount;
                                    // navigate to semester manager directly
                                    SemesterManager sm = new SemesterManager(numWeeks, id, user);
                                    this.NavigationService.Navigate(sm);
                                }
                                else
                                {
                                    // navigate to start page first to get semester length
                                    StartPage sp = new StartPage(id, user);
                                    this.NavigationService.Navigate(sp);
                                }
                            }
                        }
                    }
                } catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        private void SignUpButtonClick(object sender, RoutedEventArgs e)
        {
            SignUpPage signUp = new SignUpPage(rsaEncryption);
            this.NavigationService.Navigate(signUp);
        }
    }
}
