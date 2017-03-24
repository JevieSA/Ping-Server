using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;

namespace Ping_Server
{
    class Server
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public PingReply PingReply { get; set; }
    }
}
