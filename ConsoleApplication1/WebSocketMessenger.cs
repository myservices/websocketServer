using System;
using WebSocketSharp.Server;

namespace WebsocketServer
{
    abstract class WebSocketMessenger: WebSocketBehavior
    {
        public abstract void SetResponseTextFunc(Func<String> func);
    }
}
