using System;
using System.Collections.Generic;
using System.IO;

namespace Ping_Server
{
    class Server
    {
        public string Name { get; set; }
        public string Address { get; set; }

        /// <summary>
        /// Populates a serverList
        /// </summary>
        public List<Server> getServers()
        {
            List<Server> serverList = new List<Server>();

            try
            {
                using (StreamReader sr = new StreamReader("servers.txt", true))
                {
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
                System.Windows.MessageBox.Show("Error readingin the file.\n" + e, "File Read");
            }

            return serverList;
        }
    }
}
