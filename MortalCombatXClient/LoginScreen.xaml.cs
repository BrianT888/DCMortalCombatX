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
using System.Windows.Shapes;

namespace MortalCombatXClient
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();

            // can we set up url and connection here?
            //string URL = "net.tcp://0.0.0.0:8100/DataService";
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = EnteredText.Text;
            if (!string.IsNullOrEmpty(username))
            {
                //bool unique = client.Login(username);
                //if (unique)
                //{
                //    MessageBox.Show("Login was successful");
                   
                //}
                //else
                //{
                //    MessageBox.Show("Username is taken, choose different username");
                //}
            }
            else
            {
                MessageBox.Show("Enter username");
            }

        }
    }
}
