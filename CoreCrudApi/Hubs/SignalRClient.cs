using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
namespace CoreCrudApi.Hubs
{
    public class SignalRClient
    {
        private HubConnection _hub;
        //public event EventHandler<ValueChangedEventArgs> ValueChanged;
        private string hubUrl = "http://localhost:8081/chatHub";
        public HubConnection Hub { get { return _hub; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignalRClient"/> class.
        /// </summary>
        public SignalRClient()
        {

            try
            {
                Debug.WriteLine("SignalR Initialized...");
                InitializeSignalRAsync().ContinueWith(task =>
                {
                    Debug.WriteLine("SignalR Started...");
                });
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }


        }



        /// <summary>
        /// Initializes SignalR.
        /// </summary>
        public async Task InitializeSignalRAsync()
        {

            try
            {
                _hub = new HubConnectionBuilder()
                           .WithUrl(hubUrl)
                           .Build();
                //_hub.On<string, string>("ReceiveMessage", (user, message) => ValueChanged?.Invoke(this, new ValueChangedEventArgs(user, message)));

                _hub.On<string, string>("ReceiveMessage", (user, message) => ReciveMessage(user, message));
                await _hub.StartAsync();
            }
            catch (Exception ex)
            {

                Console.WriteLine("An error occurred {0}", ex.Message);

            }

        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="state">The state.</param>
        public async Task SendMessage(string user, string message)
        {
            try
            {
                await _hub?.InvokeAsync("SendMessage", user, message);
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

        //Receive method
        public void ReciveMessage(string user, string message)
        {
            Debug.WriteLine("SignalR message:", message);
        }
    }


}
