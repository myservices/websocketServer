using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace WebsocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var wssv = new WebSocketServer(8025);
            wssv.AddWebSocketService<WebSocketMessenger>("/websockets/messenger", MessengerRandomTextInitializer);
            wssv.Start();
            if (wssv.IsListening)
            {
                Console.WriteLine("Listening on port {0}, and providing WebSocket services:", wssv.Port);
                foreach (var path in wssv.WebSocketServices.Paths)
                    Console.WriteLine("- {0}", path);
            }

            Console.WriteLine("\nPress Enter key to stop the server...");
            Console.ReadLine();

            wssv.Stop();
        }

        private static WebSocketMessenger MessengerRandomTextInitializer()
        {
            RandomResponseTextMessenger messenger = new RandomResponseTextMessenger();
            SetResponseTextVisitor visitor = new SetResponseTextVisitor();
            messenger.AcceptVisitor(visitor);
            return messenger;
        }
    }
    
}
