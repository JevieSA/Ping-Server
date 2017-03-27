using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Ping_Server
{
    class ServerPinger
    {
        /// <summary>
        /// Does a ping async on all servers in serverList
        /// Updates UI with results of Ping
        /// </returns>
        public async Task runPing(List<List<TextBlock>> labelLists, List<StackPanel> stackPanelList, List<Server> serverList, bool checkPing, DateTime errorReported, bool firstError)
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
                        PingReply pingReply = await ping.SendPingAsync(server.Address, 1000);
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
                    }//-- end -- else

                    count++;
                }//-- end -- foreach
                await Task.Delay(5000);
            }//-- end -- while
        }
    }
}
