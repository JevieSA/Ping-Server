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
        private List<Server> serverList;

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
            while (true)
            {
                int count = 0;
                foreach (Server server in serverList)
                {
                    List<TextBlock> temp = labelLists.ElementAt(count);
                    Ping ping = new Ping();
                    long latency = 999;
                    string reply = "failed";

                    try
                    {
                        //Doing ping
                        PingReply pingReply = await ping.SendPingAsync(server.Address);
                        reply = "" + pingReply.Status;
                        latency = pingReply.RoundtripTime;
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Error during ping request.\n" + e.Message, "Ping Request");
                        break;
                    }

                    temp.ElementAt(0).Text = server.Name;
                    temp.ElementAt(1).Text = server.Address;
                    temp.ElementAt(2).Text = reply;
                    temp.ElementAt(3).Text = latency + "ms";

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
            int count = 0;

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
                            Grid.SetColumn(stack, 0);
                            Grid1.ColumnDefinitions[0].Width = GridLength.Auto;

                            Grid1.RowDefinitions.Add(new RowDefinition());
                            Grid.SetRow(stack, count);
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

                        // adding ui elements to grid
                        try
                        {
                            Grid1.ColumnDefinitions.Add(new ColumnDefinition());
                            Grid.SetColumn(stack, 1);
                            Grid1.ColumnDefinitions[1].Width = GridLength.Auto;                            
                            
                            Grid1.RowDefinitions.Add(new RowDefinition());
                            Grid.SetRow(stack, count);
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
                count++;
            }//-- end -- foreach
        }//-- end -- initialiseUI()
    }
}
