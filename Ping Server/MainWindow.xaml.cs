using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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
        private Server server;
        private DateTime errorReported = DateTime.Now;
        private bool firstError = true;

        public bool checkPing = true;


        public MainWindow()
        {
            InitializeComponent();

            server = new Server();
            serverList = server.getServers();
            initialiseUI();

            ServerPinger pinger = new ServerPinger();
            pinger.runPing(labelLists, stackPanelList, serverList, checkPing, errorReported, firstError);
        }

        /// <summary>
        /// Adding server elements to the UI
        /// Populates labelLists & textBlocksLists 
        /// for use during ping method
        /// </summary>
        public void initialiseUI()
        {
            stackPanelList = new List<StackPanel>();
            StackPanel stack;
            labelLists = new List<List<TextBlock>>();
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


        /// <summary>
        /// Clears the UI so that a new server can be added
        /// </summary>
        public void clearUI()
        {
            Grid1.DataContext = null;
            Grid1.DataContext = new Grid();
            textBlocksList.Clear();
            labelLists.Clear();
        }

        /// <summary>
        /// Calls the AddServerWindow screen
        /// </summary>
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
