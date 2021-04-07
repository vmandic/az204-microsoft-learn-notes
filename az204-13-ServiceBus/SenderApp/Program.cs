using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

namespace SenderApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting app...");

            if (!System.IO.File.Exists("serviceBusConnectionString"))
            {
                throw new System.IO.FileNotFoundException("Could not find required file 'serviceBusConnectionString'");
            }

            var sbConnectionString = System.IO.File.ReadAllText("serviceBusConnectionString");
            Console.WriteLine("Using service bus connection string: " + sbConnectionString);

            // NOTE: send the message to SB "salesmessages" queue
            var queueClient = new QueueClient(sbConnectionString, "salesmessages");

            string message = "Sure would like a large pepperoni!";
            var encodedMessage = new Message(System.Text.Encoding.UTF8.GetBytes(message));

            await queueClient.SendAsync(encodedMessage);

            // NOTE: login to azure portal and under Service Bus Queue explorer observe that the message is enqueued

            Console.WriteLine("Message sent, check in Azure Portal, press any key to exit app...");
            Console.ReadKey();
        }
    }
}