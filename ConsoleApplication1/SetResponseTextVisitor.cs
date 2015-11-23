using System;

namespace WebsocketServer
{
    class SetResponseTextVisitor : IVisitor
    {
        private static String GetRandomString()
        {
            return System.Guid.NewGuid().ToString();
        }

        public void Visit(RandomResponseTextMessenger wsb)
        {
            wsb.SetResponseTextFunc(GetRandomString);
        }
    }
}
