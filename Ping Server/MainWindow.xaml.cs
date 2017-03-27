using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
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

namespace Ping_Server
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<List<TextBlock>> labelLists;
        private List<TextBlock> textBlocksList;
        private List<StackPanel> stackPanelList;
        private List<Server> serverList;
        private DateTime errorReported = DateTime.Now;
        private bool firstError = true;

        public bool checkPing = true;


        public MainWindow()
        {
            InitializeComponent();
            getServers();
            initialiseUI();
            runPing();
        }

        /// <summary>
        /// Does a ping async on all servers in serverList
        /// Updates UI with results of Ping
        /// </returns>
        public async Task runPing()
        {


            //Updating UI
            while (checkPing)
            {

                int count = 0;
                foreach (Server server in serverList)
                {
                    List<TextBlock> textBlockTemp = labelLists.ElementAt(count);
                    StackPanel stackPanelTemp = stackPanelList.ElementAt(count);
                    Ping ping = new Ping();
                    long latency = 999;
                    string reply = "failed";

                    try
                    {
                        //Doing ping
                        PingReply pingReply = await ping.SendPingAsync(server.Address, 500);
                        reply = "" + pingReply.Status;
                        latency = pingReply.RoundtripTime;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error during ping request.\n" + e.Message, "Ping Request");
                        break;
                    }

                    textBlockTemp.ElementAt(0).Text = server.Name + "\t";
                    textBlockTemp.ElementAt(1).Text = server.Address + "\t";
                    textBlockTemp.ElementAt(2).Text = reply + "\t";
                    textBlockTemp.ElementAt(3).Text = latency + "ms\t";

                    if (reply.Equals("Success"))
                    {
                        var brush = new BrushConverter();
                        stackPanelTemp.Background = (Brush)brush.ConvertFrom("#32CD32");
                    }
                    else
                    {
                        TimeSpan timeSpan = new TimeSpan();
                        timeSpan = errorReported - DateTime.Now;
                        Reporter report = new Reporter();

                        if (firstError)
                        {
                            report.sendReport(server.Name);
                            firstError = false;
                            errorReported = DateTime.Now;
                        }
                        if (timeSpan.TotalMinutes > 10)
                        {
                            report.sendReport(server.Name);
                            errorReported = DateTime.Now;
                        }

                        var brush = new BrushConverter();
                        stackPanelTemp.Background = (Brush)brush.ConvertFrom("#DC143C");
                    }

                    count++;
                }//-- end -- foreach
                await Task.Delay(5000);
            }//-- end -- while
        }

        /// <summary>
        /// Populates serverList
        /// </summary>
        public void getServers()
        {
            try
            {
                using (StreamReader sr = new StreamReader("servers.txt", true))
                {
                    serverList = new List<Server>();
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] lineSplit = line.Split('#');

                        var server = new Server();
                        server.Name = lineSplit[0];
                        server.Address = lineSplit[1];

                        serverList.Add(server);
                    }//-- end -- while line
                }//-- end -- using StreamReader
            }
            catch (Exception e)
            {
                MessageBox.Show("Error readingin the file.\n" + e, "File Read");
            }
        }

        /// <summary>
        /// Adding server elements to the UI
        /// Populates labelLists & textBlocksLists 
        /// for use during ping method
        /// </summary>
        public void initialiseUI()
        {
            StackPanel stack;
            labelLists = new List<List<TextBlock>>();
            stackPanelList = new List<StackPanel>();
            int rowCount = 0;
            int columnCount = 0;

            foreach (Server s in serverList)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (i == 0)
                    {
                        //Instantiating and initialising Text
                        stack = new StackPanel();
                        TextBlock name = new TextBlock();
                        name.Text = "Server Name:\t";
                        TextBlock address = new TextBlock();
                        address.Text = "Address:\t";
                        TextBlock reply = new TextBlock();
                        reply.Text = "Reply:\t";
                        TextBlock latency = new TextBlock();
                        latency.Text = "Latency:\t";

                        //adding ui elements to stackpanel
                        stack.Children.Add(name);
                        stack.Children.Add(address);
                        stack.Children.Add(reply);
                        stack.Children.Add(latency);
                        stack.Margin = new Thickness(0, 5, 0, 5);

                        // adding ui elements to grid
                        try
                        {
                            Grid1.ColumnDefinitions.Add(new ColumnDefinition());
                            Grid.SetColumn(stack, columnCount);
                            Grid1.ColumnDefinitions[columnCount].Width = GridLength.Auto;

                            Grid1.RowDefinitions.Add(new RowDefinition());
                            Grid.SetRow(stack, rowCount);
                            Grid1.Children.Add(stack);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }

                    }//-- end -- column if statement
                    else
                    {
                        stack = new StackPanel();
                        textBlocksList = new List<TextBlock>();
                        TextBlock nameResult = new TextBlock();
                        nameResult.Text = "N/A";
                        textBlocksList.Add(nameResult);
                        TextBlock addressResult = new TextBlock();
                        addressResult.Text = "0.0.0.0";
                        textBlocksList.Add(addressResult);
                        TextBlock replyResult = new TextBlock();
                        replyResult.Text = "N/A";
                        textBlocksList.Add(replyResult);
                        TextBlock latencyResult = new TextBlock();
                        latencyResult.Text = "999ms";
                        textBlocksList.Add(latencyResult);

                        stack.Children.Add(nameResult);
                        stack.Children.Add(addressResult);
                        stack.Children.Add(replyResult);
                        stack.Children.Add(latencyResult);
                        stack.Margin = new Thickness(0, 5, 0, 5);
                        stackPanelList.Add(stack);

                        // adding ui elements to grid
                        try
                        {
                            Grid1.ColumnDefinitions.Add(new ColumnDefinition());
                            Grid.SetColumn(stack, columnCount + 1);
                            Grid1.ColumnDefinitions[columnCount + 1].Width = GridLength.Auto;

                            Grid1.RowDefinitions.Add(new RowDefinition());
                            Grid.SetRow(stack, rowCount);
                            Grid1.Children.Add(stack);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }

                        textBlocksList.Add(latencyResult);
                        labelLists.Add(textBlocksList);
                    }//-- end -- else
                }//-- end -- column for loop

                if (rowCount == 3)
                {
                    rowCount = 0;
                    columnCount = columnCount + 2;
                }
                else
                {
                    rowCount++;
                }

            }//-- end -- foreach
        }//-- end -- initialiseUI()

        public void clearUI()
        {
            Grid1.DataContext = null;
            Grid1.DataContext = new Grid();
            textBlocksList.Clear();
            labelLists.Clear();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddServerWindow addServer = new AddServerWindow();
            checkPing = false;
            this.Close();
            clearUI();
            addServer.Show();
        }
    }
}
