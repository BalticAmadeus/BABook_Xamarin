
using System.Collections.Generic;
using Microsoft.AspNet.SignalR.Client;

namespace BaBookApp.Resources.Functions.Chat
{
    public class Chat
    {
        public void BeginChat()
        {
            var Chat = new HubConnection("http://www.contoso.com/");

        }
    }
}