using System;
using System.Collections.Generic;
using System.IO;
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

namespace Ping_Server
{
    /// <summary>
    /// Interaction logic for AddServerWindow.xaml
    /// </summary>
    public partial class AddServerWindow : Window
    {
        public AddServerWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text;
            string address = txtAddress.Text;
            string line = name + "#" + address;
            writeToFile(line);
            
            this.Hide();

            MainWindow mainWindow = new MainWindow();
            mainWindow.checkPing = true;
            mainWindow.Show();      
        }

        public void writeToFile(string line)
        {
            try
            {
                using (StreamWriter sr = new StreamWriter("servers.txt", true))
                {
                    sr.WriteLine(line);
                }
            }//-- end -- try
            catch(Exception e)
            {
                MessageBox.Show("Error writing server to file\n" + e.Message, "Write to File");
            }//-- end -- catch
        }

    }
}
