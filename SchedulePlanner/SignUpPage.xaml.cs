using ModulesLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
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
    /// Interaction logic for SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        RsaEncryption rsaEncryption;
        public SignUpPage(RsaEncryption rsa)
        {
            InitializeComponent();
            rsaEncryption = rsa;
        }

        private void CreateButtonClick(object sender, RoutedEventArgs e)
        {
            if (usernameBox.Text.Equals(""))
            {
                errorText.Text = "Module name must be entered!";
                return;
            }

            if (passwordBox.Text.Equals(""))
            {
                errorText.Text = "Module code must be entered!";
                return;
            }

            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.connection_string))
            {
                con.Open();
                try
                {
                    string encyptedPassword = rsaEncryption.Encrypt(passwordBox.Text);
                    Debug.WriteLine("Encrypted text: " + encyptedPassword);
                    Debug.WriteLine("Decrypted text: " + rsaEncryption.Decrypt(encyptedPassword));
                    string sSQL = "INSERT INTO [tbl_users] ([username], [password])" +
                                  "VALUES ('" + usernameBox.Text + "', '" + encyptedPassword + "')";

                    SqlCommand cmd = new SqlCommand(sSQL, con);
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.InsertCommand = cmd;
                    adapter.InsertCommand.ExecuteNonQuery();
                    cmd.Dispose();
                    con.Close();
                    this.NavigationService.GoBack();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            }
        }
    }
}
