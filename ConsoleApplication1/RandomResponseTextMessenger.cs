using System;
using System.Timers;
using WebSocketSharp;

namespace WebsocketServer
{
    class RandomResponseTextMessenger: WebSocketMessenger
    {
        private int repeatTimeFrame = 20000;
        private Timer aTimer=null;
        private Func<String> responseText = null;

        
        public override void SetResponseTextFunc(Func<String> func)
        {
            responseText = func;
        }

        public void AcceptVisitor(IVisitor visitor)
        {
            visitor.Visit(this);
        }

        private void SetTimer(RandomResponseTextMessenger wsb)
        {
            aTimer = new System.Timers.Timer(repeatTimeFrame);
            aTimer.Elapsed += delegate
            {
                OnTimedEvent(wsb);
            };
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
            aTimer.Start();
        }

        private void OnTimedEvent(RandomResponseTextMessenger wsb)
        {
            wsb.Send(responseText());
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            Console.WriteLine("Client connected "+this.ID);
            SetTimer(this);
        }

        protected override void OnClose(CloseEventArgs a)
        {
            aTimer.Stop();
            Console.WriteLine("Client disconnected " + this.ID);
            base.OnClose(a);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.IsText)
            {
                Console.WriteLine(e.Data);
                Send(responseText());
            }
        }
    }
}
