
using System.Collections.Generic;
using Microsoft.AspNet.SignalR.Client;

namespace BaBookApp.Resources.Functions.Chat
{
    public class Chat
    {
        public void a()
        {
            var a = new HubConnection("http://www.contoso.com/");
        }
    }
}